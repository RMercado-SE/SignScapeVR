using System;
using System.Collections.Generic;
using UnityEngine;

public class HandTracking : MonoBehaviour
{
    // Reference to the script that provides hand tracking data from MediaPipe
    public ConnectToMediaPipe mediaPipeScript;

    // GameObject arrays representing the points of the right and left hands
    public GameObject[] handPointsRight;
    public GameObject[] handPointsLeft;

    // Constant for the number of points representing one hand in the tracking data
    private const int PointsPerHand = 21;

    void Update()
    {
        // Retrieve the hand tracking data string from MediaPipe
        string data = mediaPipeScript.data;

        // Check if the data string is not null, not empty, and has a minimal expected length
        if (!string.IsNullOrEmpty(data) && data.Length > 2)
        {
            // Process the data string by removing enclosing brackets and splitting it into an array
            data = data.Substring(1, data.Length - 2);
            List<Vector3> handPositions = ParseHandData(data);

            // Calculate the number of hands present in the data
            int numberOfHands = handPositions.Count / PointsPerHand;

            // Adjust for the case where we might have additional points beyond two hands
            numberOfHands = Mathf.Min(numberOfHands, 2); 

            // Update the right hand points with the data, ensuring not to exceed the array bounds
            if (handPointsRight.Length >= PointsPerHand && numberOfHands > 0)
            {
                UpdateHandPoints(handPointsRight, handPositions.GetRange(0, PointsPerHand));
            }

            // Update the left hand points with the data, ensuring not to exceed the array bounds
            if (handPointsLeft.Length >= PointsPerHand && numberOfHands == 2)
            {
                int startIndexOfSecondHand = PointsPerHand; // Starting index for the second hand
                int countOfSecondHand = handPositions.Count - startIndexOfSecondHand; // Number of points to take for the second hand
                countOfSecondHand = Mathf.Min(countOfSecondHand, PointsPerHand); // Ensure not to exceed PointsPerHand
                UpdateHandPoints(handPointsLeft, handPositions.GetRange(startIndexOfSecondHand, countOfSecondHand));
            }
        }
    }

    // Parses the raw string data from MediaPipe into a list of Vector3 positions
    List<Vector3> ParseHandData(string data)
    {
        List<Vector3> positions = new List<Vector3>();
        string[] points = data.Split(',');

        // Loop through the string array and convert each coordinate set into a Vector3
        for (int i = 0; i < points.Length; i += 3)
        {
            float x = -(5 - float.Parse(points[i])) / 120;
            float y = float.Parse(points[i + 1]) / 120;
            float z = float.Parse(points[i + 2]) / 120;
            positions.Add(new Vector3(x, y, z));
        }

        return positions;
    }

    // Updates the positions of the hand points based on the provided list of Vector3 positions
    void UpdateHandPoints(GameObject[] handPoints, List<Vector3> positions)
    {
        // Iterate through each position and update the corresponding hand point's position
        for (int i = 0; i < positions.Count; i++)
        {
            handPoints[i].transform.localPosition = positions[i];
        }
    }
}
