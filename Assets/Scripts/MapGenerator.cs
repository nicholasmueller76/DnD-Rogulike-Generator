using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public enum NodeType { stairs, standardCombat, eliteCombat, chanceRoom, restSite, lootRoom, adventurerShop, alchemistShop, blacksmithShop, wizardShop, bossCombat, superEliteCombat }

public class MapGenerator : MonoBehaviour
{
    private List<float> layerDistances;
    
    // ALL nodes by layer:
    private List<List<Node>> nodes = new List<List<Node>>();

    private GameObject mapParent;

    [SerializeField]
    [Range(3, 10)]
    int linePointsCount;

    [SerializeField]
    float offsetFromNodes;

    [SerializeField]
    GameObject nodePrefab;

    [SerializeField]
    GameObject mapParentPrefab;

    [SerializeField]
    GameObject linePrefab;

    [SerializeField]
    MapIcons mapIcons;

    [SerializeField]
    MapConfig config;

    private Dictionary<Node,GameObject> mapNodes = new();

    public void GenerateFloor()
    {
        GenerateLayerDistances();
        for (int i = 0; i < config.layers.Count; i++)
            PlaceLayer(i);
        
        List<List<Vector2Int>> paths = GeneratePaths();

        RandomizeNodePositions();

        SetUpConnections(paths);

        RemoveCrossConnections();

        // select all the nodes with connections:
        List<Node> nodesList = nodes.SelectMany(n => n).Where(n => n.incoming.Count > 0 || n.outgoing.Count > 0).ToList();

        LimitSuperElites(nodesList);

        ClearMap();

        mapParent = Instantiate(mapParentPrefab);

        foreach(Node n in nodesList)
        {
            GameObject mapNode = Instantiate(nodePrefab, mapParent.transform);
            foreach(MapIconBlueprint i in mapIcons.icons)
            {
                if(i.nodeType == n.nodeType) mapNode.GetComponent<SpriteRenderer>().sprite = i.sprite;
            }

            if (n.nodeType == NodeType.bossCombat) mapNode.transform.localScale *= 1.5f;
            mapNode.transform.localPosition = n.position;
            mapNodes.Add(n, mapNode);
        }

        foreach(Node n in nodesList)
        {
            foreach(Node connection in n.outgoing)
            {
                AddLineConnection(mapNodes[n], mapNodes[connection]);
            }
        }
    }

    private void LimitSuperElites(List<Node> nodes)
    {
        bool containsSuperElite = false;
        foreach(Node n in nodes)
        {
            if(!containsSuperElite)
            {
                if (n.nodeType == NodeType.superEliteCombat) containsSuperElite = true;
            }
            else
            {
                if (n.nodeType == NodeType.superEliteCombat) n.nodeType = NodeType.eliteCombat;
            }
        }
    }

    private void ClearMap()
    {
        if(mapParent != null) Destroy(mapParent);

        nodes.Clear();
        mapNodes.Clear();
    }

    protected virtual void AddLineConnection(GameObject from, GameObject to)
    {
        if (linePrefab == null) return;

        GameObject lineObject = Instantiate(linePrefab, mapParent.transform);
        LineRenderer lineRenderer = lineObject.GetComponent<LineRenderer>();
        Vector3 fromPoint = from.transform.position +
                            (to.transform.position - from.transform.position).normalized * offsetFromNodes;

        Vector3 toPoint = to.transform.position +
                          (from.transform.position - to.transform.position).normalized * offsetFromNodes;

        // drawing lines in local space:
        lineObject.transform.position = fromPoint;
        lineRenderer.useWorldSpace = false;

        // line renderer with 2 points only does not handle transparency properly:
        lineRenderer.positionCount = linePointsCount;
        for (int i = 0; i < linePointsCount; i++)
        {
            lineRenderer.SetPosition(i,
                Vector3.Lerp(Vector3.zero, toPoint - fromPoint, (float)i / (linePointsCount - 1)));
        }

        DottedLineRenderer dottedLine = lineObject.GetComponent<DottedLineRenderer>();
        if (dottedLine != null) dottedLine.ScaleMaterial();
    }

    private void GenerateLayerDistances()
    {
        layerDistances = new List<float>();
        foreach (MapLayer layer in config.layers)
            layerDistances.Add(layer.distanceFromPreviousLayer.GetValue());
    }

    private float GetDistanceToLayer(int layerIndex)
    {
        if (layerIndex < 0 || layerIndex > layerDistances.Count) return 0f;

        return layerDistances.Take(layerIndex + 1).Sum();
    }

    private void PlaceLayer(int layerIndex)
    {
        MapLayer layer = config.layers[layerIndex];
        List<Node> nodesOnThisLayer = new List<Node>();

        // offset of this layer to make all the nodes centered:
        float offset = layer.nodesApartDistance * config.GridWidth / 2f;

        for (int i = 0; i < config.GridWidth; i++)
        {
            var supportedRandomNodeTypes =
                config.randomNodes.Where(t => config.nodeTypes.Any(b => b == t)).ToList();
            NodeType nodeType = UnityEngine.Random.Range(0f, 1f) < layer.randomizeNodes && supportedRandomNodeTypes.Count > 0
                ? supportedRandomNodeTypes.Random()
                : layer.nodeType;
            Node node = new Node(nodeType, new Vector2Int(i, layerIndex))
            {
                position = new Vector2(-offset + i * layer.nodesApartDistance, GetDistanceToLayer(layerIndex))
            };
            nodesOnThisLayer.Add(node);
        }

        nodes.Add(nodesOnThisLayer);
    }

    private void RandomizeNodePositions()
    {
        for (int index = 0; index < nodes.Count; index++)
        {
            List<Node> list = nodes[index];
            MapLayer layer = config.layers[index];
            float distToNextLayer = index + 1 >= layerDistances.Count
                ? 0f
                : layerDistances[index + 1];
            float distToPreviousLayer = layerDistances[index];

            foreach (Node node in list)
            {
                float xRnd = UnityEngine.Random.Range(-0.5f, 0.5f);
                float yRnd = UnityEngine.Random.Range(-0.5f, 0.5f);

                float x = xRnd * layer.nodesApartDistance;
                float y = yRnd < 0 ? distToPreviousLayer * yRnd : distToNextLayer * yRnd;

                node.position += new Vector2(x, y) * layer.randomizePosition;
            }
        }
    }

    private void SetUpConnections(List<List<Vector2Int>> paths)
    {
        foreach (List<Vector2Int> path in paths)
        {
            for (int i = 0; i < path.Count - 1; ++i)
            {
                Node node = GetNode(path[i]);
                Node nextNode = GetNode(path[i + 1]);
                node.AddOutgoing(nextNode);
                nextNode.AddIncoming(node);
            }
        }
    }

    private void RemoveCrossConnections()
    {
        for (int i = 0; i < config.GridWidth - 1; ++i)
            for (int j = 0; j < config.layers.Count - 1; ++j)
            {
                Node node = GetNode(new Vector2Int(i, j));
                if (node == null || node.HasNoConnections()) continue;
                Node right = GetNode(new Vector2Int(i + 1, j));
                if (right == null || right.HasNoConnections()) continue;
                Node top = GetNode(new Vector2Int(i, j + 1));
                if (top == null || top.HasNoConnections()) continue;
                Node topRight = GetNode(new Vector2Int(i + 1, j + 1));
                if (topRight == null || topRight.HasNoConnections()) continue;

                // Debug.Log("Inspecting node for connections: " + node.point);
                if (!node.outgoing.Any(element => element.Equals(topRight))) continue;
                if (!right.outgoing.Any(element => element.Equals(top))) continue;

                //Debug.Log("Found a cross node: " + node.point);

                // we managed to find a cross node:
                // 1) add direct connections:
                node.AddOutgoing(top);
                top.AddIncoming(node);

                right.AddOutgoing(topRight);
                topRight.AddIncoming(right);

                float rnd = UnityEngine.Random.Range(0f, 1f);
                if (rnd < 0.2f)
                {
                    // remove both cross connections:
                    // a) 
                    node.RemoveOutgoing(topRight);
                    topRight.RemoveIncoming(node);
                    // b) 
                    right.RemoveOutgoing(top);
                    top.RemoveIncoming(right);
                }
                else if (rnd < 0.6f)
                {
                    // a) 
                    node.RemoveOutgoing(topRight);
                    topRight.RemoveIncoming(node);
                }
                else
                {
                    // b) 
                    right.RemoveOutgoing(top);
                    top.RemoveIncoming(right);
                }
            }
    }

    private Node GetNode(Vector2Int p)
    {
        if (p.y >= nodes.Count) return null;
        if (p.x >= nodes[p.y].Count) return null;

        return nodes[p.y][p.x];
    }

    private Vector2Int GetFinalNode()
    {
        int y = config.layers.Count - 1;
        if (config.GridWidth % 2 == 1)
            return new Vector2Int(config.GridWidth / 2, y);

        return UnityEngine.Random.Range(0, 2) == 0
            ? new Vector2Int(config.GridWidth / 2, y)
            : new Vector2Int(config.GridWidth / 2 - 1, y);
    }

    private List<List<Vector2Int>> GeneratePaths()
    {
        Vector2Int finalNode = GetFinalNode();
        var paths = new List<List<Vector2Int>>();
        int numOfStartingNodes = config.numOfStartingNodes.GetValue();
        int numOfPreBossNodes = config.numOfPreBossNodes.GetValue();

        List<int> candidateXs = new List<int>();
        for (int i = 0; i < config.GridWidth; i++)
            candidateXs.Add(i);

        candidateXs.Shuffle();
        IEnumerable<int> startingXs = candidateXs.Take(numOfStartingNodes);
        List<Vector2Int> startingPoints = (from x in startingXs select new Vector2Int(x, 0)).ToList();

        candidateXs.Shuffle();
        IEnumerable<int> preBossXs = candidateXs.Take(numOfPreBossNodes);
        List<Vector2Int> preBossPoints = (from x in preBossXs select new Vector2Int(x, finalNode.y - 1)).ToList();

        int numOfPaths = Mathf.Max(numOfStartingNodes, numOfPreBossNodes) + Mathf.Max(0, config.extraPaths);
        for (int i = 0; i < numOfPaths; ++i)
        {
            Vector2Int startNode = startingPoints[i % numOfStartingNodes];
            Vector2Int endNode = preBossPoints[i % numOfPreBossNodes];
            List<Vector2Int> path = Path(startNode, endNode);
            path.Add(finalNode);
            paths.Add(path);
        }

        return paths;
    }

    // Generates a random path bottom up.
    private List<Vector2Int> Path(Vector2Int fromPoint, Vector2Int toPoint)
    {
        int toRow = toPoint.y;
        int toCol = toPoint.x;

        int lastNodeCol = fromPoint.x;

        List<Vector2Int> path = new List<Vector2Int> { fromPoint };
        List<int> candidateCols = new List<int>();
        for (int row = 1; row < toRow; ++row)
        {
            candidateCols.Clear();

            int verticalDistance = toRow - row;
            int horizontalDistance;

            int forwardCol = lastNodeCol;
            horizontalDistance = Mathf.Abs(toCol - forwardCol);
            if (horizontalDistance <= verticalDistance)
                candidateCols.Add(lastNodeCol);

            int leftCol = lastNodeCol - 1;
            horizontalDistance = Mathf.Abs(toCol - leftCol);
            if (leftCol >= 0 && horizontalDistance <= verticalDistance)
                candidateCols.Add(leftCol);

            int rightCol = lastNodeCol + 1;
            horizontalDistance = Mathf.Abs(toCol - rightCol);
            if (rightCol < config.GridWidth && horizontalDistance <= verticalDistance)
                candidateCols.Add(rightCol);

            int randomCandidateIndex = UnityEngine.Random.Range(0, candidateCols.Count);
            int candidateCol = candidateCols[randomCandidateIndex];
            Vector2Int nextPoint = new Vector2Int(candidateCol, row);

            path.Add(nextPoint);

            lastNodeCol = candidateCol;
        }

        path.Add(toPoint);

        return path;
    }
}

public static class ShufflingExtension
{

    // not my code!!!!!
    // got it here: http://stackoverflow.com/questions/273313/randomize-a-listt/1262619#1262619 
    private static System.Random rng = new System.Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static T Random<T>(this IList<T> list)
    {
        return list[rng.Next(list.Count)];
    }

    public static T Last<T>(this IList<T> list)
    {
        return list[list.Count - 1];
    }

    public static List<T> GetRandomElements<T>(this List<T> list, int elementsCount)
    {
        return list.OrderBy(arg => Guid.NewGuid()).Take(list.Count < elementsCount ? list.Count : elementsCount)
            .ToList();
    }
}