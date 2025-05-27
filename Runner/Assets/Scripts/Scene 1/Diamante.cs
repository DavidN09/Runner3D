using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamante : MonoBehaviour
{
    [SerializeField] private int valorDiamante = 1;
    [SerializeField] private float colliderNuevoTamaño = 5f;
    [SerializeField] private float distanciaMinPlayer = 1.5f;
    [SerializeField] private float velocidadMovimiento = 10f;

    public Transform Player { get; set; }


    private BoxCollider collider;
    private Vector3 tamañoInicial;
    private bool imanActivado;

    private void Start()
    {
        collider = GetComponent<BoxCollider>();
        tamañoInicial = collider.size;
    }

    private void Update()
    {
        if (Player != null)
        {
            Debug.DrawLine(transform.position, Player.position, Color.blue);
            MoverDiamanteHaciaPersonaje();
        }
    }

    private void ObtenerDiamante()
    {
        DiamanteManager.Instancia.AñadirDiamantes(valorDiamante);
        GameManager.Instancia.DiamantesObtenidosEnEsteNivel += valorDiamante;
        gameObject.SetActive(false);
    }

    private void MoverDiamanteHaciaPersonaje()
    {
        if (Vector3.Distance(Player.position, transform.position) > 0.1f)
        {
            if (Vector3.Distance(Player.position, transform.position) < distanciaMinPlayer)
            {
                ObtenerDiamante();
            }
            transform.position = Vector3.MoveTowards(transform.position, Player.position + Vector3.up, velocidadMovimiento * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (imanActivado)
            {
                Player = other.transform;
            }
            else
            {
                ObtenerDiamante();
            }
        }
    }

    private void RespuestaEventoIman(float duracion)
    {
        collider.size *= colliderNuevoTamaño;
        imanActivado = true;
    }

    private void RespuestaImanFinalizado()
    {
        collider.size = tamañoInicial;
        imanActivado = false;
    }

    private void OnEnable()
    {
        Potenciadoriman.EventoIman += RespuestaEventoIman;
        GameManager.EventoImanFinalizado += RespuestaImanFinalizado;
    }

    private void OnDisable()
    {
        Potenciadoriman.EventoIman -= RespuestaEventoIman;
        GameManager.EventoImanFinalizado -= RespuestaImanFinalizado;

    }
}
