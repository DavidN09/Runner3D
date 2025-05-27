using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controla la generación de bloques del nivel, la transición al siguiente nivel y la dificultad progresiva.
/// </summary>
public class LevelManager : MonoBehaviour
{
    [SerializeField] private int diamantesNecesarios = 100;
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

    /// <summary>
    /// Obtiene el componente Pooler al iniciar el script.
    /// </summary>
    private void Awake()
    {
        pooler = GetComponent<Pooler>();
    }

    /// <summary>
    /// Reinicia el nivel y genera los bloques iniciales.
    /// </summary>
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

    /// <summary>
    /// Crea un nuevo bloque según la lógica de dificultad progresiva.
    /// </summary>
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

    /// <summary>
    /// Detecta si se debe cambiar de nivel o crear un bloque con una tecla.
    /// </summary>
    private void Update()
    {
        if (GameManager.Instancia.DiamantesObtenidosEnEsteNivel >= diamantesNecesarios)
        {
            CambiarNivel();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            CrearBloque();
        }
    }

    /// <summary>
    /// Cambia a la escena del siguiente nivel y guarda el progreso.
    /// </summary>
    private void CambiarNivel()
    {
        GameManager.Instancia.SumarProgresoDelNivel();
        SceneManager.LoadScene(siguienteNivel);
    }

    /// <summary>
    /// Añade un nuevo bloque al nivel, posicionándolo correctamente.
    /// </summary>
    /// <param name="tipo">Tipo de bloque a añadir.</param>
    /// <param name="longitud">Longitud que define el espacio del bloque.</param>
    private void AñadirBloque(TipoBloque tipo, float longitud)
    {
        Bloque nuevoBloque = ObtenerBloqueSegunTipo(tipo);
        nuevoBloque.transform.position = EstablecerPosNuevoBloque(longitud);
        ultimoBloque = nuevoBloque;
        bloquesCreados++;
    }

    /// <summary>
    /// Obtiene una instancia de bloque según el tipo desde el pooler.
    /// </summary>
    /// <param name="tipo">Tipo de bloque deseado.</param>
    /// <returns>Instancia del bloque.</returns>
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

    /// <summary>
    /// Obtiene un bloque aleatorio desde una lista específica usando el pooler.
    /// </summary>
    /// <param name="lista">Lista de bloques del tipo correspondiente.</param>
    /// <returns>Instancia del bloque del pool.</returns>
    private Bloque ObtenerInstanciaDelPooler(List<Bloque> lista)
    {
        int bloqueRandom = Random.Range(0, lista.Count);
        string nommbreDelBloque = lista[bloqueRandom].name;
        GameObject instancia = pooler.ObtenerInstanciaDelPooler(nommbreDelBloque);
        instancia.SetActive(true);
        Bloque bloque = instancia.GetComponent<Bloque>();
        return bloque;
    }

    /// <summary>
    /// Calcula la posición del siguiente bloque en base al último creado.
    /// </summary>
    /// <param name="longitud">Longitud del nuevo bloque.</param>
    /// <returns>Vector3 con la nueva posición.</returns>
    private Vector3 EstablecerPosNuevoBloque(float longitud)
    {
        return ultimoBloque.transform.position + Vector3.forward * longitud;
    }

    /// <summary>
    /// Clasifica los prefabs de bloques según su tipo para futuras instancias.
    /// </summary>
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

    /// <summary>
    /// Respuesta al evento de límite que indica que se debe crear un nuevo bloque.
    /// </summary>
    private void RespuestaNuevoBloque()
    {
        CrearBloque();
    }

    /// <summary>
    /// Se suscribe al evento de creación de bloque al activarse.
    /// </summary>
    private void OnEnable()
    {
        Limite.EventoNuevoBloque += RespuestaNuevoBloque;
    }

    /// <summary>
    /// Se desuscribe del evento de creación de bloque al desactivarse.
    /// </summary>
    private void OnDisable()
    {
        Limite.EventoNuevoBloque -= RespuestaNuevoBloque;
    }
}
