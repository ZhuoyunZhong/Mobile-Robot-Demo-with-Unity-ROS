using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.Robotics.Core;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;
using RosMessageTypes.Std;
using RosMessageTypes.Geometry;
using RosMessageTypes.BuiltinInterfaces;

/// <summary>
///     This script provides extension functions to convert
///     Unity coordinate to ROS coordinate
/// </summary>
public static class TransformExtensions
{
    public static TransformMsg ToROSTransform(this Transform tfUnity)
    {
        return new TransformMsg(
            // Using vector/quaternion To<>() because 
            // Transform.To<>() doesn't use localPosition/localRotation
            tfUnity.localPosition.To<FLU>(),
            tfUnity.localRotation.To<FLU>()
        );
    }

    public static TransformStampedMsg ToROSTransformStamped(
        this Transform tfUnity, double timeStamp)
    {
        return new TransformStampedMsg(
            new HeaderMsg(
                Clock.GetCount(), 
                new TimeStamp(timeStamp), 
                tfUnity.parent.gameObject.name
            ),
            tfUnity.gameObject.name,
            tfUnity.ToROSTransform());
    }
}
