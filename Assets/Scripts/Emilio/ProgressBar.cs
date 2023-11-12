using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public static ProgressBar PB;
    public Slider slider;
    public EndLevel endLevel;
    public float sliderIncrement = 0f;//magnitud de aumento en la barra 0...1
    public float FillSpeed = 0.01f;//Pruebas con tiempo
    public int totalIngredientes;
    private bool finish = false;

    void Awake()
    {
        if (PB != null)
            GameObject.Destroy(PB);
        else
            PB = this;
    }
    public void setFinish(bool finish)
    {
        this.finish = finish;
    }

    void Start()
    {
    }
    void Update()
    {
    }
    public void IncrementProgressBar()
    {
        slider.value += sliderIncrement;
        if (slider.value >= 0.9f)
            endLevel.detenerNivel();
    }

}
