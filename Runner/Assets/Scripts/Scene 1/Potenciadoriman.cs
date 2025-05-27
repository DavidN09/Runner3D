using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potenciadoriman : MonoBehaviour
{

    public static event Action<float> EventoIman;
    [SerializeField] private float duracion;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EventoIman?.Invoke(duracion);
            gameObject.SetActive(false);

        }
    }

}
