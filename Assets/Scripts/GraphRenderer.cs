using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class GraphRenderer : MonoBehaviour
{
    public PositionLogger positionLogger; // Reference the PositionLogger script
    public float graphWidth = 10f;
    public float graphHeight = 5f;
    public Transform graphOrigin; // Set this to where you want the graph to start
    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        List<Vector2> data = positionLogger.GetPositionData();
        lineRenderer.positionCount = data.Count;

        for (int i = 0; i < data.Count; i++)
        {
            float normalizedTime = (data[i].x - data[0].x) / (data[data.Count - 1].x - data[0].x); // Normalize time
            float xPos = normalizedTime * graphWidth;
            float yPos = (data[i].y - graphOrigin.position.y) / graphHeight;
            lineRenderer.SetPosition(i, new Vector3(xPos, yPos, 0));
        }
    }
}