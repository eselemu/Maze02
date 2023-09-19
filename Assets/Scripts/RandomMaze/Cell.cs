using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Clase celda
public class Cell{
    //Booleanos. Si la celda ha sido visitada. Si existen sus 4 paredes(Norte, Sur, Este, Oeste)
    public bool visited, nExists, sExists, eExists, wExists;
    //GameObject de cada una de sus 4 paredes (Norte, Sur, Este, Oeste)
    public GameObject nWall, sWall, eWall, wWall;

    public Cell() {
        //Se inicializa la celda como no visitada y con sus 4 paredes
        visited = false;
        nExists = true;
        sExists = true;
        eExists = true;
        wExists = true;
    }
}
