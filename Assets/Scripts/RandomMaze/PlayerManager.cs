using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
//Clase jugador
public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private GameObject controller;
    public static PlayerManager PM;
    public float moveSpeed;//Velocidad bajo la que se mueve el personaje
    public bool move, breakWalls;//Booleano que determina si pude moverse o romper paredes
    public Vector3 nextPos;//El vector que determina la siguiente posicion del usuario
    public int coordinateX, coordinateY;//Coordenadas X y Y del usuario

    void Awake()
    {
        if (PM != null)
            GameObject.Destroy(PM);
        else
            PM = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        move = false;
        breakWalls = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //Inicializacion de la posicion del personaje en una celda aleatoria y libre dentro del laberinto
    public void SetRandomPosition()
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
        ChangePosition();
    }

    //Metodo que cambia la posicion del usuario, adecuando las coordenadas X y Y con la posicion real de esas coordenadas
    public void ChangePosition() {
        transform.position =
                new Vector3(((-MazeManager.MZ.mazeRows / 2) + coordinateY) * MazeManager.MZ.wallSize, ((transform.localScale.y / 2) + 0.55f),
                ((MazeManager.MZ.mazeColumns / 2) - coordinateX) * MazeManager.MZ.wallSize);
    }

    //Metodo que se llama cuando el usuario colisiona con un objeto
    public void OnTriggerEnter(Collider collision){
        //Si el usuario colisiono con una pared
        if (collision.CompareTag("Wall"))
            WallCollision(collision);
        //Si el usuario colisiono con un Ingrediente, se colecta el respectivo ingrediente y su respectivo panel cambia de igual forma
        else if (collision.CompareTag("Ingredient"))
        {
            int index = int.Parse(collision.gameObject.name);
            //MazeManager.MZ.ingredients[index].panelIngredient.GetComponent<Image>().color = Color.red;
            MazeManager.MZ.ingredients[index].gameObjectIngredients.SetActive(false);
            MazeManager.MZ.ingredients[index].collected = true;

            // Load the image for the ingredient panel.
            Image image = MazeManager.MZ.ingredients[index].panelIngredient.GetComponent<Image>();
            //texture = Resources.Load<Texture2D>("Images/Ingredients/Collected/Aguacate.png");
            string path = "Assets/Images/Ingredients/Collected/" + MazeManager.MZ.dish.ingredients[index] + ".png";
            Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);

            if (texture != null)
            {
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
                image.sprite = sprite;
            }
        }
        //Si el usuario colisiono con un super poder
        else if (collision.CompareTag("SuperPower"))
        {
            //Dependiendo del super poder sera la accion a tomar
            collision.gameObject.SetActive(false);
            switch (collision.gameObject.name)
            {
                //5 - 10 segundos
                case "Rayo":
                    moveSpeed += 2f;
                    Invoke("StopRayo", 7.0f);
                    break;
                case "Reloj":
                    controller.GetComponent<Timer>().IncreaseTimer(5);
                    break;
                case "X":
                    CollectNextIngredient();
                    break;
                    //5-10 segundos
                case "Borrador":
                    breakWalls = true;
                    Invoke("StopBorrador", 7.0f);
                    break;
                case "EstrellaDorada":
                    Debug.Log("Star Pick-Up!");
                    int totalEstrellas = PlayerPrefs.GetInt("Alcancia_estrellas");
                    PlayerPrefs.SetInt("Alcancia_estrellas", totalEstrellas + 10);//Aumenta las estrellas del ultimo nivel a nuestra alcancia
                    break;
            }
        }
    }
    //Metodo que colecta el siguiente ingrediente sin colectar
    void CollectNextIngredient() {
        for (int a = 0; a < MazeManager.MZ.quantityIngredients; a++) {
            if (!MazeManager.MZ.ingredients[a].collected) {
                //MazeManager.MZ.ingredients[a].panelIngredient.GetComponent<Image>().color = Color.red;
                MazeManager.MZ.ingredients[a].gameObjectIngredients.SetActive(false);
                MazeManager.MZ.ingredients[a].collected = true;

                Image image = MazeManager.MZ.ingredients[a].panelIngredient.GetComponent<Image>();
                //texture = Resources.Load<Texture2D>("Images/Ingredients/Collected/Aguacate.png");
                string path = "Assets/Images/Ingredients/Collected/" + MazeManager.MZ.dish.ingredients[a] + ".png";
                Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(path);

                if (texture != null)
                {
                    Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
                    image.sprite = sprite;
                }
                break;
            }
        }
    }
    //Metodo que destruye la pared colisionada, si tiene el super poder respectivo activado
    //De lo contrario para su movimiento y lo hace retroceder.
    void WallCollision(Collider collision) {
        if (breakWalls)
        {
            bool destroy = true;
            float stopCoordinate = (MazeManager.MZ.mazeRows / 2) * MazeManager.MZ.wallSize, offset = (MazeManager.MZ.wallSize / 2);
            if (transform.position.z >= stopCoordinate + offset - 1)
            {
                if (collision.name.Contains("NWall"))
                    destroy = false;
            }
            else if (transform.position.z <= (-stopCoordinate) + offset + 1)
            {
                if (collision.name.Contains("SWall"))
                    destroy = false;
            }

            if (transform.position.x <= -stopCoordinate - offset + 1)
            {
                if (collision.name.Contains("WWall"))
                    destroy = false;
            }
            else if (transform.position.x >= stopCoordinate - offset - 1)
            {
                if (collision.name.Contains("EWall"))
                    destroy = false;
            }

            if (destroy)
                collision.gameObject.SetActive(false);
            else {
                move = false;
                transform.position -= (nextPos * 0.2f);
            }
        }
        else {
            move = false;
            transform.position -= (nextPos * 0.2f);
        }
    }

    void StopRayo() {
        moveSpeed -= 2f;
    }

    void StopBorrador(){
        breakWalls = false;
    }
}
