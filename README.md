# Mobile-Robot-Demo-with-Unity-ROS

![image](demo/freight_in_unity.jpg)

## Dependencies

This repository has been developed and tested in **Ubuntu 18.04 and ROS Melodic**, with **Unity 20.3.12f1 LTS**.

Before running this package, it is highly recommended to try the [Unity Robotics Hub](https://github.com/Unity-Technologies/Unity-Robotics-Hub) pick&place demo first. It shows the steps to import robot, convert messages, and communicate with ROS in details. But you could still run this package by following the steps below.

---

Open Unity Hub and click "Add" button. Select 

Create a new Unity 3D project and name it **Gopher In Unity Simulation**. 

Change the physics and color settings. Open `Edit` -> `Project Settings` 

- In `Physics`, change `Solver Type` from `Projected Gauss Seidel` to `Temporal Gauss Seidel`. 

The next step is to install ROS-Unity connection package. Open `Window` -> `Package Manager`, find and click the `+` in the left upper corner and switch to `Add package from git URL...`. 

- To install [ROS-TCP-Connector](https://github.com/Unity-Technologies/ROS-TCP-Connector), enter `https://github.com/Unity-Technologies/ROS-TCP-Connector.git?path=/com.unity.robotics.ros-tcp-connector` and add it.
- To install [URDF-Importer](https://github.com/Unity-Technologies/URDF-Importer), enter `https://github.com/Unity-Technologies/URDF-Importer.git?path=/com.unity.robotics.urdf-importer` and add it.

If you would like to edit or create a new navigation or building environment, you could also install `ProbBuilder` and `ProGrids` package. 

## Unity Setup

This

## ROS Setup

This

## Running Mobile Robot



## TODO List
