using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultadosFinales : MonoBehaviour
{
    [Header("Textos por nivel")]
    [SerializeField] private TextMeshProUGUI[] textosDiamantes;
    [SerializeField] private TextMeshProUGUI[] textosPuntajes;

    [Header("Totales")]
    [SerializeField] private TextMeshProUGUI textoDiamantesTotales;
    [SerializeField] private TextMeshProUGUI textoPuntajeTotal;

    private void Start()
    {
        var resultados = GameManager.Instancia.resultadosPorNivel;

        for (int i = 0; i < resultados.Count && i < 4; i++)
        {
            textosDiamantes[i].text = "Diamantes: " + resultados[i].diamantes;
            textosPuntajes[i].text = "Puntaje: " + resultados[i].puntaje;
        }

        textoDiamantesTotales.text = "Diamantes Totales: " + GameManager.Instancia.DiamantesTotales;
        textoPuntajeTotal.text = "Puntaje Total: " + GameManager.Instancia.PuntajeTotal;
    }
}
