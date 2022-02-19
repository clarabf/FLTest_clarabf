using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    [SerializeField] private Transform path;
    [SerializeField] private float maxSteerAngle = 45f;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider frontLeftWheelCollider;
    
    private List<Transform> nodes;
    private int currentNode = 0;

    // Start is called before the first frame update
    void Start()
    {
        Transform[] pathTransforms = path.GetComponentsInChildren<Transform>();
        nodes = new List<Transform>();

        foreach (Transform pathT in pathTransforms)
        {
            if (pathT != path.transform)
            {
                nodes.Add(pathT);
                Debug.Log(pathT.position);
            }
        }
    }

    private void FixedUpdate()
    {
        ApplySteer();
        Drive();
        CheckWaypointDistance();
    }

    private void ApplySteer()
    {
        Vector3 relativeVector = transform.InverseTransformPoint(nodes[currentNode].position);
        float newSteer = (relativeVector.x / relativeVector.magnitude) * maxSteerAngle;
        frontLeftWheelCollider.steerAngle = newSteer;
        frontRightWheelCollider.steerAngle = newSteer;
    }

    private void Drive()
    {
        frontLeftWheelCollider.motorTorque = 50f;
        frontRightWheelCollider.motorTorque = 50f;
    }

    private void CheckWaypointDistance()
    {
        if (Vector3.Distance(transform.position, nodes[currentNode].position) < 0.5f)
        {
            if (currentNode == nodes.Count - 1) currentNode = 0;
            else currentNode++;
        }
    }


}
