# Mobile-Robot-Demo-with-Unity-ROS

![image](demo/freight_in_unity.jpg)

## Dependencies

This repository has been developed and tested in **Ubuntu 18.04 and ROS Melodic**, with **Unity 20.3.12f1 LTS**.

Before running this package, it is highly recommended to try the [Unity Robotics Hub](https://github.com/Unity-Technologies/Unity-Robotics-Hub) pick&place demo first. It shows the steps to import robot, convert messages, and communicate with ROS in details. But you could still run this package by following the steps below.

---

Clone this project by `git clone --recurse-submodules git@github.com:ZhuoyunZhong/Mobile-Robot-Demo-with-Unity-ROS.git  `

### [Part 1: Unity Setup](part1_unity.md) 

![image](demo/navigating.gif)

This part includes how to import the mobile robot in Unity scene, and control the mobile robot with your keyboard.

### [Part 2: Sensors](part2_sensors.md) 

![image](demo/laser.gif)

This part includes how to attach common sensors: camera, laser and robot state readers to the robot.

### [Part 3: ROS Setup](part3_ros.md) 

![image](demo/ros.jpg)

This part includes how to set up the ROS node to communicate with robots in Unity.

## TODO List

- Robot State Publisher (Publish tf frames)
- RGB-D Camera / Pointcloud 
- More publishers and subscribers to use Unity as control or vision interface

