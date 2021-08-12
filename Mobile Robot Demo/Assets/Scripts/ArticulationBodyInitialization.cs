using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     This script initializes the articulation bodies by 
///     setting stiffness, damping and force limit of
///     the non-fixed ones.
/// </summary>
public class ArticulationBodyInitialization : MonoBehaviour
{
    private ArticulationBody[] articulationChain;

    public GameObject robotRoot;
    public bool assignToAllChildren = true;
    public int robotChainLength = 0;
    public float stiffness = 10000f;
    public float damping = 100f;
    public float forceLimit = 1000f;

    void Start()
    {
        // Get non-fixed joints
        articulationChain = robotRoot.GetComponentsInChildren<ArticulationBody>();
        articulationChain = articulationChain.Where(joint => joint.jointType 
                                                    != ArticulationJointType.FixedJoint).ToArray();

        // Joint length to assign
        int assignLength = articulationChain.Length;
        if (!assignToAllChildren)
            assignLength = robotChainLength;

        // Setting stiffness, damping and force limit
        int defDyanmicVal = 100;
        for (int i = 0; i < assignLength; i++)
        {
            ArticulationBody joint = articulationChain[i];
            ArticulationDrive drive = joint.xDrive;

            joint.jointFriction = defDyanmicVal;
            joint.angularDamping = defDyanmicVal;

            drive.stiffness = stiffness;
            drive.damping = damping;
            drive.forceLimit = forceLimit;
            joint.xDrive = drive;
        }
    }
    
    void Update()
    {
    }
}