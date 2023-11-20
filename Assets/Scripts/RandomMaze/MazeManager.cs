using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEditor;

public class MazeManager : MonoBehaviour
{
    public static MazeManager MZ;
    public int mazeRows;//Cantidad de Filas en el Laberinto
    public int mazeColumns;//Cantidad de Columnas en el Laberinto
    public float wallSize;//Tama�o de pared

    //Prefabs para instanciar durante la ejecuci�n del Juego
    public GameObject wallPrefab;
    public GameObject rayoPrefab;
    public GameObject relojPrefab;
    public GameObject xPrefab;
    public GameObject borradorPrefab;
    public GameObject estrellaPrefab;
    public GameObject ingredientPrefab;
    public GameObject panelPrefab;
    public GameObject floorPrefab;

    MazeGenerator maze;//Objeto Maze, con el Laberinto ya generado


    public TextAsset jsonFile;
    List<int> ownedIndexes = new List<int>();
    public Dish dish;
    public Texture2D texture;

    int quantityPowers;//Cantidad de SuperPoderes
    public Power[] powers;//Arreglo de SuperPoderes

    public int quantityIngredients;//Cantidad de Ingredientes
    public Ingredient[] ingredients;//Arreglo de Ingredientes


    public bool[,] freeCells;//Celdas Libres

    //Enumeracion de posibles SuperPoderes
    public enum TypePower {
        Rayo,
        Reloj,
        X,
        Borrador,
        EstrellaDorada
    }

    void Awake()
    {
        if (MZ != null)
            GameObject.Destroy(MZ);
        else
            MZ = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        getDish();

        //Inicializaci�n de arreglos y variables
        quantityPowers = 2;
        quantityIngredients = dish.ingredients.Length;
        freeCells = new bool[mazeRows, mazeColumns];
        powers = new Power[quantityPowers];
        ingredients = new Ingredient[quantityIngredients];
        ProgressBar.PB.sliderIncrement = (float)(1.0f / quantityIngredients);

        //Se instancia el objeto maze, generando el Laberinto
        maze = new MazeGenerator();

        InitializeFreeCells();
        InstantiatePowers();
        InstantiateIngredients();

        //Renderizaci�n de la escena
        PlayerManager.PM.SetRandomPosition();
        RenderMaze();
        RenderPowers();
        RenderIngredients();
        RenderIngredientsPanels();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void getDish() {
        ShopListingSchema jsonDishes = JsonUtility.FromJson<ShopListingSchema>(jsonFile.text);

        /*foreach (Dish d in jsonDishes.inventory) {
            ownedIndexes.Add(d.type - 1);
        }
        dish = jsonDishes.inventory[ownedIndexes[Random.Range(0, ownedIndexes.Count)]];*/
        dish = jsonDishes.inventory[GameMaster.GM.selectedDish];
    }

    void RenderMaze() {
        //Renderizaci�n del suelo del Laberinto
        GameObject floor = Instantiate(floorPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        floor.transform.localScale = new Vector3(mazeRows, 1, mazeColumns);

        for (int r = 0; r < mazeRows; r++)
        {
            for (int c = 0; c < mazeColumns; c++)
            {
                //Posici�n de la celda con respecto a la fila y la columna
                Vector3 position = new Vector3(((-mazeRows / 2) + c) * wallSize, 0, ((mazeColumns / 2) - r) * wallSize);

                //West - Left
                //Renderizaci�n de la pared oeste si es que existe
                if (maze.cells[r, c].wExists)
                {
                    maze.cells[r, c].wWall = Instantiate(wallPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                    maze.cells[r, c].wWall.transform.position = 
                        position + new Vector3(-wallSize / 2, maze.cells[r, c].wWall.transform.localScale.y / 2, 0);
                    maze.cells[r, c].wWall.transform.eulerAngles = new Vector3(0, 90, 0);
                    maze.cells[r, c].wWall.transform.localScale = new Vector3(wallSize, maze.cells[r, c].wWall.transform.localScale.y, maze.cells[r, c].wWall.transform.localScale.z);
                    maze.cells[r, c].wWall.name = "WWall " + r + ", " + c;
                }


                //East - right
                //Renderizaci�n de la pared este si es que existe
                if (maze.cells[r, c].eExists)
                {
                    maze.cells[r, c].eWall = Instantiate(wallPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                    maze.cells[r, c].eWall.transform.position = 
                        position + new Vector3(wallSize / 2, maze.cells[r, c].eWall.transform.localScale.y / 2, 0);
                    maze.cells[r, c].eWall.transform.eulerAngles = new Vector3(0, 90, 0);
                    maze.cells[r, c].eWall.transform.localScale = new Vector3(wallSize, maze.cells[r, c].eWall.transform.localScale.y, maze.cells[r, c].eWall.transform.localScale.z);
                    maze.cells[r, c].eWall.name = "EWall " + r + ", " + c;
                }

                //North - up
                //Renderizaci�n de la pared norte si es que existe
                if (maze.cells[r, c].nExists)
                {
                    maze.cells[r, c].nWall = Instantiate(wallPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                    maze.cells[r, c].nWall.transform.position = 
                        position + new Vector3(0, maze.cells[r, c].nWall.transform.localScale.y / 2, wallSize / 2);
                    maze.cells[r, c].nWall.transform.localScale = new Vector3(wallSize, maze.cells[r, c].nWall.transform.localScale.y, maze.cells[r, c].nWall.transform.localScale.z);
                    maze.cells[r, c].nWall.name = "NWall " + r + ", " + c;
                }

                //South - bottom
                //Renderizaci�n de la pared sur si es que existe
                if (maze.cells[r, c].sExists)
                {
                    maze.cells[r, c].sWall = Instantiate(wallPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                    maze.cells[r, c].sWall.transform.position = 
                        position + new Vector3(0, maze.cells[r, c].sWall.transform.localScale.y / 2, -wallSize / 2);
                    maze.cells[r, c].sWall.transform.localScale = new Vector3(wallSize, maze.cells[r, c].sWall.transform.localScale.y, maze.cells[r, c].sWall.transform.localScale.z);
                    maze.cells[r, c].sWall.name = "SWall " + r + ", " + c;
                }
            }
        }
    }

    void RenderPowers() {
        //Renderizaci�n de cada SuperPoder en el arreglo
        for (int a = 0; a < quantityPowers; a++) {
            switch (powers[a].power)
            {
                case TypePower.Rayo:
                    powers[a].gameObjectPower = Instantiate(rayoPrefab, rayoPrefab.transform.position, Quaternion.identity);
                    break;
                case TypePower.Reloj:
                    powers[a].gameObjectPower = Instantiate(relojPrefab, relojPrefab.transform.position, Quaternion.identity);
                    break;
                case TypePower.X:
                    powers[a].gameObjectPower = Instantiate(xPrefab, xPrefab.transform.position, Quaternion.identity);
                    break;
                case TypePower.Borrador:
                    powers[a].gameObjectPower = Instantiate(borradorPrefab, borradorPrefab.transform.position, Quaternion.identity);
                    powers[a].gameObjectPower.transform.eulerAngles += new Vector3(0, 90, 90);
                    break;
                case TypePower.EstrellaDorada:
                    powers[a].gameObjectPower = Instantiate(estrellaPrefab, estrellaPrefab.transform.position, Quaternion.identity);
                    break;
            }
            powers[a].gameObjectPower.transform.position = new Vector3(((-mazeRows / 2) + powers[a].coordinateY) * wallSize, powers[a].gameObjectPower.transform.position.y, ((mazeColumns / 2) - powers[a].coordinateX) * wallSize);

            powers[a].gameObjectPower.name = "" + powers[a].power;
        }
    }

    void RenderIngredients() {
        //Renderizaci�n de cada Ingrediente en el arreglo
        for (int a = 0; a < quantityIngredients; a++)
        {
            ingredients[a].gameObjectIngredients = Instantiate(ingredientPrefab, new Vector3(0, -0.25f, 0), Quaternion.identity);
            ingredients[a].gameObjectIngredients.transform.position = 
                new Vector3(((-mazeRows / 2) + ingredients[a].coordinateY) * wallSize, ingredients[a].gameObjectIngredients.transform.localScale.y / 2, ((mazeColumns / 2) - ingredients[a].coordinateX) * wallSize);
            ingredients[a].gameObjectIngredients.name = "" + a;
        }
    }

    void InitializeFreeCells() {
        //Inicializaci�n de todas las celdas a libres
        for (int rows = 0; rows < mazeRows; rows++) {
            for (int columns = 0; columns < mazeColumns; columns++)
                freeCells[rows, columns] = true;
        }
    }

    //Instancia de los SuperPoderes
    void InstantiatePowers() {
        for (int a = 0; a < quantityPowers; a++)
            powers[a] = new Power();
    }

    //Instancia de los SuperPoderes
    void InstantiateIngredients() {
        for (int a = 0; a < quantityIngredients; a++) {
            ingredients[a] = new Ingredient();
        }
    }

    //Renderizaci�n de los Paneles por Ingrediente
    void RenderIngredientsPanels() {
        RectTransform rectTransform = panelPrefab.GetComponent<RectTransform>();
        float panelWidth = rectTransform.rect.width;
        for (int a = 0; a < quantityIngredients; a++) {
            ingredients[a].panelIngredient = Instantiate(panelPrefab, new Vector3(-50, 50, 0), Quaternion.identity);
            ingredients[a].panelIngredient.transform.SetParent(GameObject.Find("IngredientPanelCanvas").transform, false);
            ingredients[a].panelIngredient.transform.localPosition += new Vector3(0, ((panelWidth / 2) * -a), 0);

            // Load the image for the ingredient panel.
            Image image = ingredients[a].panelIngredient.GetComponent<Image>();
            //texture = Resources.Load<Texture2D>("Images/Ingredients/Collected/Aguacate.png");
            string path = "Assets/Images/Ingredients/NotCollected/" + dish.ingredients[a] + " NR.png";
            texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);

            if (texture != null)
            {
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
                image.sprite = sprite;
            }
        }
    }
}
