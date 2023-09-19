using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItem : MonoBehaviour
{
    [SerializeField]
    private int iD;
    [SerializeField]
    private string nameItem;
    [SerializeField]
    private string[] ingredients;
    [SerializeField]
    private Color color;
    [SerializeField]
    private int cost;
    [SerializeField]
    private bool owned = false;
    [SerializeField]
    private Sprite sprite;
    [SerializeField]
    private string recipe;
    [SerializeField]
    private Button button;
    [SerializeField]
    private GameObject infoDisplay;
    [SerializeField]
    private GameObject shopCanvas;
    [SerializeField]
    private GameObject controller;
    
    public TextMeshProUGUI nameContainer;
    public TextMeshProUGUI costContainer;
    public Image image;

    void Start()
    {
    }

    public void SetID(int iD){
        this.iD = iD;
    }

    public int GetID(){
        return iD;
    }

    public void SetName(string nameItem){
        this.nameItem = nameItem;
        nameContainer.text = nameItem;
    }

    public string GetName(){
        return nameItem;
    }
    
    public void SetIngredients(string[] ingredients){
        this.ingredients = ingredients;
    }

    public string[] GetIngredients(){
        return ingredients;
    }

    public void SetColor(Color color){
        this.color = color;
    }

    public Color GetColor(){
        return color;
    }

    public void SetCost(int cost){
        this.cost = cost;
        costContainer.text = cost.ToString() + " Stars.";
    }

    public int GetCost(){
        return cost;
    }

    public void SetSprite(string spritePath){
        byte[] spriteBytes = File.ReadAllBytes(spritePath);
        Texture2D tex = new Texture2D(1,1);
        tex.LoadImage(spriteBytes);
        sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        //Debug.Log(sprite);
        image.sprite = sprite;
    }

    public Sprite GetSprite(){
        return sprite;
    }

    public void SetOwned (bool owned){
        this.owned = owned;
        button.interactable = owned;
        costContainer.transform.parent.gameObject.SetActive(!owned);
        if(owned){
            image.material = null;
        } 
    }

    public bool GetOwned (){
        return owned;
    }

    public void SetRecipe(string recipe){
        this.recipe = recipe;
    }

    public string GetRecipe(){
        return recipe;
    }

    public void SetInfoContainer(GameObject infoDisplay){
        this.infoDisplay = infoDisplay;
    }

    public void SetShopCanvas(GameObject shopCanvas){
     
        this.shopCanvas = shopCanvas;
    }
    public void SetController(GameObject controller){
        this.controller = controller;
    }

    public void ShowInfo(){
        shopCanvas.SetActive(false);
        infoDisplay.GetComponent<InfoDisplay>().Activate(this);
    }

    public void Buy(){
        if(controller.GetComponent<ShopUIRenderer>().BuyItem(iD)){
            SetOwned(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
