using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    [Header("Textos")]
    [SerializeField] private TextMeshProUGUI textDiamantesObtenidos;

    private void Update()
    {
        textDiamantesObtenidos.text = GameManager.Instancia.DiamantesObtenidosEnEsteNivel.ToString();
    }
}
