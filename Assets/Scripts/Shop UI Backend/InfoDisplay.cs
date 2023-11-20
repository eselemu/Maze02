using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoDisplay : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI nameContainer;
    [SerializeField]
    private TextMeshProUGUI infoContainer;

    public void Activate(ShopItem item){
        GameMaster.GM.selectedDish = item.GetID();
        gameObject.SetActive(true);
        nameContainer.text = item.GetName();
        infoContainer.text = "Ingredients\n";
        string[] ingredients = item.GetIngredients();
        for(int i = 0; i < ingredients.Length; i++){
            infoContainer.text += "     ";
            infoContainer.text += ingredients[i];
            infoContainer.text += "\n";
        }
        infoContainer.text += "\nRecipe\n";
        infoContainer.text += "     ";
        infoContainer.text += item.GetRecipe();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
