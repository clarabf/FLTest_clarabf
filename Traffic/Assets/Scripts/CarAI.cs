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
                //Debug.Log(gameObject.name + " LET'S GO!");
                
                //The car can move again because the car in front of it has stopped waiting                
                carInFront = false;
                speed = previousSpeed;
                
                bool carOnCW = otherCar.GetComponent<CarAI>().isOnCW();
                bool carOnStop = otherCar.GetComponent<CarAI>().isOnStop();

                if (!carOnCW) onCW = false;
                if (!carOnStop) onStop = false;
            }
        }

        //Waiting at the stop
        if (onStop) 
        {
            if (stopTimer < stopDuration)
            {
                //Debug.Log(gameObject.name + " STOP!: " + stopTimer);
                stopTimer += Time.deltaTime;
            }
            else
            {
                //Debug.Log(gameObject.name + " I CAN DRIVE!: " + stopTimer);
                onStop = false;
                speed = previousSpeed;
                stopTimer = Mathf.Infinity;
            }
        }
    }

    void OnCollisionEnter(Collision col)
    {
        GameObject collisionObect = col.gameObject;
        //Debug.Log(gameObject.name + " hits " + currentCW.name + "(" + col.collider.name + ")");
        switch (col.gameObject.tag)
        {
            case "Crosswalk":
                bool someoneInCW = collisionObect.GetComponent<CWScript>().IsPeopleCrossing();
                if (someoneInCW)
                {
                    previousSpeed = speed;
                    speed = 0;
                    onCW = true;
                    collisionObect.GetComponent<CWScript>().AddCarToQueue(gameObject.name);
                }
                break;
            case "Stop":
                Debug.Log(gameObject.name +  " ha llegado a <" + collisionObect.name + ">");
                previousSpeed = speed;
                speed = 0;
                stopTimer = 0;
                onStop = true;
                break;
            case "Car":
                //If the car is not waiting already and has collided with another car
                if (!onCW && !onStop)
                {
                    //Debug.Log(gameObject.name + " has collided with <" + currentCW.name + ">");

                    //Then it checks if the car with whom it has collided is waiting at stop or crosswalk
                    bool carOnCW = collisionObect.GetComponent<CarAI>().isOnCW();
                    bool carOnStop = collisionObect.GetComponent<CarAI>().isOnStop();

                    //The new car has to wait as well
                    if (carOnCW) onCW = true;
                    if (carOnStop) onStop = true;

                    if (onCW || onStop)
                    {
                        //Debug.Log(gameObject.name + " wait..." + " onCW: " + onCW + " onStop: " + onStop);
                        carInFront = true;
                        otherCar = collisionObect;
                        previousSpeed = speed;
                        speed = 0;
                    }
                }
                break;
        }
    }
    public void ResetSpeed()
    {
        speed = previousSpeed;
        onCW = false;
    }

    public bool isOnStop()
    {
        return onStop;
    }

    public bool isOnCW()
    {
        return onCW;
    }
}
