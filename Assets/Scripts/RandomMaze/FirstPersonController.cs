using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

//Clase que controla el movimiento con respecto a la entrada del ususario
public class FirstPersonController : MonoBehaviour
{
    //Vector3 que almacena la primera y ultima posicion de la entrada touch del usuario
    Vector3 firstPosition;
    Vector3 lastPosition;
    //Flotante que determina el minimo arrastre para que la entrada sea valida
    float minimumDrag;
    //Entero que guarda los grados para la rotacion del personaje. Desired son los grados que debe rotar, current son los grados
    //actuales del personaje, sign determina el signo del giro.
    int desiredDegrees, currentDegrees, signDegrees;
    //Flotante que guarda la velocidad en la que el personaje rota
    float speedRotation;
    //Booleano que almacena si el personaje se encuentra rotando
    bool rotating;

    void Start()
    {
        //Al iniciar el obejto se inicializan los valores base de las vars
        currentDegrees = 0;
        signDegrees = 1;
        speedRotation = 0.005f;
        rotating = false;
        //Se establece el arrastre mimo del usuario como el 15% de la altura de la pantalla
        minimumDrag = Screen.height * (15 / 100);
        //El personaje inicia por default moviendose hacia al frente
        PlayerManager.PM.nextPos = Vector3.forward;
    }

    void Update()
    {
        DirectionSlideInput();
        //If player is able to move, increase it's position considering it's nextPosition and base move speed
        if(PlayerManager.PM.move)
            transform.position += (PlayerManager.PM.nextPos * (PlayerManager.PM.moveSpeed) * Time.deltaTime);
    }
    //Function that determines the slide direction of the user and change the player's movement according the direction
    private void DirectionSlideInput()
    {
        //Si existe un toque en la pantalla
        if (Input.touchCount == 1)
        {
            Touch userTouch = Input.GetTouch(0);
            //Si es la fase inicial del toque, la primera y ultima posicion son los actaules toques del usuario
            if (userTouch.phase == TouchPhase.Began)
            {
                firstPosition = userTouch.position;
                lastPosition = userTouch.position;
            }
            //Si es la fase intermedia del toque, la ultima posicion son los actuales toques del usuario
            else if (userTouch.phase == TouchPhase.Moved)
                lastPosition = userTouch.position;
            //Si es la fase final del toque, la ultima posicion es el actaul toque del usuario
            else if (userTouch.phase == TouchPhase.Ended)
            {
                lastPosition = userTouch.position;
                //Si el arrastre es mayor al minimo determinado
                if (Math.Abs(lastPosition.x - firstPosition.x) > minimumDrag || Math.Abs(lastPosition.y - firstPosition.y) > minimumDrag)
                {
                    //Si el obejto no se encuentra rotando
                    if (!rotating) {
                        Vector3 aux = PlayerManager.PM.nextPos;
                        PlayerManager.PM.move = false;
                        //Si el arrastre es horizontal
                        if (Math.Abs(lastPosition.x - firstPosition.x) > Math.Abs(lastPosition.y - firstPosition.y))
                        {
                            //Right slide
                            if (lastPosition.x > firstPosition.x)
                            {
                                desiredDegrees = 90;
                                signDegrees = 1;
                                InvokeRepeating("RotatePlayer", 0, speedRotation);
                                PlayerManager.PM.nextPos = new Vector3(((int)aux.x ^ 1) * aux.z, 0, ((int)aux.z ^ 1) * -aux.x);
                            }
                            //Left slide
                            else
                            {
                                desiredDegrees = 90;
                                signDegrees = -1;
                                InvokeRepeating("RotatePlayer", 0, speedRotation);
                                PlayerManager.PM.nextPos = new Vector3(((int)aux.x ^ 1) * -aux.z, 0, ((int)aux.z ^ 1) * aux.x);
                            }
                        }
                        //Si el arrastre es vertical
                        else
                        {
                            //Down slide
                            if (lastPosition.y < firstPosition.y)
                            {
                                desiredDegrees = 180;
                                signDegrees = 1;
                                InvokeRepeating("RotatePlayer", 0, speedRotation);
                                PlayerManager.PM.nextPos = new Vector3(aux.x * -1, 0, aux.z * -1);
                            }
                            else
                                PlayerManager.PM.move = true;
                        }
                    }
                }
            }
        }
    }
    //Metodo que rota al usuario de acuerdo con los grados establecidos
    void RotatePlayer() {
        transform.eulerAngles += new Vector3(0, 1 * signDegrees, 0);
        currentDegrees += 1;
        rotating = true;
        //Cuando los grados actuales son mayores o iguales a los deseados, la rotacion termina
        if (currentDegrees >= desiredDegrees)
        {
            currentDegrees = 0;
            PlayerManager.PM.move = true;
            rotating = false;
            CancelInvoke("RotatePlayer");
        }
    }
}