using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Vector2Int point;
    public List<Node> incoming = new List<Node>();
    public readonly List<Node> outgoing = new List<Node>();
    public NodeType nodeType;
    public Vector2 position;

    public Node(NodeType nodeType, Vector2Int point)
    {
        this.nodeType = nodeType;
        this.point = point;
    }

    public void AddIncoming(Node p)
    {
        if (incoming.Contains(p))
            return;

        incoming.Add(p);
    }

    public void AddOutgoing(Node p)
    {
        if (outgoing.Contains(p))
            return;

        outgoing.Add(p);
    }

    public void RemoveIncoming(Node p)
    {
        incoming.RemoveAll(element => element.Equals(p));
    }

    public void RemoveOutgoing(Node p)
    {
        outgoing.RemoveAll(element => element.Equals(p));
    }

    public bool HasNoConnections()
    {
        return incoming.Count == 0 && outgoing.Count == 0;
    }
}
