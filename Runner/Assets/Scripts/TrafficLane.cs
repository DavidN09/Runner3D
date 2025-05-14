using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TrafficLane
{
    public Transform spawnPoint;       // Desde dónde aparece el coche
    public Transform endPoint;         // Hacia dónde se mueve
    public float spawnInterval = 2f;   // Tiempo entre spawns
}
