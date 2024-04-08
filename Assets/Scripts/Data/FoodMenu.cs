using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Menu",menuName ="New Menu")]
public class FoodMenu : ScriptableObject
{
    public string name;
    public Sprite icon;
    public Sprite titleSprite;
    public FoodMenu menuToEarnStarsIn;
    public int starsRequiredToUnlockNextMenu;
    public List<Dish> dishes;

    public int TotalStarsEarned()
    {
        int stars = 0;
        foreach (Dish d in dishes)
            stars += d.Stars;
        return stars;
    }

    public bool Locked()
    {
        if (menuToEarnStarsIn != null)
        {
            if (menuToEarnStarsIn.TotalStarsEarned() >= starsRequiredToUnlockNextMenu)
                return false;
        }
        else
            return false;


        return true;
    }
}
