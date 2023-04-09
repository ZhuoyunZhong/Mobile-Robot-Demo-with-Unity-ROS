### Part 3: ROS

This part includes how to add ROS communication scripts and set up the ROS node to communicate with robots in Unity.

To communicate between Unity and ROS, we need to define subscribers and publishers C# scripts, and launch an endpoint on the ROS side.

---

On the ROS side

- Copy all the folders under **ROS** folder of this repository, and paste them to your catkin workspace. Build your catkin workspace again with `catkin_make`. Please note that if you don't find any files under **ROS-TCP-Endpoint** or **teleop_twist_joy**, you didn't correctly clone the repository. Remember to clone it with `--recurse-submodules`.

- You don't necessarily need to change the IP adress in launch file, but you have to do that if you are using ports other than 10000.

- Now you could launch ROS node by 

  `roslaunch freight_endpoint server.launch`

On the Unity side

- click **Robotics** in the menu bar and click **ROS Settings**. Change the IP Address to your IP. You can get the IP of your machine by  `hostname -I`. This will generate a **ROSConnectionPrefab** in `Assets/Resources`, drag it to the Scene hierarchy.
- With the nodes launched, run Unity, hit connect in the hub, and you should be able to see they are connected successfully.

To send and receive messages from ROS, we will still need some subscribers and publishers scripts to send and receive messages. Some of the codes are originated from [ros-sharp](https://github.com/siemens/ros-sharp) and are modified to work with the new ROS-Unity framework. 

Let's create a new empty game object called **ROS** under `Freight` to store ROS-related scripts, or you could also add the scripts to `Freight/Plugins` as the previous ones. 

- Now in `Freight/ROS`, **Add Component**, and search **Subscriber** and add **Twist Subscriber**. This script subscribes the twist message from the ROS side and send the control signal to the wheel controller to move the robot. Now drag and drop the `Freight/Plugins` into Wheel Controller slot, and set the maximum linear and angular speed. As we are using control command from ROS side now, don't forget to disable the **Keyboard Control** in `Freight/Plugins`.

  Run the Unity simulation and launch the ROS node, send commands to topic `cmd_vel` with packages like `teleop_twist_joy`. The robot should be able to follow your command.

![image](demo/cmd_vel.gif)

- In terms of publishers, in `Freight/ROS`, **Add Component**, search **Publisher** and add publishers: **ROS Clock Publisher**, **ROS Transform Tree Publisher**, **Image Publisher**, **Laser Scan Publisher**, **Joint State Publisher**, **Pose Stamped Publisher** and **Twist Stamped Publisher**, based on your need. Don't forget to drag and drop proper game objects and parameters (e.g. publish rate) after adding scripts.

![image](demo/pub_sub.jpg)

- You can view the sent image by 

  `rosrun image_view image_view image:=/camera/color/image_raw`
  
  There are a few known issues with the camera publisher
  
  - This render mode does NOT support linear color space.
  - The renderer and encoder will slow down the frame rate dramatically. Disable it if you are not using it.
  
- You can view the laser scan with Rviz. Add a laser scan, select the `base_scan` topic and change the global fixed frame to `freight/base_link`.

![image](demo/laser_rviz.gif)

-  For the others, you could echo them to check if they are sent and received properly.

By now, you finish setting up a mobile robot demo, and you should have the same setup as the Scene **RobotROS**.

---

Proceed to [Part 4: ROS Navigation](part3_ros_nav.md) 
