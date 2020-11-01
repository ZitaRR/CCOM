﻿using System.Collections;
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
        node = Object.Instantiate(obj, new Vector3(X, .5f, Z), Quaternion.identity);
    }

    public void ChangeColour()
    {
        var material = node.GetComponent<MeshRenderer>().material;
        material.color = Color.cyan;
    }

    public override string ToString()
        => $"X: {X}, Z: {Z}";
}
