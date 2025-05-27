using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EstadosDelJuego
{
    Inicio,
    Jugando,
    GameOver
}


public class GameManager : Singletton<GameManager>
{

    public List<DatosNivel> resultadosPorNivel = new List<DatosNivel>();

    public int DiamantesTotales { get; private set; }
    public int PuntajeTotal { get; private set; }

    public static event Action EventoImanFinalizado;

    [SerializeField] private int velocidadMundo = 5;
    [SerializeField] private int multiplicadorPuntajePorMoneda = 10;

    public int Puntaje => (int) distanciaRecorrida + DiamantesObtenidosEnEsteNivel * multiplicadorPuntajePorMoneda;

    public float valorMultiplicador { get; set; }     

    public EstadosDelJuego EstadoActual { get; set; }
    public int DiamantesObtenidosEnEsteNivel { get; set; }
    public int NivelActual { get; set; } = 1;

    private float distanciaRecorrida;



    private void Start()
    {
        valorMultiplicador = 1f;
    }
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

    public void SumarProgresoDelNivel()
    {
        int puntajeNivel = Puntaje;
        int diamantesNivel = DiamantesObtenidosEnEsteNivel;

        PuntajeTotal += puntajeNivel;
        DiamantesTotales += diamantesNivel;

        resultadosPorNivel.Add(new DatosNivel(NivelActual, diamantesNivel, puntajeNivel));
        NivelActual++;
    }

    public void ReiniciarNivelActual()
    {
        DiamantesObtenidosEnEsteNivel = 0;
        distanciaRecorrida = 0;
    }

    public void CambiarEstado(EstadosDelJuego nuevoEstado)
    {
        if (EstadoActual != nuevoEstado)
        {
            EstadoActual = nuevoEstado;
        }
    } 

    public void iniciarConteoMultiplicador(float tiempo)
    {
        StartCoroutine(COMultiplicadorConteo(tiempo));
    }

    private IEnumerator COMultiplicadorConteo(float tiempo)
    {
        yield return new WaitForSeconds(tiempo);
        valorMultiplicador = 1;
    }

    private IEnumerator COImanConteo(float tiempo)
    {
        yield return new WaitForSeconds(tiempo);
        EventoImanFinalizado?.Invoke();
        
    }

    private void RespuestaEventoIman(float duracion)
    {
        StartCoroutine(COImanConteo(duracion));
    }

    private void OnEnable()
    {
        Potenciadoriman.EventoIman += RespuestaEventoIman;
    }

    private void OnDisable()
    {
        Potenciadoriman.EventoIman -= RespuestaEventoIman;

    }
}
