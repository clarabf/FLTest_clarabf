using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CWScript : MonoBehaviour
{
    // Start is called before the first frame update
    public bool peopleCrossing = false;
    private List<string> carsQueue;

    void Start()
    {
        carsQueue = new List<string>();
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
        if (!peopleCrossing)
        {
            if (carsQueue.Count > 0)
            {
                GameObject car = GameObject.Find(carsQueue.First());
                car.GetComponent<CarAI>().ResetSpeed();
                RemoveCarFromQueue(car.name);
            }
        }
    }

    public void AddCarToQueue(string car_name)
    {
        carsQueue.Add(car_name);
    }

    public void RemoveCarFromQueue(string car_name)
    {
        carsQueue.Remove(car_name);
    }

}
