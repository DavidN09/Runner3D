using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplicadorPuntaje : MonoBehaviour
{
    [SerializeField] private float valor;
    [SerializeField] private float duracion;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instancia.valorMultiplicador = valor;
            GameManager.Instancia.iniciarConteoMultiplicador(duracion);
            gameObject.SetActive(false);
        }
    }
}
