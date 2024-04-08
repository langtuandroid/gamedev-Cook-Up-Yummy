using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Ingredient",menuName ="New Ingredient")]
public class Ingredient:ScriptableObject
{
    public string name;
    public Sprite sprite;
    public int startingAmount = 0;
    public int cost;
    public GameObject prefabTapAtTheRightTime;
    public GameObject prefabSlice;
    public GameObject prefabDragIngredients;
    public GameObject prefabTapWhenItsDone;
    public GameObject prefabTouchAndHold;
    public int buyingAmount;


    private int stockAmount;

    public int Stock
    {
        get
        {
            if (PlayerPrefs.HasKey(name + "Stock"))
                return PlayerPrefs.GetInt(name + "Stock");
            return startingAmount;
        }
        set
        {
            stockAmount = value;
            PlayerPrefs.SetInt(name + "Stock", stockAmount);
            PlayerPrefs.Save();
        }
    }


    public int Cost()
    {
        return cost / buyingAmount;
    }
}
