using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;

public class ShopUIRenderer : MonoBehaviour
{
    [SerializeField]
    private Shop shop;
    [SerializeField]
    private GameObject itemPrefab;
    [SerializeField]
    private GameObject infoDisplay;
    [SerializeField]
    private GameObject shopCanvas;
    [SerializeField]
    private GameObject[] mealTypes;
    [SerializeField]
    private TextMeshProUGUI moneyUI;
    [SerializeField]
    private string path = "Assets/Data/ShopListing.json";

    // Start is called before the first frame update
    public void Start()
    {
        string jsonString = File.ReadAllText(path);
        shop = Shop.CreateFromJSON(jsonString);
        if(shop != null){
            //Debug.Log("Opened");
            shop.money = PlayerPrefs.GetInt("Alcancia_estrellas");
            moneyUI.text = "$" + shop.money.ToString();
            //Debug.Log(shop.inventory);
            for(int i = 0; i < shop.TotalInventory(); i++){
                //Debug.Log(i);
                Item item = shop.inventory[i];
                GameObject newItem = Instantiate(itemPrefab);
                newItem.transform.SetParent(mealTypes[item.type].transform);
                newItem.GetComponent<ShopItem>().SetID(i);
                newItem.GetComponent<ShopItem>().SetOwned(item.owned);
                newItem.GetComponent<ShopItem>().SetName(item.name);
                newItem.GetComponent<ShopItem>().SetCost(item.cost);
                newItem.GetComponent<ShopItem>().SetSprite(item.sprite);
                newItem.GetComponent<ShopItem>().SetRecipe(item.recipe);
                newItem.GetComponent<ShopItem>().SetIngredients(item.ingredients);
                newItem.GetComponent<ShopItem>().SetInfoContainer(infoDisplay);
                newItem.GetComponent<ShopItem>().SetShopCanvas(shopCanvas);
                newItem.GetComponent<ShopItem>().SetController(this.gameObject);
            }
        } else {
            //Debug.Log("Failed to load Shop0");
        }
    }

    public bool BuyItem(int id){
        Item item = shop.inventory[id];
        int cost = item.cost;
        if(shop.money >= cost){
            shop.money -= cost;
            item.owned = true;
            moneyUI.text = "$" + shop.money.ToString();
            shop.inventory[id] = item;
            PlayerPrefs.SetInt("Alcancia_estrellas", shop.money);
            RewriteJson(path);
            return true;
        }
        else {
            //Debug.Log("Not enough money to buy this item");
        }
        return false;
    }

    public void RewriteJson(string path){
        string jsonString = JsonUtility.ToJson(shop);
        File.WriteAllText(path, jsonString);
    }
}
