using System.Collections;
using UnityEngine;

public interface IBottle
{
    void bottle(BottleInterface bottles);
}

public class healthBottle : IBottle
{
    public int bottleAmount = 50;

    public void bottle(BottleInterface bottles)
    {
        bottles.battle.currentHealth += bottleAmount;
    }
}


public class armorBottle : IBottle
{
    public int bottleAmount = 50;


    public void bottle(BottleInterface bottles)
    {
        bottles.battle.currentArmor += bottleAmount;
    }
}


public class BottleInterface : MonoBehaviour
{
    public CharacterBattle battle;
    public CharacterHealthBar health;
    public GameObject bottlehealth;
    public GameObject BottleArmor;

    public float BottleInterval = 0;
    private void Start()
    {
        battle = GetComponent<CharacterBattle>();
        health = FindObjectOfType<CharacterHealthBar>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(battle.currentHealth < 100)
        {
            if (other.CompareTag("BottleHealth"))
            {
                IBottle bottleHealth = new healthBottle();

                bottleHealth.bottle(this);
                bottlehealth.SetActive(false);

                StartCoroutine(BottleHealtInterval());
            }
        }

        if(battle.currentArmor < 100)
        {
            if (other.CompareTag("BottleArmor"))
            {
                IBottle bottleArmor = new armorBottle();
                bottleArmor.bottle(this);
                BottleArmor.SetActive(false);
                StartCoroutine(BottleArmorInterval());
            }
        }
    }

    IEnumerator BottleHealtInterval()
    {
        yield return new WaitForSeconds(BottleInterval);

        if(!bottlehealth.activeSelf)
        {
            bottlehealth.gameObject.SetActive(true);
        }
    }

    IEnumerator BottleArmorInterval()
    {
        yield return new WaitForSeconds(BottleInterval);

        if (!BottleArmor.activeSelf)
        {
            BottleArmor.gameObject.SetActive(true);
        }
    }
}
