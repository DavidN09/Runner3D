using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAnimacion : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private bool activarRotacion;
    [SerializeField] private bool activarScale;

    [Header("Rotación")]
    [SerializeField] private Vector3 anguloRotacion;
    [SerializeField] private float velocidadRotacion;

    [Header("Scale")]
    [SerializeField] private Vector3 scaleInicial;
    [SerializeField] private Vector3 scaleFinal;
    [SerializeField] private float velocidadScale;
    [SerializeField] private float scaleRatio;

    private float tiempoScale;
    private bool scaleSuperior;

    private void Update()
    {
        if (activarRotacion)
        {
            transform.Rotate(anguloRotacion * velocidadRotacion * Time.deltaTime);
        }
        if (activarScale)
        {
            tiempoScale += Time.deltaTime;
            if (scaleSuperior)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, scaleFinal, velocidadScale * Time.deltaTime); 
            }
            else
            {
                transform.localScale = Vector3.Lerp(transform.localScale, scaleInicial, velocidadScale * Time.deltaTime);
            }

            if (tiempoScale >= scaleRatio)
            {
                scaleSuperior = !scaleSuperior;
                tiempoScale = 0f;
            }
        }
    }
}
