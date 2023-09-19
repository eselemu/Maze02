using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Clase super poder
public class Power{
    public MazeManager.TypePower power;//Tipo de SuperPoder que es el objeto
    public int coordinateX, coordinateY;//Coordenada X y Y del SuperPoder
    public GameObject gameObjectPower;//Prefab del objeto SuperPoder

    public Power()
    {
        SetRandomPower();
        SetRandomPosition();
    }
    //Escoge un SuperoPoder de forma Aleatoria, siendo la Estrella Dorada la de menor prioridad
    void SetRandomPower() {
        int selectedPower = Random.Range(0, 22);

        if (selectedPower == 21) {
            power = MazeManager.TypePower.EstrellaDorada;
            return;
        }

        switch(selectedPower % 4){
            case 0:
                power = MazeManager.TypePower.Borrador;
                break;
            case 1:
                power = MazeManager.TypePower.Rayo;
                break;
            case 2:
                power = MazeManager.TypePower.Reloj;
                break;
            case 3:
                power = MazeManager.TypePower.X;
                break;
            default:
                power = MazeManager.TypePower.Reloj;
                break;
        }
        //power = MazeManager.TypePower.Borrador;
    }
    //Método que setea la coordenada X y Y del SuperPoder en una posición libre y aleatoria
    void SetRandomPosition() {
        while (true) {
            coordinateX = Random.Range(0, MazeManager.MZ.mazeRows);
            coordinateY = Random.Range(0, MazeManager.MZ.mazeColumns);
            if (MazeManager.MZ.freeCells[coordinateX, coordinateY]) {
                MazeManager.MZ.freeCells[coordinateX, coordinateY] = false;
                break;
            }
        }
    }
}
