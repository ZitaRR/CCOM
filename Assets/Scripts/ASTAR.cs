using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ASTAR 
{
    private LevelManager level;
    private int size;

    public ASTAR(LevelManager level)
    {
        this.level = level;
    }

    public Stack<Node> FindPath(Vector3 start, Vector3 target)
    {
        int x = Mathf.RoundToInt(start.x);
        int z = Mathf.RoundToInt(start.z);
        Node nStart = level.Map[z, x];

        x = Mathf.RoundToInt(target.x);
        z = Mathf.RoundToInt(target.z);
        Node nTarget = level.Map[z, x];

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

    public Stack<Node> FindPath(int sx, int sz, int tx, int tz)
        => FindPath(new Vector3(sx, 0f, sz), new Vector3(tx, 0f, tz));

    public Stack<Node> FindPath(Node start, Node target)
        => FindPath(start.GetVector(), target.GetVector());

    public List<Node> GetAdjacentNodes(Node node)
    {
        var list = new List<Node>();
        if (node.X == level.Offset)
            list.Add(level.Map[node.Z, node.X + 1]);
        else if (node.X == size)
            list.Add(level.Map[node.Z, node.X - 1]);
        else 
        {
            list.Add(level.Map[node.Z, node.X + 1]);
            list.Add(level.Map[node.Z, node.X - 1]);
        }
    
        if (node.Z == level.Offset)
            list.Add(level.Map[node.Z + 1, node.X]);
        else if (node.Z == size)
            list.Add(level.Map[node.Z - 1, node.X]);
        else
        {
            list.Add(level.Map[node.Z + 1, node.X]);
            list.Add(level.Map[node.Z - 1, node.X]);
        }

        //absolute cancer
        //should use for loop but im a dumb dumb 
        //also, i might not end up using this bit at all

        //if (node.X == 0 && node.Z == 0)
        //    list.Add(level.Map[node.Z + 1, node.X + 1]);
        //else if (node.X == 0 && node.Z == size - 1)
        //    list.Add(level.Map[node.Z - 1, node.X + 1]);
        //else if (node.X == size - 1 && node.Z == 0)
        //    list.Add(level.Map[node.Z + 1, node.X - 1]);
        //else if (node.X == size - 1 && node.Z == size - 1)
        //    list.Add(level.Map[node.Z - 1, node.X - 1]);
        //else if (node.X == 0 && node.Z < size - 1)
        //{
        //    list.Add(level.Map[node.Z + 1, node.X + 1]);
        //    list.Add(level.Map[node.Z - 1, node.X + 1]);
        //}
        //else if (node.X == size - 1 && node.Z < size - 1)
        //{
        //    list.Add(level.Map[node.Z + 1, node.X - 1]);
        //    list.Add(level.Map[node.Z - 1, node.X - 1]);
        //}
        //else if (node.X < size - 1 && node.Z == 0)
        //{
        //    list.Add(level.Map[node.Z + 1, node.X + 1]);
        //    list.Add(level.Map[node.Z + 1, node.X - 1]);
        //}
        //else if (node.X < size - 1 && node.Z == size - 1)
        //{
        //    list.Add(level.Map[node.Z - 1, node.X + 1]);
        //    list.Add(level.Map[node.Z - 1, node.X - 1]);
        //}
        //else if ((node.X > 0 && node.X < size - 1) &&
        //         (node.Z > 0 && node.Z < size - 1))
        //{
        //    list.Add(level.Map[node.Z + 1, node.X + 1]);
        //    list.Add(level.Map[node.Z + 1, node.X - 1]);
        //    list.Add(level.Map[node.Z - 1, node.X + 1]);
        //    list.Add(level.Map[node.Z - 1, node.X - 1]);
        //}
    
        return list;
    }

    public List<Node> GetWalkableNodes(Node start, int limit)
    {
        var list = new List<Node>();
        int xLimit = 1;
        bool reverse = false;

        for (int z = start.Z + limit; z >= start.Z - limit; z--)
        {
            for (int x = start.X; x < start.X + xLimit; x++)
            {
                var path = FindPath(start.X, start.Z, x, z);
            
                if (path == null)
                    continue;

                int index = 0;
                foreach (var node in path)
                {
                    if (index++ > limit)
                        break;
                    if (!list.Contains(node))
                        list.Add(node);
                }
            }

            for (int x = start.X; x > start.X - xLimit; x--)
            {
                var path = FindPath(start.X, start.Z, x, z);

                if (path == null)
                    continue;

                int index = 0;
                foreach (var node in path)
                {
                    if (index++ > limit)
                        break;
                    if (!list.Contains(node))
                        list.Add(node);
                }
            }

            if (xLimit > limit)
                reverse = true;
            
            if (reverse)
                xLimit--;
            else xLimit++;
        }

        return list;
    }
}