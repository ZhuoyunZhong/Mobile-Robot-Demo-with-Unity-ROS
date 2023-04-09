using System;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.BuiltinInterfaces;
using RosMessageTypes.Rosgraph;
using Unity.Robotics.Core;

/// <summary>
///     This script publishes simulation time
///     TODO - Change to use InvokeRepeat()
/// </summary>
public class ROSClockPublisher : MonoBehaviour
{
    [SerializeField]
    Clock.ClockMode m_ClockMode;

    [SerializeField, HideInInspector]
    Clock.ClockMode m_LastSetClockMode;
    
    [SerializeField] 
    double m_PublishRateHz = 100f;

    double m_LastPublishTimeSeconds;

    ROSConnection m_ROS;

    double PublishPeriodSeconds => 1.0f / m_PublishRateHz;

    bool ShouldPublishMessage => Clock.FrameStartTimeInSeconds - PublishPeriodSeconds > m_LastPublishTimeSeconds;

    void OnValidate()
    {
        var clocks = FindObjectsOfType<ROSClockPublisher>();
        if (clocks.Length > 1)
        {
            Debug.LogWarning("Found too many clock publishers in the scene, there should only be one!");
        }

        if (Application.isPlaying && m_LastSetClockMode != m_ClockMode)
        {
            Debug.LogWarning("Can't change ClockMode during simulation! Setting it back...");
            m_ClockMode = m_LastSetClockMode;
        }
        
        SetClockMode(m_ClockMode);
    }

    void SetClockMode(Clock.ClockMode mode)
    {
        Clock.Mode = mode;
        m_LastSetClockMode = mode;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetClockMode(m_ClockMode);
        m_ROS = ROSConnection.GetOrCreateInstance();
        m_ROS.RegisterPublisher<ClockMsg>("clock");
    }

    void PublishMessage()
    {
        var publishTime = Clock.time;
        TimeStamp timeStamp = new TimeStamp(Clock.time);
        var clockMsg = new TimeMsg
        {
            sec = timeStamp.Seconds,
            nanosec = timeStamp.NanoSeconds
        };
        m_LastPublishTimeSeconds = publishTime;
        m_ROS.Publish("clock", clockMsg);
    }

    void Update()
    {
        if (ShouldPublishMessage)
        {
            PublishMessage();
        }
    }
}
