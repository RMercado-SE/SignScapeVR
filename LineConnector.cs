using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineConnector : MonoBehaviour
{
    // Reference to the LineRenderer component used to draw the line
    private LineRenderer lineRenderer;

    // Transforms representing the start and end points of the line
    public Transform origin;
    public Transform destination;

    // Start is called before the first frame update
    void Start()
    {
        // Get the LineRenderer component attached to this GameObject
        // The LineRenderer is used to render the line between two points
        lineRenderer = GetComponent<LineRenderer>();

        // Set the width of the line at the start and end points
        // These can be adjusted to make the line thicker or thinner
        lineRenderer.startWidth = 0.2f;
        lineRenderer.endWidth = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if both origin and destination transforms are assigned
        if(origin != null && destination != null)
        {
            // Set the start position of the line to the position of the 'origin' transform
            // This makes the line start at the position of the 'origin' GameObject
            lineRenderer.SetPosition(0, origin.position);

            // Set the end position of the line to the position of the 'destination' transform
            // This makes the line end at the position of the 'destination' GameObject
            lineRenderer.SetPosition(1, destination.position);

            // Debugging: Log world and local positions of the origin and destination
            // This is useful for troubleshooting and ensuring the positions are updating correctly
            Debug.Log("World - Origin: " + origin.position + ", Destination: " + destination.position);
            Debug.Log("Local - Origin: " + origin.localPosition + ", Destination: " + destination.localPosition);
        }
    }
}
