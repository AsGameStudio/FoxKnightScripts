using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterEnergyBar : MonoBehaviour
{
    public Slider EnergyBar;



    public void SetMaxEnergy(float energy)
    {
        EnergyBar.maxValue = energy;
        EnergyBar.value = energy;
    }

    public void SetEnergy(float energy)
    {
        EnergyBar.value = energy;
    }
}
