using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PedestrianAI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Transform path;
    //[SerializeField] private Transform[] target;
    [SerializeField] private float speed;
    [SerializeField] private bool personCrossing = false;

    private List<Transform> nodes;
    private int currentNode;
    private GameObject currentCW;
    
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
            if (personCrossing == false)
            {
                Debug.Log("************person crossing " + col.collider.name);
                personCrossing = true;
                currentCW = col.gameObject;
                currentCW.GetComponent<CWScript>().PeopleIsCrossing(personCrossing);
            }
        }
        else
        {
            if (col.collider.name.Contains("Street"))
            {
                if (personCrossing == true)
                {
                    Debug.Log("************person reachs " + col.collider.name);
                    personCrossing = false;
                    currentCW.GetComponent<CWScript>().PeopleIsCrossing(personCrossing);
                }
            }
        }
    }
}