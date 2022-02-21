using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Transform path;
    //[SerializeField] private Transform[] target;
    [SerializeField] private float speed;

    private List<Transform> nodes;
    private int currentNode;
    private bool personCrossing = false;

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
        if (transform.name == "SimplePerson")
        {
            if (col.collider.name.Contains("CW"))
            {
                Debug.Log("************person crossing!");
                personCrossing = true;
            }
            else
            {
                personCrossing = false; //the person is in the street,
            }
        }
        else if (transform.name == "SimpleCar")
        {
            if (col.collider.name.Contains("CW"))
            {
                Debug.Log("************COCHEEE: " + col.collider.name + " personCrossing: " + personCrossing);
                if (personCrossing) Debug.Log("************I HAVE TO STOOOOP!");
            }
        }
    }
}
