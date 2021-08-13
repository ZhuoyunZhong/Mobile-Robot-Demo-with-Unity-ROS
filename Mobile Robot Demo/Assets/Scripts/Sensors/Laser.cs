using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     This script simulates a laser scanner to
///     detect the surrounding obstacles
/// </summary>
public class Laser : MonoBehaviour
{
    public GameObject laserLink;
    public bool alwaysOn = true;
    public bool visualization = false;

    public int samples = 180;
    public float angleMin = -1.5708f;
    public float angleMax = 1.5708f;
    private float angleIncrement;
    public int updateRate = 10;
    private float scanTime;
    public float rangeMin = 0.1f;
    public float rangeMax = 5.0f;

    private RaycastHit[] raycastHits;
    public float[] ranges;

    void Start()
    {
        // Calculate resolution based on angle limit and number of samples
        angleIncrement = (angleMax - angleMin)/(samples-1);

        ranges = new float[samples];
        raycastHits = new RaycastHit[samples];

        scanTime = 1f/updateRate;
        if (alwaysOn)
            InvokeRepeating("Scan", 1f, scanTime);
    }

    void Update()
    {
    }

    private void Scan()
    {
        ranges = new float[samples];

        // Cast rays towards diffent directions to find colliders
        for (int i = 0; i < samples; i++)
        {
            Vector3 rotation = GetRayRotation(i) * laserLink.transform.forward;
            // Check if hit colliders within distance
            if (Physics.Raycast(laserLink.transform.position, rotation, 
                                out raycastHits[i], rangeMax) && 
               (raycastHits[i].distance >= rangeMin))
            {
                ranges[i] = raycastHits[i].distance;

                // Debug
                if (visualization)
                    Debug.DrawRay(laserLink.transform.position, 
                                  ranges[i] * rotation, 
                                  Color.red, scanTime);
            }
        }
    }

    private Quaternion GetRayRotation(int sampleInd) {
        float angle = (angleMin + (angleIncrement * sampleInd)) * Mathf.Rad2Deg;
        return Quaternion.AngleAxis(angle, laserLink.transform.up);
    }

    public float[] GetCurrentScanRanges() 
    {
        return ranges;
    }
}