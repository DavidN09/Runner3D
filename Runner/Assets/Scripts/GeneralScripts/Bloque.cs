using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TipoBloque
{
    Facil,
    Dificil,
    Especial
}

public class Bloque : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private TipoBloque tipoBloque;
    
    
    [Header("Especial")]
    [SerializeField] private ObjEspecial[] ObjEspeciales;

    [Header("Diamantes")]
    [SerializeField] private GameObject[] diamantes;

    [Header("Potenciadores")]
    [SerializeField] private float probabilidadMinima;
    [SerializeField] private GameObject[] potenciadores;
    

    public TipoBloque TipoDeBloque => tipoBloque;

    private List<GameObject> diamantesLista = new List<GameObject>();
    private bool diamantesReferenciados;

    private ObjEspecial ObjEspecialSeleccionado;
    public void InicializarBloque()
    {
        if (tipoBloque == TipoBloque.Especial)
        {
            SeleccionarObjEspecial();
        }

        obtenerDiamantes();
        ActivarDiamantes();
        SeleccionarPotenciador();
    }

    private void SeleccionarPotenciador()
    {
        if (potenciadores == null)
        {
            return;
        }

        for (int i = 0; i < potenciadores.Length; i++)
        {
            potenciadores[i].SetActive(false); 
}
        float probabilidadRandom = Random.Range(0f, 100f);
        if (probabilidadRandom <= probabilidadMinima)
        {
            int itemRandomIndex = Random.Range(0, potenciadores.Length);
            potenciadores[itemRandomIndex].SetActive(true); 
}
    }

    private void obtenerDiamantes()
    {
        if (diamantesReferenciados)
        {
            return;
        }

        foreach (GameObject parent in diamantes)
        {
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                GameObject diamante = parent.transform.GetChild(i).gameObject;
                diamantesLista.Add(diamante);
            }
        }

        diamantesReferenciados = true;
    }

    private void ActivarDiamantes()
    {
        if (diamantesLista.Count == 0)
        {
            return;
        }

        foreach (GameObject diamante in diamantesLista)
        {
            diamante.SetActive(true);
        }
    }

    private void SeleccionarObjEspecial()
    {
        if (ObjEspeciales == null || ObjEspeciales.Length == 0)
        {
            return;
        }
        int index = Random.Range(0, ObjEspeciales.Length);
        ObjEspeciales[index].gameObject.SetActive(true);
        ObjEspecialSeleccionado = ObjEspeciales[index];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (ObjEspecialSeleccionado != null)
            {
                ObjEspecialSeleccionado.PuedeMoverse = true;
                ObjEspecialSeleccionado.Player = other.GetComponent<PlayerController>();
            }
        }
    }
}
