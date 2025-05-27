using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enum que representa los estados posibles del juego.
/// </summary>
public enum EstadosDelJuego
{
    Inicio,
    Jugando,
    GameOver
}

/// <summary>
/// Controlador general del juego. Administra el estado del juego, puntaje, niveles y eventos especiales.
/// Implementa el patrón Singleton a través de la clase base Singletton.
/// </summary>
public class GameManager : Singletton<GameManager>
{
    /// <summary>
    /// Lista de resultados almacenados por cada nivel jugado.
    /// </summary>
    public List<DatosNivel> resultadosPorNivel = new List<DatosNivel>();

    /// <summary>
    /// Total de diamantes obtenidos a lo largo de la partida.
    /// </summary>
    public int DiamantesTotales { get; private set; }

    /// <summary>
    /// Puntaje acumulado total del jugador.
    /// </summary>
    public int PuntajeTotal { get; private set; }

    /// <summary>
    /// Evento que se dispara cuando el efecto del imán ha terminado.
    /// </summary>
    public static event Action EventoImanFinalizado;

    [SerializeField] private int velocidadMundo = 5;
    [SerializeField] private int multiplicadorPuntajePorMoneda = 10;

    /// <summary>
    /// Calcula el puntaje del jugador en función de la distancia recorrida y los diamantes obtenidos.
    /// </summary>
    public int Puntaje => (int)distanciaRecorrida + DiamantesObtenidosEnEsteNivel * multiplicadorPuntajePorMoneda;

    /// <summary>
    /// Valor del multiplicador que puede aumentar la velocidad o puntaje.
    /// </summary>
    public float valorMultiplicador { get; set; }

    /// <summary>
    /// Estado actual del juego.
    /// </summary>
    public EstadosDelJuego EstadoActual { get; set; }

    /// <summary>
    /// Diamantes recogidos en el nivel actual.
    /// </summary>
    public int DiamantesObtenidosEnEsteNivel { get; set; }

    /// <summary>
    /// Nivel actual en el que se encuentra el jugador.
    /// </summary>
    public int NivelActual { get; set; } = 1;

    /// <summary>
    /// Distancia recorrida en el nivel actual.
    /// </summary>
    private float distanciaRecorrida;

    /// <summary>
    /// Inicializa el multiplicador al iniciar el juego.
    /// </summary>
    private void Start()
    {
        valorMultiplicador = 1f;
    }

    /// <summary>
    /// Actualiza la lógica del juego cada frame: cambia estado y suma distancia si el juego está activo.
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
    /// Registra el puntaje y diamantes del nivel actual, los suma al total y pasa al siguiente nivel.
    /// </summary>
    public void SumarProgresoDelNivel()
    {
        int puntajeNivel = Puntaje;
        int diamantesNivel = DiamantesObtenidosEnEsteNivel;

        PuntajeTotal += puntajeNivel;
        DiamantesTotales += diamantesNivel;

        resultadosPorNivel.Add(new DatosNivel(NivelActual, diamantesNivel, puntajeNivel));
        NivelActual++;
    }

    /// <summary>
    /// Reinicia los valores del nivel actual sin cambiar de nivel.
    /// </summary>
    public void ReiniciarNivelActual()
    {
        DiamantesObtenidosEnEsteNivel = 0;
        distanciaRecorrida = 0;
    }

    /// <summary>
    /// Cambia el estado del juego si el nuevo estado es diferente al actual.
    /// </summary>
    /// <param name="nuevoEstado">Estado al que se desea cambiar.</param>
    public void CambiarEstado(EstadosDelJuego nuevoEstado)
    {
        if (EstadoActual != nuevoEstado)
        {
            EstadoActual = nuevoEstado;
        }
    }

    /// <summary>
    /// Inicia la corrutina para el conteo del multiplicador.
    /// </summary>
    /// <param name="tiempo">Duración del efecto del multiplicador.</param>
    public void iniciarConteoMultiplicador(float tiempo)
    {
        StartCoroutine(COMultiplicadorConteo(tiempo));
    }

    /// <summary>
    /// Corrutina que espera un tiempo determinado antes de reiniciar el multiplicador.
    /// </summary>
    /// <param name="tiempo">Tiempo en segundos.</param>
    private IEnumerator COMultiplicadorConteo(float tiempo)
    {
        yield return new WaitForSeconds(tiempo);
        valorMultiplicador = 1;
    }

    /// <summary>
    /// Corrutina que espera el tiempo de duración del imán y luego dispara el evento de finalización.
    /// </summary>
    /// <param name="tiempo">Duración del efecto del imán.</param>
    private IEnumerator COImanConteo(float tiempo)
    {
        yield return new WaitForSeconds(tiempo);
        EventoImanFinalizado?.Invoke();
    }

    /// <summary>
    /// Método llamado en respuesta al evento del imán, inicia su corrutina de duración.
    /// </summary>
    /// <param name="duracion">Duración del efecto del imán.</param>
    private void RespuestaEventoIman(float duracion)
    {
        StartCoroutine(COImanConteo(duracion));
    }

    /// <summary>
    /// Se suscribe al evento del potenciador de imán cuando se activa este objeto.
    /// </summary>
    private void OnEnable()
    {
        Potenciadoriman.EventoIman += RespuestaEventoIman;
    }

    /// <summary>
    /// Se desuscribe del evento del potenciador de imán cuando se desactiva este objeto.
    /// </summary>
    private void OnDisable()
    {
        Potenciadoriman.EventoIman -= RespuestaEventoIman;
    }
}

