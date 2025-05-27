using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sistema de pooling que instancia previamente un número definido de objetos
/// y los reutiliza para mejorar el rendimiento.
/// </summary>
public class Pooler : MonoBehaviour
{
    /// <summary>
    /// Nombre que identifica a este pooler en la jerarquía.
    /// </summary>
    [SerializeField] private string nombreDelPooler;

    /// <summary>
    /// Arreglo de objetos que se deben instanciar dentro del pool.
    /// </summary>
    [SerializeField] private GameObject[] objsPorCrear;

    /// <summary>
    /// Cantidad de instancias que se generarán por cada objeto en el pool.
    /// </summary>
    [SerializeField] private int cantidadPorObjeto;

    /// <summary>
    /// Lista que almacena todas las instancias creadas para reutilización.
    /// </summary>
    private List<GameObject> instanciasCreadas = new List<GameObject>();

    /// <summary>
    /// GameObject contenedor que agrupa todas las instancias del pool en la jerarquía.
    /// </summary>
    private GameObject contenedorPooler;

    /// <summary>
    /// Al iniciar el objeto, crea el contenedor del pool y genera las instancias iniciales.
    /// </summary>
    private void Awake()
    {
        contenedorPooler = new GameObject(name: $"Pooler - {nombreDelPooler}");
        CrearPooler();
    }

    /// <summary>
    /// Crea las instancias de cada objeto configurado, según la cantidad deseada.
    /// </summary>
    private void CrearPooler()
    {
        for (int i = 0; i < objsPorCrear.Length; i++)
        {
            for (int j = 0; j < cantidadPorObjeto; j++)
            {
                instanciasCreadas.Add(item: AñadirInstancia(objsPorCrear[i]));
            }
        }
    }

    /// <summary>
    /// Instancia un nuevo objeto, lo desactiva, lo nombra y lo añade al contenedor del pool.
    /// </summary>
    /// <param name="obj">Prefab a instanciar.</param>
    /// <returns>Instancia recién creada y desactivada del objeto.</returns>
    private GameObject AñadirInstancia(GameObject obj)
    {
        GameObject nuevoObj = Instantiate(obj, contenedorPooler.transform);
        nuevoObj.name = obj.name;
        nuevoObj.SetActive(false);
        return nuevoObj;
    }

    /// <summary>
    /// Devuelve una instancia inactiva del pool que coincida con el nombre especificado.
    /// </summary>
    /// <param name="nombre">Nombre del objeto a buscar en el pool.</param>
    /// <returns>Una instancia disponible del objeto solicitado o null si no hay ninguna libre.</returns>
    public GameObject ObtenerInstanciaDelPooler(string nombre)
    {
        for (int i = 0; i < instanciasCreadas.Count; i++)
        {
            if (instanciasCreadas[i].name == nombre)
            {
                if (instanciasCreadas[i].activeSelf == false)
                {
                    return instanciasCreadas[i];
                }
            }
        }
        return null;
    }
}

