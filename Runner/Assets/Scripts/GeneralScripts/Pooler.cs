using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pooler : MonoBehaviour
{
    [SerializeField] private string nombreDelPooler;
    [SerializeField] private GameObject[] objsPorCrear;
    [SerializeField] private int cantidadPorObjeto;

    private List<GameObject> instanciasCreadas = new List<GameObject>();
    private GameObject contenedorPooler;

    private void Awake()
    {
        contenedorPooler = new GameObject(name: $"Pooler - {nombreDelPooler}");
        CrearPooler();
    }
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

    private GameObject AñadirInstancia(GameObject obj)
    {
        GameObject nuevoObj = Instantiate(obj, contenedorPooler.transform);
        nuevoObj.name = obj.name;
        nuevoObj.SetActive(false);
        return nuevoObj;
    }

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
