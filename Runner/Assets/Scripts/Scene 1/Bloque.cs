using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TipoBloque
{
    Facil,
    Dificil,
    Trenes
}

public class Bloque : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private TipoBloque tipoBloque;
    
    
    [Header("Tren")]
    [SerializeField] private Tren[] trenes;

    public TipoBloque TipoDeBloque => tipoBloque;

    public void InicializarBloque()
    {
        if (tipoBloque == TipoBloque.Trenes)
        {
            SeleccionarTren();
        }
    }

    private void SeleccionarTren()
    {
        if (trenes == null || trenes.Length == 0)
        {
            return;
        }
        int index = Random.Range(0, trenes.Length);
        trenes[index].gameObject.SetActive(true);
    }
}
