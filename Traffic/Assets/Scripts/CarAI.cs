using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Transform path;
    //[SerializeField] private Transform[] target;
    [SerializeField] private float speed;

    private List<Transform> nodes;
    private int currentNode;
    private float previousSpeed;
    private float stopTimer = Mathf.Infinity;
    private float stopDuration = 2;
    private GameObject otherCar;

    private bool onStop = false;
    private bool onCW = false;
    private bool carInFront = false;
    

    void Start()
    {
        Transform[] pathTransforms = path.GetComponentsInChildren<Transform>();
        nodes = new List<Transform>();

        foreach (Transform pathT in pathTransforms)
        {
            if (pathT != path.transform)
            {
                nodes.Add(pathT);
            }
        }
    }

    void FixedUpdate()
    {
        //Car position
        //if (transform.position != nodes[currentNode].position)
        if (Vector3.Distance(transform.position, nodes[currentNode].position) > 0.3f)
        {
            Vector3 pos = Vector3.MoveTowards(transform.position, nodes[currentNode].position, speed * Time.deltaTime);
            GetComponent<Rigidbody>().MovePosition(pos);
        }
        else
        {
            currentNode = (currentNode + 1) % nodes.Count;
        }

        if (carInFront)
        {
            if (Vector3.Distance(transform.position, otherCar.GetComponent<Rigidbody>().position) > 0.3f)
            {
                carInFront = false;
                speed = previousSpeed;
            }
        }

        //Waiting at the stop
        if (onStop) 
        {
            if (stopTimer < stopDuration)
            {
                //Debug.Log("STOP!: " + stopTimer);
                stopTimer += Time.deltaTime;
            }
            else
            {
                //Debug.Log("PUEDO SEGUIR!: " + stopTimer);
                onStop = false;
                speed = previousSpeed;
                stopTimer = Mathf.Infinity;
            }
        }
    }

    void OnCollisionEnter(Collision col)
    {
        GameObject currentCW = col.gameObject;
        Debug.Log(gameObject.name + " hits " + currentCW.name + "(" + col.collider.name + ")");
        switch (col.gameObject.tag)
        {
            case "Crosswalk":
                bool someoneInCW = currentCW.GetComponent<CWScript>().IsPeopleCrossing();
                if (someoneInCW)
                {
                    previousSpeed = speed;
                    speed = 0;
                    onCW = true;
                    currentCW.GetComponent<CWScript>().AddCarToQueue(gameObject.name);
                }
                break;
            case "Stop":
                Debug.Log("He llegado a <" + currentCW.name + ">");
                previousSpeed = speed;
                speed = 0;
                stopTimer = 0;
                onStop = true;
                break;
            case "Car":
                //Avoid case of car stopped colliding with Bounding Box of the car behind
                if (!onCW && !onStop)
                {
                    carInFront = true;
                    otherCar = col.gameObject;
                    previousSpeed = speed;
                    speed = 0;
                }
                break;
        }
    }

    public void ResetSpeed()
    {
        speed = previousSpeed;
        onCW = false;
    }
}
