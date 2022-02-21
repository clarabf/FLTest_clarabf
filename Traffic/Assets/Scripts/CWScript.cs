using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CWScript : MonoBehaviour
{
    // Start is called before the first frame update
    public bool peopleCrossing = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsPeopleCrossing()
    {
        return peopleCrossing;
    }

    public void PeopleIsCrossing(bool crossingOrNot)
    {
        peopleCrossing = crossingOrNot;
    }

}
