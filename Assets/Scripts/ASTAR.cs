using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ASTAR : MonoBehaviour
{
    private static WorldGeneration world;

    private void Awake()
    {
        world = GetComponent<WorldGeneration>();
    }

    public static Stack<Node> FindPath(Vector3 start, Vector3 target)
    {
        int x = Mathf.RoundToInt(start.x);
        int z = Mathf.RoundToInt(start.z);
        Node nStart = world.Map[z, x];

        x = Mathf.RoundToInt(target.x);
        z = Mathf.RoundToInt(target.z);
        Node nTarget = world.Map[z, x];

        var path = new Stack<Node>();
        var opened = new List<Node>();
        var closed = new List<Node>();
        var adjacencies = new List<Node>();
        Node current = nStart;

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

        path.Push(nStart);
        return path;
    }

    private static List<Node> GetAdjacentNodes(Node node)
    {
        var list = new List<Node>();
        if (node.X == 0)
            list.Add(world.Map[node.Z, node.X + 1]);
        else if (node.X == world.Size - 1)
            list.Add(world.Map[node.Z, node.X - 1]);
        else
        {
            list.Add(world.Map[node.Z, node.X + 1]);
            list.Add(world.Map[node.Z, node.X - 1]);
        }
    
        if (node.Z == 0)
            list.Add(world.Map[node.Z + 1, node.X]);
        else if (node.Z == world.Size - 1)
            list.Add(world.Map[node.Z - 1, node.X]);
        else
        {
            list.Add(world.Map[node.Z + 1, node.X]);
            list.Add(world.Map[node.Z - 1, node.X]);
        }
    
        return list;
    }
}
