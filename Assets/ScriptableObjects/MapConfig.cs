using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MapConfig : ScriptableObject
{
    public List<NodeType> nodeTypes;
    [Tooltip("Nodes that will be used on layers with Randomize Nodes > 0")]
    public List<NodeType> randomNodes = new List<NodeType>();
    public int GridWidth => Mathf.Max(numOfPreBossNodes.max, numOfStartingNodes.max);

    public IntMinMax numOfPreBossNodes;
    public IntMinMax numOfStartingNodes;

    [Tooltip("Increase this number to generate more paths")]
    public int extraPaths;
    public List<MapLayer> layers;
}

[System.Serializable]
public class FloatMinMax
{
    public float min;
    public float max;

    public float GetValue()
    {
        return Random.Range(min, max);
    }
}

[System.Serializable]
public class IntMinMax
{
    public int min;
    public int max;

    public int GetValue()
    {
        return Random.Range(min, max + 1);
    }
}

[System.Serializable]
public class MapLayer
{
    [Tooltip("Default node for this map layer. If Randomize Nodes is 0, you will get this node 100% of the time")]
    public NodeType nodeType;
    public FloatMinMax distanceFromPreviousLayer;
    [Tooltip("Distance between the nodes on this layer")]
    public float nodesApartDistance;
    [Tooltip("If this is set to 0, nodes on this layer will appear in a straight line. Closer to 1f = more position randomization")]
    [Range(0f, 1f)] public float randomizePosition;
    [Tooltip("Chance to get a random node that is different from the default node on this layer")]
    [Range(0f, 1f)] public float randomizeNodes;
}