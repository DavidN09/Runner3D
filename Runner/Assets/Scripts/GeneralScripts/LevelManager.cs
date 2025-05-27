using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    [SerializeField] private int puntajeNecesario = 1000;
    [SerializeField] private string siguienteNivel;

    [Header("Config")]
    [SerializeField] private int bloquesAlInicio = 5;
    [SerializeField] private int maxBloquesParaDificil = 5;
    [SerializeField] private int maxBloquesParaEspecial = 10;
    [SerializeField] private int maxBloquesEspecialReset = 3;

    [Header("Bloques")]
    [SerializeField] private Bloque bloqueInicial;
    [SerializeField] private int longitudBloqueNormal = 40;
    [SerializeField] private int longitudBloqueEspecial = 80;   
    [SerializeField] private Bloque[] bloquesPrefab;

    private List<Bloque> listaBloquesFaciles = new List<Bloque>();
    private List<Bloque> listaBloquesDificiles = new List<Bloque>();
    private List<Bloque> listaBloquesEspeciales = new List<Bloque>();

    private Pooler pooler;
    private Bloque ultimoBloque;
    private int bloquesCreados;

    private void Awake()
    {
        pooler = GetComponent<Pooler>();
    }

    void Start()
    {
        GameManager.Instancia.ReiniciarNivelActual();
        LLenarBloquesSegunTipo();
        ultimoBloque = bloqueInicial;

        for (int i = 0; i < bloquesAlInicio; i++)
        {
            CrearBloque();
        }
    }



    private void CrearBloque()
    {
        if (bloquesCreados >= maxBloquesParaEspecial)
        {
            if (bloquesCreados < maxBloquesParaEspecial + 1)
            {
                AñadirBloque(TipoBloque.Especial, longitudBloqueNormal);
            }
            else
            {
                AñadirBloque(TipoBloque.Especial, longitudBloqueEspecial);
            }

            if (bloquesCreados == maxBloquesParaEspecial + maxBloquesEspecialReset)
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
            if (ultimoBloque.TipoDeBloque == TipoBloque.Especial)
            {
                AñadirBloque(TipoBloque.Facil, longitudBloqueEspecial);
            }
            else
            {
                AñadirBloque(TipoBloque.Facil, longitudBloqueNormal);
            }
        }
        
    }

    private void Update()
    {
        if (GameManager.Instancia.Puntaje >= puntajeNecesario)
        {
            CambiarNivel();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            CrearBloque();
        }
    }

    private void CambiarNivel()
    {
        GameManager.Instancia.SumarProgresoDelNivel();
        SceneManager.LoadScene(siguienteNivel);
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
                nuevoBloque = ObtenerInstanciaDelPooler(listaBloquesFaciles);
                break;
            case TipoBloque.Dificil:
                nuevoBloque = ObtenerInstanciaDelPooler(listaBloquesDificiles);
                break;
            case TipoBloque.Especial:
                nuevoBloque = ObtenerInstanciaDelPooler(listaBloquesEspeciales);
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
                    listaBloquesFaciles.Add(bloque);
                    break;
                case TipoBloque.Dificil:
                    listaBloquesDificiles.Add(bloque);
                    break;
                case TipoBloque.Especial:
                    listaBloquesEspeciales.Add(bloque);
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
