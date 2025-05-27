using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Administra la generación y posicionamiento de bloques en el nivel,
/// alternando entre bloques fáciles, difíciles y especiales.
/// </summary>
public class LevelManager : MonoBehaviour
{
    /// <summary>
    /// Cantidad de bloques que se generan al inicio del nivel.
    /// </summary>
    [Header("Config")]
    [SerializeField] private int bloquesAlInicio = 5;

    /// <summary>
    /// Número máximo de bloques fáciles antes de comenzar a generar difíciles.
    /// </summary>
    [SerializeField] private int maxBloquesParaDificil = 5;

    /// <summary>
    /// Número máximo de bloques difíciles antes de comenzar a generar especiales.
    /// </summary>
    [SerializeField] private int maxBloquesParaEspecial = 10;

    /// <summary>
    /// Cantidad de bloques especiales antes de reiniciar el ciclo de dificultad.
    /// </summary>
    [SerializeField] private int maxBloquesEspecialReset = 3;

    /// <summary>
    /// Bloque inicial colocado al inicio del nivel.
    /// </summary>
    [Header("Bloques")]
    [SerializeField] private Bloque bloqueInicial;

    /// <summary>
    /// Longitud de un bloque normal (fácil o difícil).
    /// </summary>
    [SerializeField] private int longitudBloqueNormal = 40;

    /// <summary>
    /// Longitud de un bloque especial.
    /// </summary>
    [SerializeField] private int longitudBloqueEspecial = 80;

    /// <summary>
    /// Prefabs disponibles para instanciar bloques.
    /// </summary>
    [SerializeField] private Bloque[] bloquesPrefab;

    /// <summary>
    /// Lista de bloques fáciles disponibles.
    /// </summary>
    private List<Bloque> listaBloquesFaciles = new List<Bloque>();

    /// <summary>
    /// Lista de bloques difíciles disponibles.
    /// </summary>
    private List<Bloque> listaBloquesDificiles = new List<Bloque>();

    /// <summary>
    /// Lista de bloques especiales disponibles.
    /// </summary>
    private List<Bloque> listaBloquesEspeciales = new List<Bloque>();

    /// <summary>
    /// Referencia al sistema de pooling para optimizar la reutilización de bloques.
    /// </summary>
    private Pooler pooler;

    /// <summary>
    /// Último bloque instanciado, usado para calcular la posición del siguiente.
    /// </summary>
    private Bloque ultimoBloque;

    /// <summary>
    /// Cantidad de bloques generados en el ciclo actual.
    /// </summary>
    private int bloquesCreados;

    /// <summary>
    /// Obtiene el componente Pooler al iniciar la escena.
    /// </summary>
    private void Awake()
    {
        pooler = GetComponent<Pooler>();
    }

    /// <summary>
    /// Inicializa las listas de bloques y genera los bloques iniciales al comenzar el juego.
    /// </summary>
    void Start()
    {
        LLenarBloquesSegunTipo();
        ultimoBloque = bloqueInicial;

        for (int i = 0; i < bloquesAlInicio; i++)
        {
            CrearBloque();
        }
    }

    /// <summary>
    /// Genera un nuevo bloque según la dificultad actual del nivel.
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
    /// Permite crear un nuevo bloque manualmente al presionar la tecla G.
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            CrearBloque();
        }
    }

    /// <summary>
    /// Añade un nuevo bloque al nivel y actualiza su posición.
    /// </summary>
    /// <param name="tipo">Tipo de bloque a instanciar.</param>
    /// <param name="longitud">Longitud del bloque para calcular su posición.</param>
    private void AñadirBloque(TipoBloque tipo, float longitud)
    {
        Bloque nuevoBloque = ObtenerBloqueSegunTipo(tipo);
        nuevoBloque.transform.position = EstablecerPosNuevoBloque(longitud);
        ultimoBloque = nuevoBloque;
        bloquesCreados++;
    }

    /// <summary>
    /// Obtiene una instancia de bloque del tipo especificado desde el pool.
    /// </summary>
    /// <param name="tipo">Tipo de bloque requerido.</param>
    /// <returns>Instancia del bloque listo para colocar en el nivel.</returns>
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
    /// Obtiene una instancia activa de un bloque aleatorio desde una lista específica.
    /// </summary>
    /// <param name="lista">Lista de bloques disponibles del tipo requerido.</param>
    /// <returns>Bloque activado y listo para usar.</returns>
    private Bloque ObtenerInstanciaDelPooler(List<Bloque> lista)
    {
        int bloqueRandom = Random.Range(0, lista.Count);
        string nombreDelBloque = lista[bloqueRandom].name;
        GameObject instancia = pooler.ObtenerInstanciaDelPooler(nombreDelBloque);
        instancia.SetActive(true);
        Bloque bloque = instancia.GetComponent<Bloque>();
        return bloque;
    }

    /// <summary>
    /// Calcula la posición del siguiente bloque a partir del último bloque instanciado.
    /// </summary>
    /// <param name="longitud">Longitud del nuevo bloque.</param>
    /// <returns>Posición en la que se colocará el nuevo bloque.</returns>
    private Vector3 EstablecerPosNuevoBloque(float longitud)
    {
        return ultimoBloque.transform.position + Vector3.forward * longitud;
    }

    /// <summary>
    /// Llena las listas de bloques separándolos por tipo, usando los prefabs asignados.
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
    /// Respuesta al evento que indica que debe generarse un nuevo bloque.
    /// </summary>
    private void RespuestaNuevoBloque()
    {
        CrearBloque();
    }

    /// <summary>
    /// Se suscribe al evento de nuevo bloque al habilitar el objeto.
    /// </summary>
    private void OnEnable()
    {
        Limite.EventoNuevoBloque += RespuestaNuevoBloque;
    }

    /// <summary>
    /// Se desuscribe del evento de nuevo bloque al deshabilitar el objeto.
    /// </summary>
    private void OnDisable()
    {
        Limite.EventoNuevoBloque -= RespuestaNuevoBloque;
    }
}
