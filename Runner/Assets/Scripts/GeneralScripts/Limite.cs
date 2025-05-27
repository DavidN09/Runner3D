using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Clase que detecta cuándo un bloque cruza el límite del escenario.
/// Al detectar el evento, invoca una acción para generar un nuevo bloque y desactiva el actual.
/// </summary>
public class Limite : MonoBehaviour
{
    /// <summary>
    /// Evento estático que se dispara cuando un bloque colisiona con el límite.
    /// Utilizado para indicar que debe generarse un nuevo bloque.
    /// </summary>
    public static event Action EventoNuevoBloque;

    /// <summary>
    /// Método llamado automáticamente por Unity cuando otro collider entra en este trigger.
    /// Si el objeto tiene la etiqueta "Bloque", lanza el evento y lo desactiva.
    /// </summary>
    /// <param name="other">El collider que entra en el trigger.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bloque"))
        {
            EventoNuevoBloque?.Invoke();
            other.transform.position = Vector3.zero;
            other.gameObject.SetActive(false);
        }
    }
}
