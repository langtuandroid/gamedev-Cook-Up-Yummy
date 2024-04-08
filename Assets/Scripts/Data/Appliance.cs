using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Appliance",menuName ="New Appliance")]
public class Appliance : ScriptableObject
{
    public string name;
    public Sprite sprite;
    public int cost;
    [SerializeField]
    bool locked;

    public AudioClip applianceSound;

    public bool Locked
    {
        get
        {
            if (PlayerPrefs.HasKey(name + "UNLOCKED"))
                return false;
            else
                return true;
        }
        set
        {
            locked = value;
            if (!locked)
            {
                PlayerPrefs.SetString(name + "UNLOCKED", "yes");
                PlayerPrefs.Save();
            }

        }
    }
}
