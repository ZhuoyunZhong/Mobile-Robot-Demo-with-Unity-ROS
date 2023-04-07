using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.Robotics.Core;
using Unity.Robotics.ROSTCPConnector;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;
using RosMessageTypes.Std;
using RosMessageTypes.Geometry;

/// <summary>
///     This script publishes robot stamped twist
///     with repect to the local robot frame
/// </summary>
public class TwistStampedPublisher : MonoBehaviour
{
    // ROS Connector
    private ROSConnection ros;
    // Variables required for ROS communication
    public string twistStampedTopicName = "model_twist";

    // Transform
    public Transform publishedTransform;
    private Vector3 previousPosition;
    private Vector3 previousRotation;
    private Vector3 linearVelocity;
    private Vector3 angularVelocity;

    // Message
    private TwistStampedMsg twistStamped;
    private string frameId = "model_twist";
    public float publishRate = 10f;
    private float deltaTime;


    void Start()
    {
        // Get ROS connection static instance
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<TwistStampedMsg>(twistStampedTopicName);

        previousPosition = publishedTransform.position;
        previousRotation = publishedTransform.rotation.eulerAngles;

        // Initialize message
        twistStamped = new TwistStampedMsg
        {
            header = new HeaderMsg(
                Clock.GetCount(), new TimeStamp(Clock.time), frameId
            )
        };

        deltaTime = 1f/publishRate;
        InvokeRepeating("PublishTwistStamped", 1f, deltaTime);
    }

    private void PublishTwistStamped()
    {
        twistStamped.header = new HeaderMsg(
            Clock.GetCount(), new TimeStamp(Clock.time), frameId
        );

        // Linear
        linearVelocity = (publishedTransform.position - previousPosition)
                         /deltaTime;
        linearVelocity = publishedTransform.InverseTransformDirection(linearVelocity);
        previousPosition = publishedTransform.position;

        // Angular
        angularVelocity = (publishedTransform.rotation.eulerAngles - previousRotation)
                          /deltaTime * Mathf.Deg2Rad;
        angularVelocity = publishedTransform.InverseTransformDirection(angularVelocity);
        // Using Vector3 euler angles instead of Quatenion to compute angular velocity
        // Need to adjust the result
        angularVelocity = -angularVelocity;
        previousRotation = publishedTransform.rotation.eulerAngles;

        twistStamped.twist.linear = linearVelocity.To<FLU>();
        twistStamped.twist.angular = angularVelocity.To<FLU>();
        
        ros.Publish(twistStampedTopicName, twistStamped);
    }
}
