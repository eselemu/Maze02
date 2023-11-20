using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    public static GameMaster GM;
    public int selectedDish;

    void Awake()
    {
        if (GM != null)
            GameObject.Destroy(GM);
        else
            GM = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        selectedDish = 0;
    }

    public void GoToRandomMazeScene()
    {
        SceneManager.LoadScene("RandomMaze");
    }
}
