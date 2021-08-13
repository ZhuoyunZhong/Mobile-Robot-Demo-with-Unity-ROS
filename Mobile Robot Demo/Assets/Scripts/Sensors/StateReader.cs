using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     This script reads robot positions, velocities,
///     joint states, etc.
/// </summary>
public class StateReader : MonoBehaviour
{
    public GameObject robot;
    public int updateRate = 10;
    private float deltaTime;

    private Transform tf;
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 eulerRotation;
    public Vector3 linearVelocity;
    public Vector3 angularVelocity;

    private ArticulationBody[] articulationChain;
    private int jointStateLength;
    public string[] names;
    public float[] positions;
    public float[] velocities;
    public float[] forces;

    void Start()
    {
        deltaTime = 1f/updateRate;
        InvokeRepeating("ReadState", 1f, deltaTime);

        // Get robot transform
        tf = robot.transform;

        // Get joints
        articulationChain = robot.GetComponentsInChildren<ArticulationBody>();
        articulationChain = articulationChain.Where(joint => joint.jointType 
                                                    != ArticulationJointType.FixedJoint).ToArray();
        jointStateLength = articulationChain.Length;
        names = new string[jointStateLength];
        positions = new float[jointStateLength];
        velocities = new float[jointStateLength];
        forces = new float[jointStateLength];
       
        for (int i = 0; i < jointStateLength; i++)
            names[i] = articulationChain[i].name;
    }

    void Update()
    {
    }

    void ReadState()
    {
        // Lienar and angular velocity
        linearVelocity = (tf.position - position) / deltaTime;
        angularVelocity = (tf.rotation.eulerAngles - eulerRotation) / deltaTime;
        // Transfer to local frame
        linearVelocity = tf.InverseTransformDirection(linearVelocity);
        angularVelocity = tf.InverseTransformDirection(angularVelocity);

        // Position and orientation
        position = tf.position;
        rotation = tf.rotation;
        eulerRotation = rotation.eulerAngles;

        // Joint states
        for (int i = 0; i < jointStateLength; i++)
        {   
            positions[i] = articulationChain[i].jointPosition[0];
            velocities[i] = articulationChain[i].jointVelocity[0];
            forces[i] = articulationChain[i].jointForce[0];
        }
        
    }
}
