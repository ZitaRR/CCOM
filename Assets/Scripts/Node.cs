using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node 
{
    public Node Parent { get; set; }
    public int X { get; set; }
    public int Z { get; set; }
    public int Distance { get; set; } = -1;
    public int Cost { get; set; } = 1;
    public int Weight { get; set; }
    public int F
    {
        get
        {
            if (Distance != -1 && Cost != -1)
                return Distance + Cost;
            return -1;
        }
    }
    public bool Walkable { get; set; }

    private GameObject node;

    public Node(GameObject obj, int x, int z, bool walkable, int weight = 1)
    {
        Parent = null;
        X = x;
        Z = z;
        Walkable = walkable;
        Weight = weight;
        node = Object.Instantiate(obj, GetVector(), Quaternion.identity);
        node.GetComponent<MeshRenderer>().material.color = Color.white;
        if (!Walkable)
        {
            ChangeColour(Color.red);
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = GetVector();
        }
    }

    public void ChangeColour(Color colour)
    {
        var material = node.GetComponent<MeshRenderer>().material;
        material.color = colour;
    }

    public Vector3 GetVector()
        => new Vector3(X, .5f, Z);

    public override string ToString()
        => $"X: {X}, Z: {Z}";
}
