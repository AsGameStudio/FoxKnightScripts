using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySword : MonoBehaviour
{
    public int SwordDamage = 0;
    public float AttackInterval = 0f;
    public float AttackRadius = 0f;

    public bool isPlayerShield = false;

    CharacterBattle PlayerHealth;
    ShieldDefence defence;
    EnemyScript die;
    Animator animator;
    SpinningSkillComponent spinning;
    private void Awake()
    {
        animator= GetComponent<Animator>();
        PlayerHealth = FindObjectOfType<CharacterBattle>();
        defence= FindObjectOfType<ShieldDefence>();
        die= FindObjectOfType<EnemyScript>();
        spinning = FindObjectOfType<SpinningSkillComponent>();
    }


    bool CanPerformAction()
    {
        return !die.isDie
               && !PlayerHealth.isDie
               && !die.isWall
               && !defence.isShield
               && die.agent.isStopped == false
               && !spinning.isSpinning;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (CanPerformAction() && other.CompareTag("Character"))
        {
            PlayerHealth.TakeDamage(SwordDamage);
        }

        if (defence.isShield && other.CompareTag("Character"))
        {
            PlayerHealth.TakeArmorDamage(SwordDamage);
        }

        if (other.CompareTag("Character"))
        {

            if (!isPlayerShield)
            {
                isPlayerShield = true;
            }
            else
            {
                isPlayerShield = false;
            }
        }
        else
        {
            isPlayerShield = false;
        }
    }

}
