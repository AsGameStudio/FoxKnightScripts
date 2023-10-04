using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static UnityEngine.EventSystems.EventTrigger;
using static UnityEngine.UI.Image;

public class FireSkillComponent : MonoBehaviour
{
    public float FireDamage;
    public float RadiusFire;
    public float FireAttackInterval;
    public float AttackTime;
    public float CurrentAttackTime;
    public float maxDistanceFire;

    public bool isAttackHit;

    OnFireComponent fire;
    CharacterBattle character;
    private void Start()
    {
        fire = FindObjectOfType<OnFireComponent>();
        character = FindObjectOfType<CharacterBattle>();
    }

    private void Update()
    {
        FireAttack();
    }

    void FireAttack()
    {
        float radius = RadiusFire;
        Vector3 direction = transform.forward;
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, radius, direction);

        foreach (RaycastHit enemy in hits)
        {
            if (enemy.transform.parent.TryGetComponent(out EnemyScript enemies))
            {
                if (fire.isFire && !enemies.isDie)
                {

                    float distance = Vector3.Distance(transform.position, enemies.transform.position);

                    if(distance <= maxDistanceFire)
                    {
                        FireAttackInterval += Time.deltaTime;
                        isAttackHit = true;
                        if (FireAttackInterval > CurrentAttackTime)
                        {
                            FireAttackInterval = 0;
                            enemies.TakeDamage(FireDamage);
                            enemies.agent.isStopped = true;
                        }
                    }
                }
            }
        }
    }

}
