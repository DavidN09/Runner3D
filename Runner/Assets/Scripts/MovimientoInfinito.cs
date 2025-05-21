using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoInfinito : MonoBehaviour
{
    public float velocidad = 2f;       // Qué tan rápido se mueve
    public float distancia = 3f;       // Qué tan lejos se mueve hacia un lado

    private Vector3 posicionInicial;

    void Start()
    {
        posicionInicial = transform.position;
    }

    void Update()
    {
        float movimiento = Mathf.PingPong(Time.time * velocidad, distancia);
        transform.position = posicionInicial + Vector3.right * movimiento;
    }
}
