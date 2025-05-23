using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Limite : MonoBehaviour
{

    public static event Action EventoNuevoBloque;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bloque"))
        {
            EventoNuevoBloque?.Invoke();
            other.transform.position = Vector3.zero;
            other.gameObject.SetActive(false);
        }
    }
}
