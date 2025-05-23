using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaculoAnimado : MonoBehaviour
{
    public Animator animator;
    public string nombreAnimacion;  // Nombre del clip de animación
    public float intervalo = 2f;

    private float tiempoSiguiente;

    void Start()
    {
        tiempoSiguiente = Time.time + intervalo;
    }

    void Update()
    {
        if (Time.time >= tiempoSiguiente)
        {
            animator.Play(nombreAnimacion, 0, 0f); // Reproduce desde el inicio
            tiempoSiguiente = Time.time + intervalo;
        }
    }
}
