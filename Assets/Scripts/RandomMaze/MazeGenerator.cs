using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator
{
    //Arreglo bidimensional de celdas
    public Cell[,] cells;

    //Coordenada X y Y donde se encuentra el algoritmo
    public int selectedCoordinateX;
    public int selectedCoordinateY;

    //Arreglo de booleanos que indica los posibles 4 caminos a seguir y su validez: arriba, abajo, izquierda y derecha 
    bool[] validPath = new bool[4];

    public MazeGenerator() {
        //Inicialización del Laberinto
        InitializeMaze();
        //Aplicación del algoritmo Hunt and Kill
        HuntAndKill();
    }

    void InitializeMaze()
    {
        //Se inicia en una posición aleatoria dentro del Laberinto
        selectedCoordinateX = Random.Range(0, MazeManager.MZ.mazeRows);
        selectedCoordinateY = Random.Range(0, MazeManager.MZ.mazeColumns);
        //Se inicializa el arreglo con respecto a número de filas y número de columnas
        cells = new Cell[MazeManager.MZ.mazeRows, MazeManager.MZ.mazeColumns];
        for (int r = 0; r < MazeManager.MZ.mazeRows; r++)
        {
            for (int c = 0; c < MazeManager.MZ.mazeColumns; c++)
            {
                //Se instancia la celda, si no es primera columna su pared oeste no existe, si no es la primera fila su pared norte no existe
                //Eso con el fin de evitar que dos paredes de 2 celdas se interpongan entre sí mismas
                cells[r, c] = new Cell();
                if (c != 0)
                    cells[r, c].wExists = false;
                if (r != 0)
                    cells[r, c].nExists = false;
            }
        }
    }
    //Metodo que aplica el algortimo Hunt and Kill
    void HuntAndKill()
    {
        int nextStep = 0;
        //La celda en la que se encuentra el algoritmo se setea como visitada
        cells[selectedCoordinateX, selectedCoordinateY].visited = true;
        //Se llama el metodo que determina si son validos los 4 caminos a seguir
        AvalaiblePath(selectedCoordinateX, selectedCoordinateY);
        //Mientra que alguno de los caminos sea valido
        while (validPath[0] || validPath[1] || validPath[2] || validPath[3])
        {
            //Se escoge el siguiente paso de forma aleatoria dentro de los posibles caminos validos
            while (true)
            {
                nextStep = Random.Range(0, 4);
                if (validPath[nextStep])
                    break;
            }
            //De acuerdo con el siguiente paso se destruyen las paredes por las que paso
            switch (nextStep)
            {
                case 0:
                    cells[selectedCoordinateX, selectedCoordinateY].nExists = false;
                    selectedCoordinateX -= 1;
                    cells[selectedCoordinateX, selectedCoordinateY].sExists = false;
                    break;
                case 1:
                    cells[selectedCoordinateX, selectedCoordinateY].sExists = false;
                    selectedCoordinateX += 1;
                    cells[selectedCoordinateX, selectedCoordinateY].nExists = false;
                    break;
                case 2:
                    cells[selectedCoordinateX, selectedCoordinateY].wExists = false;
                    selectedCoordinateY -= 1;
                    cells[selectedCoordinateX, selectedCoordinateY].eExists = false;
                    break;
                case 3:
                    cells[selectedCoordinateX, selectedCoordinateY].eExists = false;
                    selectedCoordinateY += 1;
                    cells[selectedCoordinateX, selectedCoordinateY].wExists = false;
                    break;
            }
            cells[selectedCoordinateX, selectedCoordinateY].visited = true;
            //Se determinan los caminos validos considerando la nueva posicion en la que se encuentra
            AvalaiblePath(selectedCoordinateX, selectedCoordinateY);
        }
        //Si se puede cazar una nueva posicion se llama de forma recrusiva el metodo Hunt and Kill
        if (HuntNewPosition())
            HuntAndKill();
        return;
    }

    //Metodo que establece la validez de los psoibles 4 caminos, al analizar si ya han sido visitados
    void AvalaiblePath(int x, int y)
    {
        validPath[0] = false;
        validPath[1] = false;
        validPath[2] = false;
        validPath[3] = false;
        if (x - 1 > 0)
        {
            if (!cells[x - 1, y].visited)
                validPath[0] = true;
        }
        if (x + 1 < MazeManager.MZ.mazeRows)
        {
            if (!cells[x + 1, y].visited)
                validPath[1] = true;
        }
        if (y - 1 > 0)
        {
            if (!cells[x, y - 1].visited)
                validPath[2] = true;
        }
        if (y + 1 < MazeManager.MZ.mazeColumns)
        {
            if (!cells[x, y + 1].visited)
                validPath[3] = true;
        }
    }

    //Metodo que caza una nueva posicion en la que se inicia el algoritmo Hunt and Kill
    //Pasa cuando el algoritmo se estanca en un punto donde ya no hay mas caminos validos que seguir
    //Si regresa falso significa que ya no hay mas caminos nuevos que cazar por lo que el algoritmo termino
    bool HuntNewPosition()
    {
        bool foundNewPosition = false;
        for (int r = 0; r < MazeManager.MZ.mazeRows && !foundNewPosition; r++)
        {
            for (int c = 0; c < MazeManager.MZ.mazeColumns && !foundNewPosition; c++)
            {
                if (!cells[r, c].visited)
                {
                    if (r > 0 && cells[r - 1, c].visited)
                    {
                        selectedCoordinateX = r;
                        selectedCoordinateY = c;
                        cells[selectedCoordinateX, selectedCoordinateY].nExists = false;
                        cells[selectedCoordinateX - 1, selectedCoordinateY].sExists = false;
                        foundNewPosition = true;
                    }
                    else if (r < MazeManager.MZ.mazeRows - 1 && cells[r + 1, c].visited)
                    {
                        selectedCoordinateX = r;
                        selectedCoordinateY = c;
                        cells[selectedCoordinateX, selectedCoordinateY].sExists = false;
                        cells[selectedCoordinateX + 1, selectedCoordinateY].nExists = false;
                        foundNewPosition = true;
                    }
                    else if (c > 0 && cells[r, c - 1].visited)
                    {
                        selectedCoordinateX = r;
                        selectedCoordinateY = c;
                        cells[selectedCoordinateX, selectedCoordinateY].wExists = false;
                        cells[selectedCoordinateX, selectedCoordinateY - 1].eExists = false;
                        foundNewPosition = true;
                    }
                    else if (c < MazeManager.MZ.mazeColumns - 1 && cells[r, c + 1].visited)
                    {
                        selectedCoordinateX = r;
                        selectedCoordinateY = c;
                        cells[selectedCoordinateX, selectedCoordinateY].eExists = false;
                        cells[selectedCoordinateX, selectedCoordinateY + 1].wExists = false;
                        foundNewPosition = true;
                    }
                }
            }
        }
        return foundNewPosition;
    }
}
