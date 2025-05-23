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
    [SerializeField] private int longitudBloqueNormal = 40;
    [SerializeField] private int longitudBloqueTrenes = 80;
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

        for (int i = 0; i < bloquesAlInicio; i++)
        {
            CrearBloque();
        }
    }



    private void CrearBloque()
    {
        if (bloquesCreados >= maxBloquesParaTrenes)
        {
            if (bloquesCreados < maxBloquesParaTrenes + 1)
            {
                AñadirBloque(TipoBloque.Trenes, longitudBloqueNormal);
            }
            else
            {
                AñadirBloque(TipoBloque.Trenes, longitudBloqueTrenes);
            }

            if (bloquesCreados == maxBloquesParaTrenes + maxBloquesTrenesReset)
            {
                bloquesCreados = 0;
            }
        }
        else if (bloquesCreados >= maxBloquesParaDificil)
        {
            AñadirBloque(TipoBloque.Dificil, longitudBloqueNormal);
        }
        else
        {
            if (ultimoBloque.TipoDeBloque == TipoBloque.Trenes)
            {
                AñadirBloque(TipoBloque.Facil, longitudBloqueTrenes);
            }
            else
            {
                AñadirBloque(TipoBloque.Facil, longitudBloqueNormal);
            }
        }
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            CrearBloque();
        }
    }


    private void AñadirBloque(TipoBloque tipo, float longitud)
    {
        Bloque nuevoBloque = ObtenerBloqueSegunTipo(tipo);
        nuevoBloque.transform.position = EstablecerPosNuevoBloque(longitud);
        ultimoBloque = nuevoBloque;
        bloquesCreados++;
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

    private Vector3 EstablecerPosNuevoBloque(float longitud)
    {
        return ultimoBloque.transform.position + Vector3.forward * longitud;
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

    private void RespuestaNuevoBloque()
    {
        CrearBloque();
    }

    private void OnEnable()
    {
        Limite.EventoNuevoBloque += RespuestaNuevoBloque;
    }

    private void OnDisable()
    {
        Limite.EventoNuevoBloque -= RespuestaNuevoBloque;
    }

}
