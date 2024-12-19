using UnityEngine;
using System.Collections.Generic;

public class PositionLogger : MonoBehaviour
{
    public Transform character; // Assign your character's transform here
    public int maxDataPoints = 100; // Limit the number of data points
    private List<Vector2> positionData = new List<Vector2>();
    
    public List<Vector2> temppositionData = new List<Vector2>();

    void tUpdate()
    {
        // Record time and position
        if (positionData.Count >= maxDataPoints)
            positionData.RemoveAt(0); // Remove oldest data to keep the list size constant

        positionData.Add(new Vector2(Time.time, character.position.y));
    }

    public List<Vector2> GetPositionData()
    {
        return new List<Vector2>(temppositionData); // Return a copy to avoid unintended modifications
    }
}