using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private int size;
    [SerializeField]
    private int offset;
    [SerializeField]
    private GameObject node;
    [SerializeField]
    private GameObject obstacle;

    private Node[,] map;
    private ASTAR astar;

    public int Size => size;
    public int Offset => offset;
    public Node[,] Map
    {
        get => map;
        set => map = value;
    }
    public ASTAR Pathfinding => astar;

    private void Awake()
    {
        astar = new ASTAR(this);
        GenerateLevel();
    }

    private void GenerateLevel()
    {
        map = new Node[size, size];

        for (int z = 0; z < size; z++)
        {
            for (int x = 0; x < size; x++)
            {
                if (z < offset || x < offset ||
                    z > size - offset || x > size - offset)
                {
                    map[z, x] = new Node(null, x, z, false);
                    continue;
                }

                if (Random.value > .8f)
                {
                    map[z, x] = new Node(obstacle, x, z, false);
                    continue;
                }

                map[z, x] = new Node(node, x, z, true);
            }
        }
    }

    public Node GetRandomWalkableNode()
    {
        Node node;
        do
        {
            int x = Random.Range(offset, size - offset);
            int z = Random.Range(offset, size - offset);
            node = map[z, x];
        } while (!node.Walkable);
        return node;
    }

    public Node GetNodeAtPosition(Vector3 position)
    {
        int x = (int)position.x;
        int z = (int)position.z;
        return Map[z, x];
    }

    public Node GetNodeAtPosition(int x, int z)
        => GetNodeAtPosition(new Vector3(x, 0f, z));
}
