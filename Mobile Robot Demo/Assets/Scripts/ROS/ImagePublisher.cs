using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.Robotics.Core;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Sensor;
using RosMessageTypes.Std;

/// <summary>
///     This script publishes camera view
///     as compressed image 
/// </summary>
public class ImagePublisher : MonoBehaviour
{
    // ROS Connector
    private ROSConnection ros;
    // Variables required for ROS communication
    public string cameraTopicName = "camera/color/image_raw/compressed";

    // Sensor
    public Camera imageCamera;
    public int resolutionWidth = 1280;
    public int resolutionHeight = 720;
    public int qualityLevel = 50;
    
    // Message
    private CompressedImageMsg compressedImage;
    private string frameId = "camera";

    private Texture2D texture2D;
    private Rect rect;

    void Start()
    {
        // Get ROS connection static instance
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<CompressedImageMsg>(cameraTopicName);

        // Initialize renderer
        texture2D = new Texture2D(resolutionWidth, resolutionHeight, TextureFormat.ARGB32, false);
        rect = new Rect(0, 0, resolutionWidth, resolutionHeight);
        RenderTexture renderTexture = new RenderTexture(
            resolutionWidth, resolutionHeight, 24, 
            RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB
        );
        imageCamera.targetTexture = renderTexture;
        
        // Initialize messages
        compressedImage = new CompressedImageMsg();
        compressedImage.header = new HeaderMsg(
            Clock.GetCount(), new TimeStamp(Clock.time), frameId
        );
        compressedImage.format = "jpeg";

        // Call back
        Camera.onPostRender += UpdateImage;
    }

    private void UpdateImage(Camera cameraObject)
    {
        if (texture2D != null && cameraObject == imageCamera)
        {
            compressedImage.header = new HeaderMsg(
                Clock.GetCount(), new TimeStamp(Clock.time), frameId
            );
            texture2D.ReadPixels(rect, 0, 0);
            compressedImage.data = texture2D.EncodeToJPG(qualityLevel);

            ros.Publish(cameraTopicName, compressedImage);
        }
    }
}
