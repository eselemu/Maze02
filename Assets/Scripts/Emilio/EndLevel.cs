using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class EndLevel : MonoBehaviour
{
    public Timer timer;
    public ProgressBar progressBar;
    public GameData gameData;
    public GameObject background; // referencia la pantalla de final del nivel
    public TextMeshProUGUI estrellas;
    public TextMeshProUGUI alcancia;
    public void detenerNivel()//cuando es llamada se detiene tantoe el timer como la barra de progreso y el gameplay
    {
        timer.setFinish(true);
        progressBar.setFinish(true);
        gameData.ContarEstrellas();//Conteo de las estrellas del nivel
        gameData.aumentarAlcancia(gameData.getNEstrellas());//Aumentamos las estrellas correspondientes 
        pantallaFinNivel();
    }

    public void pantallaFinNivel()// ufncion para llamar la pantalla de que el nivel ya acabos
    {
        estrellas.text = "Estrellas: " + gameData.getNEstrellas();//Asigna el numero de estrellas a la text box coordinada con estrellas
        alcancia.text = "EstrellasEnAlcancia: " + PlayerPrefs.GetInt("Alcancia_estrellas");//Establece el numero de estrellas como el texto asignado desde unity
        background.SetActive(true);//El default de mi background es false, al ponerlo true se sobrepone al juego.
        Debug.Log(PlayerPrefs.GetInt("Alcancia_estrellas"));
    }
}
