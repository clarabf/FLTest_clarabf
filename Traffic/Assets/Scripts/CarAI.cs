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
    private bool inStop = false;

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

        //Waiting at the stop
        if (inStop) 
        {
            if (stopTimer < stopDuration)
            {
                Debug.Log("STOP!: " + stopTimer);
                stopTimer += Time.deltaTime;
            }
            else
            {
                Debug.Log("PUEDO SEGUIR!: " + stopTimer);
                inStop = false;
                speed = previousSpeed;
                stopTimer = Mathf.Infinity;
            }
        }
    }

    void OnCollisionEnter(Collision col)
    {
        GameObject currentCW = col.gameObject;
        switch (col.gameObject.tag)
        {
            case "Crosswalk":
                bool someoneInCW = currentCW.GetComponent<CWScript>().IsPeopleCrossing();
                if (someoneInCW)
                {
                    previousSpeed = speed;
                    speed = 0;
                    currentCW.GetComponent<CWScript>().AddCarToQueue(gameObject.name);
                }
                break;
            case "Stop":
                Debug.Log("He llegado a <" + currentCW.name + ">");
                previousSpeed = speed;
                speed = 0;
                stopTimer = 0;
                inStop = true;
                break;
            case "Car":

                break;
        }
    }

    public void ResetSpeed()
    {
        speed = previousSpeed;
    }
}
