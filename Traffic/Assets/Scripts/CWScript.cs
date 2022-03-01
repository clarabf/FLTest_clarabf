using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CWScript : MonoBehaviour
{
    private List<int> peopleCrossingIds;
    private List<string> carsQueue;

    // Start is called before the first frame update
    void Start()
    {
        carsQueue = new List<string>();
        peopleCrossingIds = new List<int>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsPeopleCrossing()
    {
        return peopleCrossingIds.Count != 0;
    }

    public void AddPedestrian(int pedestrian_id)
    {
        peopleCrossingIds.Add(pedestrian_id);
    }

    public void RemovePedestrian(int pedestrian_id)
    {
        peopleCrossingIds.Remove(pedestrian_id);
        if (!IsPeopleCrossing())
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
