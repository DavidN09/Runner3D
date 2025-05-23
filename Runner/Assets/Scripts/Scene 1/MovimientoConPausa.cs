using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoConPausa : MonoBehaviour
{
    public float velocidad = 2f;             // Velocidad del movimiento
    public float distancia = 3f;             // Distancia de ida
    public float tiempoDeEspera = 3f;        // Tiempo de pausa al llegar a cada extremo

    private Vector3 puntoA;
    private Vector3 puntoB;
    private bool moviendoAHaciaB = true;


    void Start()
    {
        puntoA = transform.position;
        puntoB = puntoA + Vector3.right * distancia;
        StartCoroutine(Mover());
    }

    IEnumerator Mover()
    {
        while (true)
        {
            Vector3 destino = moviendoAHaciaB ? puntoB : puntoA;

            // Mover hacia el destino
            while (Vector3.Distance(transform.position, destino) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, destino, velocidad * Time.deltaTime);
                yield return null;
            }

            // Asegurarse de estar exactamente en el destino
            transform.position = destino;

            // Esperar
            yield return new WaitForSeconds(tiempoDeEspera);

            // Cambiar dirección
            moviendoAHaciaB = !moviendoAHaciaB;
        }
    }
}
