using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.Robotics.ROSTCPConnector;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;
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

    // Sensor
    public Laser laser;

    // Message
    private LaserScanMsg laserScan;
    private string FrameId = "laser_scan";
    public float publishRate;

    void Start()
    {
        // Get ROS connection static instance
        ros = ROSConnection.instance;

        // Initialize messages
        float angle_increment = (laser.angleMax - laser.rangeMin)/(laser.samples-1);
        float scan_time = 1f / laser.updateRate;
        float time_increment = 0f;
        float[] intensities = new float[laser.ranges.Length];
        laserScan = new LaserScanMsg
        {
            header = new HeaderMsg { frame_id = FrameId },
            angle_min       = laser.angleMin,
            angle_max       = laser.angleMax,
            angle_increment = angle_increment,
            time_increment  = time_increment,
            scan_time       = scan_time,
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
        laserScan.header.Update();
        laserScan.ranges = laser.GetCurrentScanRanges();

        ros.Send(laserTopicName, laserScan);
    }
}
