using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.Robotics.ROSTCPConnector;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;
using RosMessageTypes.Std;
using RosMessageTypes.Geometry;

/// <summary>
///     This script publishes robot stamped pose
/// </summary>
public class PoseStampedPublisher : MonoBehaviour
{
    // ROS Connector
    private ROSConnection ros;
    // Variables required for ROS communication
    public string poseStampedTopicName = "model_pose";
    
    // Transform
    public Transform publishedTransform;

    // Message
    private PoseStampedMsg poseStamped;
    private string frameId = "model_pose";
    public float publishRate;

    void Start()
    {
        // Get ROS connection static instance
        ros = ROSConnection.instance;

        // Initialize message
        poseStamped = new PoseStampedMsg
        {
            header = new HeaderMsg()
            {
                frame_id = frameId
            }
        };

        InvokeRepeating("PublishPoseStamped", 1f, 1f/publishRate);
    }

    private void PublishPoseStamped()
    {
        poseStamped.header.Update();

        poseStamped.pose.position = publishedTransform.position.To<FLU>();
        poseStamped.pose.orientation = publishedTransform.rotation.To<FLU>();

        ros.Send(poseStampedTopicName, poseStamped);
    }
}
