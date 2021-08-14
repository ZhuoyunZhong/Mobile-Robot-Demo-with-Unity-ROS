using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     This script converts linear velocity and 
///     angular velocity to joint velocities for
///     differential drive robot.
/// </summary>
public class ArticulationWheelController : MonoBehaviour
{
    public ArticulationBody leftWheel;
    public ArticulationBody rightWheel;
    public float wheelTrackLength;
    public float wheelRadius;

    private float vRight;
    private float vLeft;

    void Start()
    {
    }

    void Update()
    {
    }

    public void setRobotVelocity(float targetLinearSpeed, float targetAngularSpeed)
    {
        // Stop the wheel if target velocity is 0
        if (targetLinearSpeed == 0 && targetAngularSpeed == 0)
        {
            stopWheel(leftWheel);
            stopWheel(rightWheel);
        }
        else
        {
            // Convert from linear x and angular z velocity to wheel speed
            vRight = targetAngularSpeed*(wheelTrackLength/2) + targetLinearSpeed;
            vLeft = -targetAngularSpeed*(wheelTrackLength/2) + targetLinearSpeed;

            setWheelVelocity(leftWheel, vLeft / wheelRadius * Mathf.Rad2Deg);
            setWheelVelocity(rightWheel, vRight / wheelRadius * Mathf.Rad2Deg);
        }
    }

    private void setWheelVelocity(ArticulationBody wheel, float jointVelocity)
    {
        ArticulationDrive drive = wheel.xDrive;
        drive.target = drive.target + jointVelocity * Time.fixedDeltaTime;
        wheel.xDrive = drive;
    }

    private void stopWheel(ArticulationBody wheel)
    {
        // Set desired angle as current angle to stop the wheel
        ArticulationDrive drive = wheel.xDrive;
        drive.target = wheel.jointPosition[0] * Mathf.Rad2Deg;
        wheel.xDrive = drive;
    }
}
