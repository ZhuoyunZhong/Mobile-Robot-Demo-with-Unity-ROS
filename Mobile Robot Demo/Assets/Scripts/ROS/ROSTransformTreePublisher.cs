using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

using Unity.Robotics.Core;
using Unity.Robotics.ROSTCPConnector;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;
using RosMessageTypes.Std;
using RosMessageTypes.Geometry;
using RosMessageTypes.Tf2;

/// <summary>
///     This script publishes tf trees
///     TODO - Change to use InvokeRepeat()
/// </summary>
public class ROSTransformTreePublisher : MonoBehaviour
{
    // ROS Connector
    private ROSConnection ros;
    // Variables required for ROS communication
    public List<string> globalFrameIds = new List<string> { "map", "odom" };
    private const string tfTopic = "/tf";

    // Robots
    public GameObject robot;
    private TransformTreeNode transformRoot;

    // Message
    public float publishRate = 20f;

    double lastPublishTimeSeconds;
    double publishPeriodSeconds => 1.0f / publishRate;
    bool shouldPublishMessage => Clock.NowTimeInSeconds > lastPublishTimeSeconds + publishPeriodSeconds;

    void Start()
    {
        if (robot == null)
        {
            Debug.LogWarning(
                $"No GameObject explicitly defined as {nameof(robot)}, so using {name} as root."
            );
            robot = gameObject;
        }

        // Get ROS connection static instance
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<TFMessageMsg>(tfTopic);

        // Get robot transform tree
        transformRoot = new TransformTreeNode(robot);

        lastPublishTimeSeconds = Clock.time + publishPeriodSeconds;
    }

    void Update()
    {
        if (shouldPublishMessage)
        {
            PublishTF();
        }

    }

    private static void PopulateTFList(List<TransformStampedMsg> tfList, TransformTreeNode tfNode)
    {
        // TODO: Some of this could be done once and cached rather than doing from scratch every time
        // Only generate transform messages from the children, because This node will be parented to the global frame
        foreach (var childTf in tfNode.Children)
        {
            tfList.Add(TransformTreeNode.ToTransformStamped(childTf));

            if (!childTf.IsALeafNode)
            {
                PopulateTFList(tfList, childTf);
            }
        }
    }

    private void PublishTF()
    {
        var tfMessageList = new List<TransformStampedMsg>();

        if (globalFrameIds.Count > 0)
        {
            var tfRootToGlobal = new TransformStampedMsg(
                new HeaderMsg(Clock.GetCount(), new TimeStamp(Clock.time), globalFrameIds.Last()),
                transformRoot.name,
                transformRoot.Transform.To<FLU>()
            );
            tfMessageList.Add(tfRootToGlobal);
        }
        else
        {
            Debug.LogWarning($"No {globalFrameIds} specified, transform tree will be entirely local coordinates.");
        }
        
        // In case there are multiple "global" transforms that are effectively the same coordinate frame, 
        // treat this as an ordered list, first entry is the "true" global
        for (var i = 1; i < globalFrameIds.Count; ++i)
        {
            var tfGlobalToGlobal = new TransformStampedMsg(
                new HeaderMsg(Clock.GetCount(), new TimeStamp(Clock.time), globalFrameIds[i - 1]),
                globalFrameIds[i],
                // Initializes to identity transform
                new TransformMsg());
            tfMessageList.Add(tfGlobalToGlobal);
        }

        PopulateTFList(tfMessageList, transformRoot);

        var tfMessage = new TFMessageMsg(tfMessageList.ToArray());
        ros.Publish(tfTopic, tfMessage);
        lastPublishTimeSeconds = Clock.FrameStartTimeInSeconds;
    }
}
