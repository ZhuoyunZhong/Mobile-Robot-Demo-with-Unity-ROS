using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     This script simulates a laser scanner to
///     detect the surrounding obstacles
/// </summary>
public class Laser : MonoBehaviour
{
    // general
    public GameObject laserLink;
    private Vector3 rayStartPosition;
    private Vector3 rayStartForward;
    public bool debugVisualization = false;
    // scan params
    public int updateRate = 10;
    private float scanTime;
    public int samples = 180;
    public float angleMin = -1.5708f;
    public float angleMax = 1.5708f;
    private float angleIncrement;
    public float rangeMin = 0.1f;
    public float rangeMax = 5.0f;
    // containers
    private RaycastHit[] raycastHits;
    private Quaternion[] rayRotations;
    public float[] directions;
    public float[] ranges;

    private void Start()
    {
        // Containers
        rayRotations = new Quaternion[samples];
        directions = new float[samples];
        ranges = new float[samples];

        // Calculate resolution based on angle limit and number of samples
        angleIncrement = (angleMax - angleMin) / (samples - 1);
        for (int i = 0; i < samples; ++i)
        {
            directions[i] = angleMin + i * angleIncrement;
            rayRotations[i] = Quaternion.Euler(
                new Vector3(0f, directions[i] * Mathf.Rad2Deg, 0f)
            );
        }

        // Start scanning
        scanTime = 1f / updateRate;
        InvokeRepeating("Scan", 1f, scanTime);
    }

    private void Scan()
    {
        ranges = new float[samples];

        // Cast rays towards diffent directions to find colliders
        rayStartPosition = laserLink.transform.position;
        rayStartForward = laserLink.transform.forward;
        for (int i = 0; i < samples; ++i)
        {
            // Ray angle
            Vector3 rotation = rayRotations[i] * rayStartForward;

            // Check if hit colliders within distance
            raycastHits = new RaycastHit[samples];
            if (Physics.Raycast(rayStartPosition, rotation, out raycastHits[i], rangeMax) 
                && (raycastHits[i].distance >= rangeMin)
                && (!raycastHits[i].collider.isTrigger))
            {
                ranges[i] = raycastHits[i].distance;

                // Visualization
                if (debugVisualization)
                {
                    Debug.DrawRay(
                        rayStartPosition, ranges[i] * rotation, Color.red, scanTime
                    );
                }
            }
        }
    }

    public float[] GetCurrentScanRanges() 
    {
        return ranges;
    }
}