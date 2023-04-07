using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.Robotics.ROSTCPConnector;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;
using RosMessageTypes.Geometry;

/// <summary>
///     This script subscribes to twist command
///     and use robot controller to 
/// </summary>
public class TwistSubscriber : MonoBehaviour
{
    // ROS Connector
    private ROSConnection ros;
    // Variables required for ROS communication
    public string twistTopicName = "base_controller/cmd_vel";

    public ArticulationWheelController wheelController;
    public float linearSpeed = 1.5f;
    public float angularSpeed = 1.5f;
    private float targetLinearVelocity;
    private float targetAngularVelocity;
    
    void Start()
    {
        // Get ROS connection static instance
        ros = ROSConnection.GetOrCreateInstance();

        targetLinearVelocity = 0f;
        targetAngularVelocity = 0f;
        
        ros.Subscribe<TwistMsg>(twistTopicName, UpdateVelocity);
    }

    private void UpdateVelocity(TwistMsg twist)
    {
        targetLinearVelocity = twist.linear.From<FLU>().z * linearSpeed;
        targetAngularVelocity = twist.angular.From<FLU>().y * angularSpeed;
        wheelController.SetRobotVelocity(targetLinearVelocity, targetAngularVelocity);
    }
}
