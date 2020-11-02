using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Entity : MonoBehaviour
{
    private Vector3 start;
    private Vector3 end;
    private Stack<Node> path;

    private void Start()
    {
        GetComponent<MeshRenderer>().material.color = Color.white;
        do
        {
            int x = Random.Range(0, WorldGeneration.Size);
            int z = Random.Range(0, WorldGeneration.Size);
            start = new Vector3(x, .5f, z);
        } while (!WorldGeneration.GetNode(start).Walkable);

        transform.position = start;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out hit))
                return;

            path = ASTAR.FindPath(transform.position, hit.point);
        }

        if (path == null || path.Count <= 0)
            return;

        Movement(path.Peek().GetVector());

        if (path.Peek().GetVector() == transform.position)
            path.Pop();
    }

    protected void Movement(Vector3 destination)
    {
        var movement = Vector3.MoveTowards(transform.position, destination, 2f * Time.deltaTime);
        var direction = destination - transform.position;
        var rotation = Quaternion.LookRotation(direction);
        transform.position = movement;
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 10f * Time.deltaTime);
    }
    private void NewDestination()
    {
        do
        {
            int x = Random.Range(0, WorldGeneration.Size);
            int z = Random.Range(0, WorldGeneration.Size);
            end = new Vector3(x, .5f, z);
        } while (!WorldGeneration.GetNode(end).Walkable);
    }
}
