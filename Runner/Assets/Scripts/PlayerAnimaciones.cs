using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controla las animaciones del personaje principal.
/// Permite cambiar entre animaciones específicas como correr, saltar o colisionar.
/// </summary>

public class PlayerAnimaciones : MonoBehaviour
{
    /// <summary>
    /// Referencia al componente Animator asignado al jugador.
    /// </summary>
    private Animator animator;

    /// <summary>
    /// Nombre de la animación que se está reproduciendo actualmente.
    /// </summary>
    private string animacionActual;

    /// <summary>
    /// Inicializa el componente Animator al iniciar la escena.
    /// </summary>
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Cambia la animación activa si es diferente a la actual.
    /// Evita reproducir la misma animación repetidamente.
    /// </summary>
    /// <param name="nuevaAnimacion">Nombre de la nueva animación a reproducir.</param>

    private void CambiarAnimacion(string nuevaAnimacion)
    {
        if (animacionActual == nuevaAnimacion)
        {
            return;
        }


        animator.Play(nuevaAnimacion);
        animacionActual = nuevaAnimacion;
    }

    /// <summary>
    /// Muestra la animación de estado inactivo (Idle).
    /// </summary>


    public void MostrarAnimacionIdle()
    {
        CambiarAnimacion("Idle");
    }
    /// <summary>
    /// Muestra la animación de correr (Run).
    /// </summary>
    public void MostrarAnimacionCorrer()
    {
        CambiarAnimacion("Run");
    }
    /// <summary>
    /// Muestra la animación de salto (Jump).
    /// </summary>
    public void MostrarAnimacionSaltar()
    {
        CambiarAnimacion("Jump");
    }
    /// <summary>
    /// Muestra la animación de deslizamiento o agachado (Crawl).
    /// </summary>

    public void MostrarAnimacionDeslizar()
    {
        CambiarAnimacion("Crawl");
    }

    /// <summary>
    /// Muestra la animación de colisión o muerte (Dead).
    /// </summary>

    public void MostrarAnimacionColision()
    {
        CambiarAnimacion("Dead");
    }

}
