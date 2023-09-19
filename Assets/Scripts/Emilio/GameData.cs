using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class GameData : MonoBehaviour
{
    /*Tareas:
 * Crear una funcion iniciarNivel
    * Asegurarse que el estatus inicial de Background sea false. 
    * Iniciar el nivel en base al JSON de niveles que se me va a pasar
 * Aplicar excepciones
 * Cambiar las estrellas de texto a sprite
 * Guardar el GameData en un archivo JSON para guardar el progreso
 * A�adir funcionalidades a los botenes de Background
 * Crear un menu simple
 * Coordinarme con Hector para hacer pruebas con movimiento
 */
    /* Actualizaciones (segundo avance):
     *Cree e implemente un bakcground que se llama al finalizar el nivel
        *El numero de estrellas es actualizable
        *Botones para continuar al siguiente nivel o ir al menu
     *Cree las funciones ContarEstrellas(), aumentarCochinito(), detenerNivel(), pantallaFinNivel();
     *getters y settes de algunas variables que son actualizadas
     *Movi la mayoria de las funciones a un archivo de Funciones (reversible de ser necesario)
     */
    public ProgressBar progressBar;//referencia al script de la barra de progreso
    private int nEstrellas;//Estrellas del nivel
    private int estrellasInicio;//Estrellas en la alcancia al inicio del nivel
    private int estrellasFinal;//Estrellas en la alcancia al final del nivel
    private int estrellasRecogidas;//Estrellas recogidas durante el nivel

    void Start(){
        estrellasInicio = PlayerPrefs.GetInt("Alcancia_estrellas");
    }

    public void ContarEstrellas()// Calcula las estrellas conseguidas en el nivel, ubicando el progreso de la barra.
    {
        if (progressBar.slider.value == 0)
        {
            nEstrellas = 0;
        }
        else if (progressBar.slider.value < 0.33)
        {
            nEstrellas = 1;
        }
        else if (progressBar.slider.value >= 0.33 && progressBar.slider.value < 0.99)
        {
            nEstrellas = 2;
        }
        else if (progressBar.slider.value >= 0.99)
        {
            nEstrellas = 3;
        }
    }
    
    public void aumentarAlcancia(int nEstrellas)//Aumenta nuestra variable que se va almacenar en el dispositivo
    {
        estrellasFinal = PlayerPrefs.GetInt("Alcancia_estrellas");//Actualiza el valor de estrellas al �ltimo guardado
        estrellasRecogidas = estrellasFinal - estrellasInicio;
        Debug.Log("Estrellas Recogidas: " +  estrellasRecogidas);
        PlayerPrefs.SetInt("Alcancia_estrellas", estrellasFinal + nEstrellas);//Aumenta las estrellas del ultimo nivel a nuestra alcancia
    }
    
    public int getNEstrellas()
    {
        return nEstrellas + estrellasRecogidas;
    }
}
