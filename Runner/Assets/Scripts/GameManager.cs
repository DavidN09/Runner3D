using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Define los estados del juego.
/// </summary>
public enum EstadosDelJuego
{
    Inicio,
    Jugando,
    GameOver
}

/// <summary>
/// Controlador principal del juego que gestiona el estado del juego, la puntuación y los eventos relacionados con el juego.
/// Implementa el patrón Singleton para asegurar que solo haya una instancia de GameManager en la escena.
/// </summary>


public class GameManager : Singletton<GameManager>
{
    /// <summary>
    /// Evento que se invoca cuando el imán finaliza su efecto.
    /// </summary>

    public static event Action EventoImanFinalizado;

    /// <summary>
    /// Velocidad del mundo, que afecta la distancia recorrida por el jugador.
    /// </summary>

    [SerializeField] private int velocidadMundo = 5;
    /// <summary>
    /// Multiplicador de puntuación por diamante recogido.
    /// </summary>
    [SerializeField] private int multiplicadorPuntajePorMoneda = 10;

    /// <summary>
    /// puntuación total del jugador, calculada a partir de la distancia recorrida y los diamantes obtenidos en el nivel.
    /// </summary>
    public int Puntaje => (int) distanciaRecorrida + DiamantesObtenidosEnEsteNivel * multiplicadorPuntajePorMoneda;
    /// <summary>
    /// Factor que multiplica la velocidad del mundo. Puede ser afectado por potenciadores como el imán.
    /// </summary>
    public float valorMultiplicador { get; set; }

    /// <summary>
    /// Estado actual del juego, que puede ser Inicio, Jugando o GameOver.
    /// </summary>
    public EstadosDelJuego EstadoActual { get; set; }
    /// <summary>
    /// Numero de diamantes obtenidos en el nivel actual.
    /// </summary>
    public int DiamantesObtenidosEnEsteNivel { get; set; }
    /// <summary>
    /// Distancia recorrida por el jugador en el nivel actual.
    private float distanciaRecorrida;

    /// <summary>
    /// Inicializa valores por defecto al iniciar el juego.
    /// </summary>
    private void Start()
    {
        valorMultiplicador = 1f;
    }
    /// <summary>
    /// Actualiza la logica del juego en cada frame. Maneja la entrada del usuario y actualiza la distancia recorrida.
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CambiarEstado(EstadosDelJuego.Jugando);
        }

        if (EstadoActual == EstadosDelJuego.Inicio || EstadoActual == EstadosDelJuego.GameOver)
        {
            return;
        }

        distanciaRecorrida += Time.deltaTime * velocidadMundo * valorMultiplicador;  
    }

    /// <summary>
    /// Cambia el estado actual del juego si es diferente al nuevo estado proporcionado.
    /// </summary>
    /// <param name="nuevoEstado">nuevo estado a estableer</param>
    public void CambiarEstado(EstadosDelJuego nuevoEstado)
    {
        if (EstadoActual != nuevoEstado)
        {
            EstadoActual = nuevoEstado;
        }
    }

    /// <summary>
    /// Inicia un conteo regresivo que modifica temporalmente el multiplicador de velocidad.
    /// </summary>
    /// <param name="tiempo">Duracion del efecto del multiplicador.</param>
    public void iniciarConteoMultiplicador(float tiempo)
    {
        StartCoroutine(COMultiplicadorConteo(tiempo));
    }

    /// <summary>
    /// Corrutina que restaura el multiplicador de velocidad tras cierto tiempo.
    /// </summary>
    /// <param name="tiempo">Duración del multiplicador.</param>
    /// <returns>Una enumeración que espera.</returns>

    private IEnumerator COMultiplicadorConteo(float tiempo)
    {
        yield return new WaitForSeconds(tiempo);
        valorMultiplicador = 1;
    }
    /// <summary>
    /// Corrutina que espera cierto tiempo antes de finalizar el efecto del imán.
    /// </summary>
    /// <param name="tiempo">Duración del imán.</param>
    /// <returns>Una enumeración que espera.</returns>

    private IEnumerator COImanConteo(float tiempo)
    {
        yield return new WaitForSeconds(tiempo);
        EventoImanFinalizado?.Invoke();
        
    }

    /// <summary>
    /// Maneja el evento cuando se activa el imán, iniciando su conteo regresivo.
    /// </summary>
    /// <param name="duracion">Duración del efecto del imán.</param>

    private void RespuestaEventoIman(float duracion)
    {
        StartCoroutine(COImanConteo(duracion));
    }

    /// <summary>
    /// Se suscribe al evento del imán al habilitar el objeto.
    /// </summary>

    private void OnEnable()
    {
        Potenciadoriman.EventoIman += RespuestaEventoIman;
    }

    /// <summary>
    /// Se desuscribe del evento del imán al deshabilitar el objeto.
    /// </summary>


    private void OnDisable()
    {
        Potenciadoriman.EventoIman -= RespuestaEventoIman;

    }
}
