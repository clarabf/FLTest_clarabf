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
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Crosswalk")
        {
            GameObject currentCW = col.gameObject;
            bool someoneInCW = currentCW.GetComponent<CWScript>().IsPeopleCrossing();
            if (someoneInCW)
            {
                Debug.Log(gameObject.name + " HAS TO STOOOOOOOOOOOOOOOP AT " + col.collider.name);
                previousSpeed = speed;
                speed = 0;
                currentCW.GetComponent<CWScript>().AddCarToQueue(gameObject.name);
            }
        }
    }

    public void ResetSpeed()
    {
        speed = previousSpeed;
    }
}
