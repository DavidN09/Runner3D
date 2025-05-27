using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamante : MonoBehaviour
{
    [SerializeField] private int valorDiamante = 1;

    private void ObtenerDiamante()
    {
        DiamanteManager.Instancia.AñadirDiamantes(valorDiamante);
        GameManager.Instancia.DiamantesObtenidosEnEsteNivel += valorDiamante;
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ObtenerDiamante();
        }
    }
}
