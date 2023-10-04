using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


// Отвечает за ключения умения огня и его перезарядка.
public class OnFireComponent : MonoBehaviour
{
    public GameObject fire;
    public GameObject FireRecoveryWindow;
    public TextMeshProUGUI FireRecoveryText;
    SpinningSkillComponent spinningSkill;
    CharacterBattle character;
    MagicSwordComponent magicSword;
    // Управление временем огнём
    public float FireTime = 0;
    public float CurrentFireTime;

    // управления временем перезарядки умения

    public float FireRecovery = 0;
    public float CurrentFireRecovery;

    // Булочки 
    public bool isFire = false;
    public bool isFireRecovery = false;

    // Дебав интервал для персонажа
    public float DebafInterval;
    public float currentDebafInterval;

    // Урон от дебафа и до какого значения, дебаф будет действовать
    public int DebafDamage;
    public int MaxDebafDamage;
    private void Start()
    {
        spinningSkill = FindObjectOfType<SpinningSkillComponent>();
        character = GetComponent<CharacterBattle>();
        magicSword = FindObjectOfType<MagicSwordComponent>();
    }

    private void Update()
    {
        OnFireSkill();
    }

    bool CheckForeSkill()
    {
        return Input.GetKey(KeyCode.Alpha2)
            && !isFireRecovery
            && !spinningSkill.isDebaf
            && !character.isDie;
    }

    // влючение пламени
    void OnFireSkill()
    {
        if(CheckForeSkill())
        {
            isFire = true;
            fire.SetActive(true);
        }

        if(isFire)
        {
            FireTime += Time.deltaTime;
            DebafInterval += Time.deltaTime;

            /*if (magicSword.isFireMagic || magicSword.isIceMagic)
            {
                magicSword.isNonMagic = true;
            }*/
        }

        if(DebafInterval > currentDebafInterval && character.currentHealth > MaxDebafDamage)
        {
            DebafInterval = 0;
            character.TakeDamage(DebafDamage);
        }

        OfFireSkill();

    }

    // вылючение пламени
    void OfFireSkill()
    {
        if (FireTime > CurrentFireTime)
        {
            isFireRecovery = true;
            isFire = false;
            FireTime = 0;
            fire.SetActive(false);
            FireRecoveryWindow.SetActive(true);
        }

        if(isFireRecovery)
        {
            FireRecovery -= Time.deltaTime;
            FireRecoveryText.text = FireRecovery.ToString("F0");
        }

        if(FireRecovery <= 0)
        {
            isFireRecovery = false;
            FireRecovery = CurrentFireRecovery;
            FireRecoveryWindow.SetActive(false);
        }
    }
}
