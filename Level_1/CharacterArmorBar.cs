using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterArmorBar : MonoBehaviour
{
    public Slider ArmorBar;


    public void SetMaxArmorBar(int armor)
    {
        ArmorBar.maxValue= armor;
        ArmorBar.value = armor;
    }

    public void SetArmorBar(int armor)
    {
        ArmorBar.value= armor;
    }
}
