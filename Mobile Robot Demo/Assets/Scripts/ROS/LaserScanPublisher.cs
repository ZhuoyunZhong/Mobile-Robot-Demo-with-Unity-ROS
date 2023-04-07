using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.Robotics.Core;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Sensor;
using RosMessageTypes.Std;

/// <summary>
///     This script publishes laser scan
/// </summary>
public class LaserScanPublisher : MonoBehaviour
{
    // ROS Connector
    private ROSConnection ros;
    // Variables required for ROS communication
    public string laserTopicName = "base_scan";
    public string laserLinkId = "laser_link";

    // Sensor
    public Laser laser;

    // Message
    private LaserScanMsg laserScan;
    public float publishRate = 10f;

    void Start()
    {
        // Get ROS connection static instance
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<LaserScanMsg>(laserTopicName);

        // Initialize messages
        float angleIncrement = (laser.angleMax - laser.angleMin)/(laser.samples-1);
        float scanTime = 1f / laser.updateRate;
        float timeIncrement = scanTime / laser.samples;
        float[] intensities = new float[laser.ranges.Length];
        laserScan = new LaserScanMsg
        {
            header = new HeaderMsg(Clock.GetCount(), 
                                   new TimeStamp(Clock.time), laserLinkId),
            angle_min       = laser.angleMin,
            angle_max       = laser.angleMax,
            angle_increment = angleIncrement,
            time_increment  = timeIncrement,
            scan_time       = scanTime,
            range_min       = laser.rangeMin,
            range_max       = laser.rangeMax,
            ranges          = laser.ranges,      
            intensities     = intensities
        };

        InvokeRepeating("PublishScan", 1f, 1f/publishRate);
    }

    void Update()
    {
    }

    private void PublishScan()
    {   
        laserScan.header = new HeaderMsg(
            Clock.GetCount(), new TimeStamp(Clock.time), laserLinkId
        );
        laserScan.ranges = laser.ranges;

        ros.Publish(laserTopicName, laserScan);
    }
}
