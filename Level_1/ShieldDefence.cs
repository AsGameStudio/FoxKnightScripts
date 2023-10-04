using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldDefence : MonoBehaviour
{
    public int Defence = 0;

    public bool isShield = false;

    public float StanTime = 0f;
    public float StanRadius = 0f;
    SpinningSkillComponent spinningSkill;

    public CharacterBattle battle;

    private void Start()
    {
        spinningSkill = FindObjectOfType<SpinningSkillComponent>();
    }

    private void Update()
    {
        AttackInDefence();
        StanEnemy();
    }


    private void AttackInDefence()
    {
        if(battle.currentArmor != 0 && Input.GetKey(KeyCode.Mouse1) && !spinningSkill.isDebaf)
        {
            isShield= true;
        } else
        {
            isShield= false;
        }
    }

    void StanEnemy()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, StanRadius);

        foreach(Collider enemy in colliders)
        {
            if (enemy.transform.parent.TryGetComponent(out EnemyScript enemyScript))
            {
                if(Input.GetKey(KeyCode.Mouse1))
                {
                    StanTime += Time.deltaTime;
                } else
                {
                    StanTime = 0;
                }

                if(isShield && Input.GetKey(KeyCode.Mouse1))
                {

                    if(enemyScript.enemySword.isPlayerShield == true && StanTime < 0.2f)
                    {
                        enemyScript.agent.isStopped = true;
                        enemyScript.animation.SetBool("attack", false);
                        enemyScript.animation.SetBool("run", false);
                    }
                }
            }
        }
    }
}
