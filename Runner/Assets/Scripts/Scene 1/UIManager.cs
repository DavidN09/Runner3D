using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    [Header("Textos")]
    [SerializeField] private TextMeshProUGUI textDiamantesObtenidos;
    [SerializeField] private TextMeshProUGUI textPuntaje;

    private void Update()
    {
        textDiamantesObtenidos.text = GameManager.Instancia.DiamantesObtenidosEnEsteNivel.ToString();
        textPuntaje.text = GameManager.Instancia.Puntaje.ToString();
    }
}
