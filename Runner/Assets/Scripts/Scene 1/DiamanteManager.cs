using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamanteManager : Singletton<DiamanteManager>
{
    public int DiamantesTotales { get; private set; }
    private string DIAMANTES_KEY = "MIS_DIAMANTES";

    protected override void Awake()
    {
        base.Awake();
        DiamantesTotales = PlayerPrefs.GetInt(DIAMANTES_KEY);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            AñadirDiamantes(1);
        }
    }
    public void AñadirDiamantes(int cantidad)
    {
        DiamantesTotales += cantidad;
        PlayerPrefs.SetInt(DIAMANTES_KEY, DiamantesTotales);
        PlayerPrefs.Save();
    }

    public void GastarDiamantes(int cantidad)
    {
        if (DiamantesTotales >= cantidad)
        {
            DiamantesTotales -= cantidad;
            PlayerPrefs.SetInt(DIAMANTES_KEY, DiamantesTotales);
            PlayerPrefs.Save();
        }
    }
}
