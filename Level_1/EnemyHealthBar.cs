using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Slider healthBar;
    public Transform target;


    private void Update()
    {
        transform.LookAt(target);
    }
    public void SetMaxHealth(float health)
    {
        healthBar.maxValue = health;
        healthBar.value = health;
    }
    public void Sethealth(float health)
    {
        healthBar.value = health;
    }

}
