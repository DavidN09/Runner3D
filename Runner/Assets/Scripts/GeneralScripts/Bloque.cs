using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enum que define los tipos posibles de bloque: Fácil, Difícil o Especial.
/// </summary>
public enum TipoBloque
{
    Facil,
    Dificil,
    Especial
}

/// <summary>
/// Clase que representa un bloque dentro del nivel.
/// Puede contener objetos especiales, diamantes o potenciadores.
/// </summary>
public class Bloque : MonoBehaviour
{
    /// <summary>
    /// Tipo del bloque actual (Fácil, Difícil, Especial).
    /// </summary>
    [Header("Config")]
    [SerializeField] private TipoBloque tipoBloque;

    /// <summary>
    /// Lista de objetos especiales que se pueden activar si el bloque es especial.
    /// </summary>
    [Header("Especial")]
    [SerializeField] private ObjEspecial[] ObjEspeciales;

    /// <summary>
    /// Contenedores de diamantes que se deben activar dentro del bloque.
    /// </summary>
    [Header("Diamantes")]
    [SerializeField] private GameObject[] diamantes;

    /// <summary>
    /// Probabilidad mínima para que se active un potenciador.
    /// </summary>
    [Header("Potenciadores")]
    [SerializeField] private float probabilidadMinima;

    /// <summary>
    /// Arreglo de potenciadores disponibles en el bloque.
    /// </summary>
    [SerializeField] private GameObject[] potenciadores;

    /// <summary>
    /// Retorna el tipo de bloque.
    /// </summary>
    public TipoBloque TipoDeBloque => tipoBloque;

    /// <summary>
    /// Lista interna con todos los diamantes referenciados del bloque.
    /// </summary>
    private List<GameObject> diamantesLista = new List<GameObject>();

    /// <summary>
    /// Marca si los diamantes ya fueron referenciados.
    /// </summary>
    private bool diamantesReferenciados;

    /// <summary>
    /// Objeto especial que fue seleccionado aleatoriamente para este bloque.
    /// </summary>
    private ObjEspecial ObjEspecialSeleccionado;

    /// <summary>
    /// Inicializa el bloque al entrar en juego. Prepara objetos especiales, diamantes y potenciadores.
    /// </summary>
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

    /// <summary>
    /// Activa aleatoriamente un potenciador según la probabilidad mínima.
    /// </summary>
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

    /// <summary>
    /// Busca y guarda en lista todos los diamantes hijos de los contenedores.
    /// </summary>
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

    /// <summary>
    /// Activa todos los diamantes referenciados del bloque.
    /// </summary>
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

    /// <summary>
    /// Selecciona aleatoriamente un objeto especial y lo activa.
    /// </summary>
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

    /// <summary>
    /// Detecta la entrada del jugador al bloque y activa la lógica del objeto especial, si corresponde.
    /// </summary>
    /// <param name="other">Collider del objeto que entra al bloque.</param>
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

