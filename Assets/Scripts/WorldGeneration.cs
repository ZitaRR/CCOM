using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WorldGeneration : MonoBehaviour
{   
    [SerializeField]
    private GameObject node;

    private new Renderer renderer;

    public int Size { get; private set; }
    public Node[,] Map { get; private set; }

    private void Awake()
    {
        renderer = GetComponent<Renderer>();
        Size = (int)renderer.bounds.size.x + 1;
        Map = new Node[Size, Size];
        transform.position = new Vector3(Size / 2, 0, Size / 2);

        for (int z = 0; z < Size; z++)
        {
            for (int x = 0; x < Size; x++)
            {
                Map[z, x] = new Node(node, x, z, true);
            }
        }
    }
}
