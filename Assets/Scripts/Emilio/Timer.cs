using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI TimerText;
    public EndLevel endLevel;
    private float startTime;//variable para establecer el tiempo por partida
    private bool finish;//Booleano para detener el juego
    private float t; //t es la variable en la que se guarda la cuenta regresiva(es la variable impresa en el UI)

    public void setFinish(bool finish)//Funcion para modificar el valor de finish y detener el conteo
    {
        this.finish = finish;
    }
    void Start()
    {
        startTime = Time.time + 45.0f;//Establece el tiempo  de la partida en 45 segundos (no se imprime en pantalla)
        finish = false;
    }

    void Update()
    {
        if (finish == false) //comparador para detener o continuar con el timer
        {
            t = startTime - Time.time;
            //Time.time hace un conteo del tiempo transcurrido desde que comenzo el juego (segundos)
            string seconds = t.ToString("f0");/*(t % 60).ToString("f2");*/
            //convierte a string t para poderla imprimir en el UI

            TimerText.text = seconds;//Imprime en pantalla
            if (t <= 0)//condicion para llamar fin del juego
                endLevel.detenerNivel();
        }
            
    }

    private void OnTriggerEnter(Collider other)//Detector de power ups de tiempo
    {
        if (other.gameObject.tag.Equals("tiempo+") == true)
        {
            t += 5;
        }
    }

    public void IncreaseTimer(int increaseBy){
        startTime += increaseBy;
        if(startTime > 45 + Time.time){
            startTime = 45 + Time.time;
        }
    }
}
