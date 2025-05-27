using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Clase base genérica que implementa el patrón Singleton para componentes MonoBehaviour.
/// Asegura que exista una sola instancia de un tipo T en la escena.
/// </summary>
/// <typeparam name="T">Tipo de componente que debe ser único.</typeparam>
public class Singletton<T> : MonoBehaviour where T : Component
{
    /// <summary>
    /// Instancia estática del tipo T.
    /// </summary>
    private static T instancia;

    /// <summary>
    /// Propiedad pública para acceder a la instancia única del tipo T.
    /// Si no existe, intenta encontrarla o crearla automáticamente.
    /// </summary>
    public static T Instancia
    {
        get
        {
            if (instancia == null)
            {
                instancia = FindObjectOfType<T>();
                if (instancia == null)
                {
                    GameObject nuevoGO = new GameObject();
                    instancia = nuevoGO.AddComponent<T>();
                }
            }

            return instancia;
        }
    }

    /// <summary>
    /// Establece la instancia en el método Awake para asegurar el Singleton.
    /// Puede ser sobrescrito por clases derivadas.
    /// </summary>
    protected virtual void Awake()
    {
        instancia = this as T;
    }
}
