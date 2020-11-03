using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField]
    private int moves;
    [SerializeField]
    private new Camera camera;
    [SerializeField]
    private Vector3 cameraOffset;

    private Vector3 start;
    private List<Node> availableNodes;
    private Stack<Node> path;

    private LevelManager level;

    private void Awake()
    {
        level = GameObject.FindGameObjectWithTag("GameManager")
            .GetComponent<LevelManager>();
        GetComponent<MeshRenderer>().material.color = Color.grey;
    }

    private void Start()
    {
        var node = level.GetRandomWalkableNode();
        transform.position = node.GetVector();
    
        availableNodes = level.Pathfinding.GetWalkableNodes(node, moves);
        foreach (var n in availableNodes)
        {
            n.ChangeColour(Color.green);
        }

        camera.transform.position = transform.position + cameraOffset;
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
            if (!Physics.Raycast(ray, out hit))
                return;

            var node = level.GetNodeAtPosition(hit.collider.gameObject.transform.position);
            if (!availableNodes.Contains(node))
                return;
    
            foreach (var n in availableNodes)
            {
                n.ChangeColour();
            }
            path = level.Pathfinding.FindPath(transform.position, node.GetVector());
        }
        
        if (path == null || path.Count <= 0)
            return;
        
        Movement(path.Peek().GetVector());
        
        if (path.Peek().GetVector() == transform.position)
        {
            var current = path.Pop();
            if (path.Count <= 0)
            {
                availableNodes = level.Pathfinding.GetWalkableNodes(current, moves);
                foreach (var node in availableNodes)
                {
                    node.ChangeColour(Color.green);
                }
            }
        }
    }
    
    protected void Movement(Vector3 destination)
    {
        var movement = Vector3.MoveTowards(transform.position, destination, 2f * Time.deltaTime);
        var direction = destination - transform.position;
        var rotation = Quaternion.LookRotation(direction);
        transform.position = movement;
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 10f * Time.deltaTime);
        camera.transform.position = movement + cameraOffset;
    }
}
