using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Dish",menuName ="New Dish")]
public class Dish : ScriptableObject
{
    public string name;
    public Sprite sprite;
    public GameObject prefab;

    public int earnings1Star;
    public int earnings2Star;
    public int earnings3Star;

    public StarGoals goals;


    public List<DishIngredient> ingredients;
    public List<Appliance> appliances;

    public List<DishCreationPhase> creationPhases;

    private int stars;

    public int Cost()
    {
        int cost = 0;
        foreach(DishIngredient ing in ingredients)
        {
            cost += ing.ingredient.Cost()*ing.amount;
        }
        return cost;
    }

    public int Stars
    {
        get
        {
            if (PlayerPrefs.HasKey(name + "Stars"))
                stars = PlayerPrefs.GetInt(name + "Stars");
            return stars;
        }
        set
        {
            stars = value;
            PlayerPrefs.SetInt(name + "Stars", stars);

            PlayerPrefs.Save();
        }
    }

    public bool HasIngredients()
    {
        foreach (DishIngredient ding in ingredients)
            if (ding.amount > ding.ingredient.Stock)
                return false;

        foreach (Appliance app in appliances)
            if (app.Locked)
                return false;

        return true;
    }
    public bool HasFoodIngredients()
    {
        foreach (DishIngredient ding in ingredients)
            if (ding.amount > ding.ingredient.Stock)
                return false;
        return true;
    }

    public void RemoveIngredientsNeededForDish()
    {
        foreach (DishIngredient ding in ingredients)
            ding.ingredient.Stock -= ding.amount;
    }

    public void AddIngredientsNeededForDish()
    {
        foreach (DishIngredient ding in ingredients)
            ding.ingredient.Stock += ding.amount;
    }
}

[System.Serializable]
public class DishIngredient
{
    public Ingredient ingredient;
    public int amount;
}

[System.Serializable]
public class StarGoals
{
    public int goal1Star;
    public int goal2Star;
    public int goal3Star;
}

[System.Serializable]
public class DishCreationPhase
{
    public string sceneName;
    public float timeLimit;
    [Range(.25f,.75f)]
    public float barPerfectGoal;
    public Ingredient ingredient;
    public int ingredientAmount;
    public GameObject gameVersionPrefab;
    public AudioClip soundEffect;
}

