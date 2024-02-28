using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode] 

public class GraphComponent : MonoBehaviour
{
    public GameObject[] nodesToAdd;
    public GameObject[] edgesToAdd;

    private Graph<Vector3, float> graph;
    // Start is called before the first frame update
    void Start()
    {
        graph = new Graph<Vector3, float>();
        var node1 = new Node<Vector3>() { Value = Vector3.zero, NodeColor = Color.red};
        var node2 = new Node<Vector3>() { Value = Vector3.one, NodeColor = Color.green };
        var edge1 = new Edge<float, Vector3>() { 
            Value = 1.0f, 
            From = node1, 
            To = node2,
            EdgeColor = Color.yellow
        };
        graph.Nodes.Add(node1);
        graph.Nodes.Add(node2);
        graph.Edges.Add(edge1);

    }


    private void OnDrawGizmos()
    {
        if (graph == null)
        {
            Start();
        }

        foreach(var node in graph.Nodes)
        {
            Gizmos.color = node.NodeColor;
            Gizmos.DrawSphere(node.Value, 0.125f);
        }

        foreach (var edge in graph.Edges)
        {
            Gizmos.color = edge.EdgeColor;
            Gizmos.DrawLine(edge.From.Value, edge.To.Value);
        }

    }

    public void configureNodes()
    {
        foreach(GameObject node in nodesToAdd)
        {
            var newNode = new Node<Vector3>() { Value = node.transform.position, NodeColor = Color.cyan };
        }
    }
}
