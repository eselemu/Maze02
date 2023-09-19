using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Clase ingrediente
public class Ingredient{

    public int coordinateX, coordinateY;//Coordenada X y Y del ingrediente
    public GameObject gameObjectIngredients, panelIngredient;//Prefabs del objeto Ingrediente y su respectivo panel
    public bool collected;//Booleano que indica si ha sido recolectado o no

    public Ingredient() {
        collected = false;
        SetRandomPosition();
    }
    //Método que setea la coordenada X y Y del ingrediente en una posición libre y aleatoria
    void SetRandomPosition()
    {
        while (true)
        {
            coordinateX = Random.Range(0, MazeManager.MZ.mazeRows);
            coordinateY = Random.Range(0, MazeManager.MZ.mazeColumns);
            if (MazeManager.MZ.freeCells[coordinateX, coordinateY])
            {
                MazeManager.MZ.freeCells[coordinateX, coordinateY] = false;
                break;
            }
        }
    }
}
