using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class DatosNivel : MonoBehaviour
{
    public int numeroNivel;
    public int diamantes;
    public int puntaje;

    public DatosNivel(int numeroNivel, int diamantes, int puntaje)
    {
        this.numeroNivel = numeroNivel;
        this.diamantes = diamantes;
        this.puntaje = puntaje;
    }
}
