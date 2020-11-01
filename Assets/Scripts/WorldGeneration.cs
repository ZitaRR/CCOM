using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WorldGeneration : GameManager
{   
    [SerializeField]
    private GameObject node;

    private new Renderer renderer;
    private int size;

    public Node[,] Map { get; private set; }

    private void Start()
    {
        renderer = GetComponent<Renderer>();
        size = (int)renderer.bounds.size.x + 1;
        Map = new Node[size, size];
        transform.position = new Vector3(size / 2, 0, size / 2);

        for (int z = 0; z < size; z++)
        {
            for (int x = 0; x < size; x++)
            {
                Map[z, x] = new Node(node, x, z, true);
            }
        }

        var nodes = FindPath(new Vector3(4, 0, 3), new Vector3(7, 0, 7));
        foreach (var node in nodes)
        {
            node.ChangeColour();
        }
    }

    public Stack<Node> FindPath(Vector3 start, Vector3 target)
    {
        int x = Mathf.RoundToInt(start.x);
        int z = Mathf.RoundToInt(start.z);
        Node nStart = Map[z, x];

        x = Mathf.RoundToInt(target.x);
        z = Mathf.RoundToInt(target.z);
        Node nTarget = Map[z, x];

        var path = new Stack<Node>();
        var opened = new List<Node>();
        var closed = new List<Node>();
        var adjacencies = new List<Node>();
        Node current = nStart;
        path.Push(nStart);

        opened.Add(current);
        while(opened.Count != 0 && !closed.Exists(t => t.X == nTarget.X && t.Z == nTarget.Z))
        {
            current = opened[0];
            opened.Remove(current);
            closed.Add(current);
            adjacencies = GetAdjacentNodes(current);

            foreach (var node in adjacencies)
            {
                if (closed.Contains(node) || !node.Walkable)
                    continue;
                if (opened.Contains(node))
                    continue;

                node.Parent = current;
                node.Distance = Mathf.Abs(node.X - nTarget.X) + Mathf.Abs(node.Z - nTarget.Z);
                node.Cost = node.Weight + node.Parent.Cost;
                opened.Add(node);
                opened = opened.OrderBy(n => n.F).ToList();
            }
        }

        if (!closed.Exists(n => n.X == nTarget.X && n.Z == nTarget.Z))
            return null;

        Node temp = closed[closed.IndexOf(current)];
        if (temp == null)
            return null;
        do
        {
            path.Push(temp);
            temp = temp.Parent;
        } while (temp != nStart && temp != null);
        return path;
    }

    public List<Node> GetAdjacentNodes(Node node)
    {
        var list = new List<Node>();
        if (node.X == 0)
            list.Add(Map[node.Z, node.X + 1]);
        else if (node.X == size - 1)
            list.Add(Map[node.Z, node.X - 1]);
        else
        {
            list.Add(Map[node.Z, node.X + 1]);
            list.Add(Map[node.Z, node.X - 1]);
        }
    
        if (node.Z == 0)
            list.Add(Map[node.Z + 1, node.X]);
        else if (node.Z == size - 1)
            list.Add(Map[node.Z - 1, node.X]);
        else
        {
            list.Add(Map[node.Z + 1, node.X]);
            list.Add(Map[node.Z - 1, node.X]);
        }
    
        return list;
    }
}
