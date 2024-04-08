using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="Data",menuName ="New Data")]
public class Data : ScriptableObject
{
    public List<FoodMenu> AllMenus;
    public List<Ingredient> AllIngredients;
    public List<Appliance> AllAppliances;

}
