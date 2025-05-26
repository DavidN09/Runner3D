using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singletton<T> : MonoBehaviour where T: Component
{
    private static T instancia;
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

    protected virtual void Awake()
    {
        instancia = this as T;
    }
}
