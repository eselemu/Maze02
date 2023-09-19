using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Slider slider;
    public EndLevel endLevel;
    public float sliderIncrement = 0f;//magnitud de aumento en la barra 0...1
    public float FillSpeed = 0.01f;//Pruebas con tiempo
    public int totalIngredientes;
    private bool finish = false;
    public void setFinish(bool finish)
    {
        this.finish = finish;
    }

    void Start()
    {
        sliderIncrement = 0;
        /*sliderIncrement = 1 / totalIngredientes;*/  //Para que aumente proporcionalmente de acuerdo al numero de ingredientes
    }
    void Update()
    {
        if (finish == false)//Detiene la barra cuando el tiempo acaba
            IncrementProgressBar();//El aumento en el relleno de la barra de progreso
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("ingrediente") == true)//Detector de ingredientes
        {
            slider.value = (float)(slider.value + sliderIncrement);//slider.value es la varible por default de unity indicante del progreso de la barra
        }
    }
    public void IncrementProgressBar()
    {
        slider.value = (float)(slider.value + sliderIncrement * Time.deltaTime);//se encarga de que la barra aumente
        if (slider.value >= 1)
            endLevel.detenerNivel();
    }

}
