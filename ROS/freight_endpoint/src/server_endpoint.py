#!/usr/bin/env python

import rospy
from ros_tcp_endpoint import TcpServer, RosPublisher, RosSubscriber, RosService

from std_msgs.msg import Float64
from geometry_msgs.msg import Pose, Twist, PoseStamped, TwistStamped
from sensor_msgs.msg import CompressedImage, LaserScan, JointState


def main():
    ros_node_name = rospy.get_param("/TCP_NODE_NAME", 'TCPServer')
    tcp_server = TcpServer(ros_node_name)
    rospy.init_node(ros_node_name, anonymous=True)

    # Start the Server Endpoint with a ROS communication objects dictionary for routing messages
    tcp_server.start({
        # Ground truth
        "model_pose": RosPublisher("model_pose", PoseStamped, queue_size=1),
        "model_twist": RosPublisher("model_twist", TwistStamped, queue_size=1),
        
        # Sensors
        # camera
        "camera/color/image_raw/compressed": 
            RosPublisher("camera/color/image_raw/compressed", CompressedImage, queue_size=1),
        # lidar
        "base_scan": 
            RosPublisher("base_scan", LaserScan, queue_size=1),
        # state
        "joint_states": 
            RosPublisher("joint_states", JointState, queue_size=1),
        
        # Controllers
        # base controller
        "base_controller/cmd_vel":
            RosSubscriber("base_controller/cmd_vel", Twist, tcp_server)
    })
    
    rospy.spin()


if __name__ == "__main__":
    main()
