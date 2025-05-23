using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingCarTrafficManager : MonoBehaviour
{
    public TrafficLane[] lanes;               
    public GameObject[] carPrefabs;           
    public float carSpeed = 10f;              

    void Start()
    {
        foreach (var lane in lanes)
        {
            StartCoroutine(SpawnCarsInLane(lane));
        }
    }

    private IEnumerator SpawnCarsInLane(TrafficLane lane)
    {
        while (true)
        {
            SpawnCar(lane);
            yield return new WaitForSeconds(lane.spawnInterval);
        }
    }

    private void SpawnCar(TrafficLane lane)
    {
        int index = Random.Range(0, carPrefabs.Length);
        Vector3 spawnPos = lane.spawnPoint.position;

        
        Vector3 direction = (lane.endPoint.position - spawnPos).normalized;

        
        GameObject car = Instantiate(carPrefabs[index], spawnPos, Quaternion.LookRotation(direction));

      
        FloatingCarMover mover = car.AddComponent<FloatingCarMover>();
        mover.SetTarget(lane.endPoint.position, carSpeed);
    }
}
