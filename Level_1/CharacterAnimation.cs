using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    Animator animator;

    public bool isDefence = false;
    public bool isDie = false;
    CharacterBattle battle;
    SpinningSkillComponent SpinningSkill;
    FireSkillComponent FireSkill;
    private void Start()
    {
        animator= GetComponent<Animator>();
        battle= GetComponent<CharacterBattle>();
        SpinningSkill= GetComponent<SpinningSkillComponent>();
        FireSkill= GetComponent<FireSkillComponent>();
    }

    private void Update()
    {
        if(!battle.isDie)
        {
            Attack();
            AttackInDefence();
            Shield();
        }
    }

    // Проверка возможности включения анимации атаки.
    bool CheckAttack()
    {
        return battle.isAttack
            && !isDefence
            && Input.GetKeyDown(KeyCode.Mouse0)
            && !SpinningSkill.isDebaf;
    }

    private void Attack()
    {
        if(CheckAttack())
        {
            animator.SetBool("attack", true);
        } else
        {
            animator.SetBool("attack", false);
        }
    }
    private void AttackInDefence()
    {
        if(Input.GetKey(KeyCode.Mouse1) && Input.GetKeyDown(KeyCode.Mouse0))
        {
            animator.SetBool("AttackDefence", true);
        } else
        {
            animator.SetBool("AttackDefence", false);
        }
    }

    private void Shield()
    {
        if(battle.currentArmor != 0 && Input.GetKey(KeyCode.Mouse1))
        {
            isDefence= true;

            animator.SetBool("shield", true);
        } else
        {
            isDefence = false;
            animator.SetBool("shield", false);
        }
    }
    public void Die()
    {
        animator.SetTrigger("die");
        animator.SetBool("shield", false);
        animator.SetBool("walk", false);
        animator.SetBool("run", false);
        animator.SetBool("attack", false);
        animator.SetBool("AttackDefence", false);
        animator.SetBool("dizzy", false);
    }
}
