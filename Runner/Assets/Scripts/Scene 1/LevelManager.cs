using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private int bloquesAlInicio = 5;
    [SerializeField] private int maxBloquesParaDificil = 5;
    [SerializeField] private int maxBloquesParaTrenes = 10;
    [SerializeField] private int maxBloquesTrenesReset = 3;

    [Header("Bloques")]
    [SerializeField] private Bloque bloqueInicial;
    [SerializeField] private Bloque[] bloquesPrefab;

    private List<Bloque> listtaBloquesFaciles = new List<Bloque>();
    private List<Bloque> listtaBloquesDificiles = new List<Bloque>();
    private List<Bloque> listtaBloquesTrenes = new List<Bloque>();

    private Pooler pooler;
    private Bloque ultimoBloque;
    private int bloquesCreados;

    private void Awake()
    {
        pooler = GetComponent<Pooler>();
    }

    void Start()
    {
        LLenarBloquesSegunTipo();
        ultimoBloque = bloqueInicial;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Bloque bloque = ObtenerBloqueSegunTipo(TipoBloque.Facil);
            bloque.transform.position = bloqueInicial.transform.position + Vector3.forward * 40f;
        }
    }

    private Bloque ObtenerBloqueSegunTipo(TipoBloque tipo)
    {
        Bloque nuevoBloque = null;

        switch (tipo)
        {
            case TipoBloque.Facil:
                nuevoBloque = ObtenerInstanciaDelPooler(listtaBloquesFaciles);
                break;
            case TipoBloque.Dificil:
                nuevoBloque = ObtenerInstanciaDelPooler(listtaBloquesDificiles);
                break;
            case TipoBloque.Trenes:
                nuevoBloque = ObtenerInstanciaDelPooler(listtaBloquesTrenes);
                break;
        }

        if (nuevoBloque != null)
        {
            nuevoBloque.InicializarBloque();
        }
        return nuevoBloque;
    }

    private Bloque ObtenerInstanciaDelPooler(List<Bloque> lista)
    {
        int bloqueRandom = Random.Range(0, lista.Count);
        string nommbreDelBloque = lista[bloqueRandom].name;
        GameObject instancia = pooler.ObtenerInstanciaDelPooler(nommbreDelBloque);
        instancia.SetActive(true);
        Bloque bloque = instancia.GetComponent<Bloque>();
        return bloque;
    }

    private void LLenarBloquesSegunTipo()
    {
        foreach (Bloque bloque in bloquesPrefab)
        {
            switch (bloque.TipoDeBloque)
            {
                case TipoBloque.Facil:
                    listtaBloquesFaciles.Add(bloque);
                    break;
                case TipoBloque.Dificil:
                    listtaBloquesDificiles.Add(bloque);
                    break;
                case TipoBloque.Trenes:
                    listtaBloquesTrenes.Add(bloque);
                    break;
            }
        }
    }



}
