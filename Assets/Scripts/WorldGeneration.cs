using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WorldGeneration : MonoBehaviour
{   
    [SerializeField]
    private GameObject node;

    private new Renderer renderer;

    public static int Size { get; private set; }

    private static Node[,] map;
    public Node[,] Map
    {
        get => map;
        set => map = value;
    }

    private void Awake()
    {
        renderer = GetComponent<Renderer>();

        Size = (int)renderer.bounds.size.x + 1;
        Map = new Node[Size, Size];
        transform.position = new Vector3(Size / 2, 0, Size / 2);
        print("Size: " + Size);

        for (int z = 0; z < Size; z++)
        {
            for (int x = 0; x < Size; x++)
            {
                bool walkable = true;
                if (Random.value > .7f)
                    walkable = false;
                Map[z, x] = new Node(node, x, z, walkable);
            }
        }
    }

    public static Node GetNode(Vector3 pos)
    {
        int x = (int)pos.x;
        int z = (int)pos.z;
        return map[z, x];
    }
}
