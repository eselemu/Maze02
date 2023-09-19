using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SceneSwitch(int id){
        string path = "Assets/Data/ShopListing.json";
        SceneManager.LoadScene(id);
        string jsonString = File.ReadAllText(path);
        Shop shop = Shop.CreateFromJSON(jsonString);
        shop.money = PlayerPrefs.GetInt("Alcania_estrellas");
        jsonString = JsonUtility.ToJson(shop);
        File.WriteAllText(path, jsonString);
    }
}