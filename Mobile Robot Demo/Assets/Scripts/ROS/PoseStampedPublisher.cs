using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.Robotics.Core;
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
    private string frameID = "model_pose";
    public float publishRate = 10f;

    void Start()
    {
        // Get ROS connection static instance
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<PoseStampedMsg>(poseStampedTopicName);

        // Initialize message
        poseStamped = new PoseStampedMsg
        {
            header = new HeaderMsg(
                Clock.GetCount(), new TimeStamp(Clock.time), frameID
            )
        };

        InvokeRepeating("PublishPoseStamped", 1f, 1f/publishRate);
    }

    private void PublishPoseStamped()
    {
        poseStamped.header = new HeaderMsg(
            Clock.GetCount(), new TimeStamp(Clock.time), frameID
        );

        poseStamped.pose.position = publishedTransform.position.To<FLU>();
        poseStamped.pose.orientation = publishedTransform.rotation.To<FLU>();

        ros.Publish(poseStampedTopicName, poseStamped);
    }
}
