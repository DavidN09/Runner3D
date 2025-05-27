using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controla el movimiento de un objeto especial hacia atrás en el eje Z mientras el jugador avanza.
/// Deja de moverse una vez queda suficientemente atrás del jugador.
/// </summary>
public class ObjEspecial : MonoBehaviour
{
    /// <summary>
    /// Velocidad a la que se mueve el objeto en la dirección negativa del eje Z.
    /// </summary>
    [SerializeField] private float velocidad;

    /// <summary>
    /// Indica si el objeto puede moverse actualmente.
    /// </summary>
    public bool PuedeMoverse { get; set; }

    /// <summary>
    /// Referencia al jugador para comparar posiciones y detener el movimiento.
    /// </summary>
    public PlayerController Player { get; set; }

    /// <summary>
    /// Mueve el objeto si está habilitado para hacerlo. Deja de moverse cuando está 40 unidades detrás del jugador.
    /// </summary>
    void Update()
    {
        if (PuedeMoverse)
        {
            transform.Translate(Vector3.forward * -velocidad * Time.deltaTime);

            if (transform.position.z + 40 < Player.transform.position.z)
            {
                PuedeMoverse = false;
            }
        }
    }
}
