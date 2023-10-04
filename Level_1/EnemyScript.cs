using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class EnemyScript : MonoBehaviour
{
    public Transform character;
    public float CharacterRadius = 1f;
    public float GroundDistance = 1f;
    public float AttackPower = 1f;
    public float speed = 1f;
    public float maxhealth = 100;
    public float currentHealth;
    public float currentTime;
    public float AttackInterval = 2f;
    public float WallRadius = 1f;
    public float DieTime = 1f;
    public float StopTime = 0f;

    public float EnemyRadius = 1f;

    public float currentAnimationDelay;

    public bool isGround = false;
    public bool isAttack = false;
    public bool isDie = false;
    public bool isWall = false;
    public bool isDefence = false;
    public bool isStan = false;

    // Булочки для включения нанесения переодического урона, который реализован в скрипте "MagicSwordComponent"
    public bool isFireMagic = false;
    public bool isIceMagic = false;
    public float FireMagicTime = 0f;
    public float FireMagicDamageInterval;
    public float FireDamage = 0f;

    public LayerMask LayerWall;
    public LayerMask LayerEnemy;
    public NavMeshAgent agent;

    CharacterBattle CharacterHealth;
    public EnemySword enemySword;
    public EnemyHealthBar health;

    public Animator animation;

    private void Awake()
    {
        currentHealth = maxhealth;
        animation = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        CharacterHealth = FindObjectOfType<CharacterBattle>();
        enemySword= FindObjectOfType<EnemySword>();
    }


    private void FixedUpdate()
    {
        if (!isDie)
        {
            EnemyDistance();
            Wall();
            StanScript();
            StoppedEnemy();
        }
    }


    private void moveEnemy()
    {
        RaycastHit hit;
        Vector3 direction = character.position - transform.position;
        if(Physics.Raycast(transform.position, direction, out hit, CharacterRadius))
        {
            Vector3 GroundNormal = hit.normal;
            direction = Vector3.ProjectOnPlane(direction, GroundNormal).normalized;
            direction.y = 0;

            float distancePlayer = Vector3.Distance(transform.position, character.position);
            if(distancePlayer < CharacterRadius && CharacterHealth.currentHealth > 0 && !isWall)
            {
                agent.SetDestination(character.position);
                animation.SetBool("run", true);
                animation.SetBool("defend", false);
                animation.SetBool("attack", false);
            }
            if(agent.isStopped == false && distancePlayer < 2f)
            {
                animation.SetBool("run", false);
                animation.SetBool("defend", true);

                transform.LookAt(character.position);
                if (Time.time - currentTime >= AttackInterval && !isWall)
                {
                    animation.SetBool("attack", true);
                    animation.SetBool("defend", false);
                    currentTime = Time.time;
                }

                if (CharacterHealth.currentHealth <= 0)
                {
                    animation.SetBool("attack", false);
                }
            }

            if(character.position.y > transform.position.y)
            {
                Vector3 HorizontalDirection = new Vector3(direction.x, 0, direction.z);

                if(HorizontalDirection.magnitude > 1f)
                {
                    transform.rotation = Quaternion.LookRotation(direction);
                }
            }
        }
    }

    void ResetAnimation()
    {
        animation.SetBool("run", false);
        animation.SetBool("attack", false);
        animation.SetBool("defend", false);
        animation.SetBool("hit", false);
        animation.SetBool("dizzy", false);
    }
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        health.Sethealth(currentHealth);
        if (currentHealth <= 0)
        {
            isDie = true;
        }

        if (isDie)
        {
            ResetAnimation();
            animation.SetTrigger("die");
            MeshCollider colider = transform.Find("root").GetComponent<MeshCollider>();
            colider.isTrigger = true;
            StartCoroutine(DieInterval());
        }
        if (!isDie)
        {
            animation.SetTrigger("hit");
        }

    }

    // Остановка врагов, после смерти персонажа.

    void StoppedEnemy()
    {
        if(CharacterHealth.isDie)
        {
            agent.isStopped = true;
            ResetAnimation();
        }
    }
    public void Wall()
    {
        RaycastHit hit;
        Vector3 direction = transform.forward;
        Debug.DrawRay(transform.position, direction, Color.red);
        if(Physics.Raycast(transform.position, direction, out hit, WallRadius, LayerWall)) 
        {
            if(hit.collider)
            {
                isWall = true;
            }
        } else
        {
            isWall= false;
        }
    }

    private void EnemyDistance()
    {
        Collider[] EnemyCollider = Physics.OverlapSphere(transform.position, EnemyRadius, LayerEnemy);

        foreach(Collider enemy in EnemyCollider)
        {
            if(enemy.gameObject != gameObject)
            {
                Vector3 OtherEnemy = enemy.transform.position - transform.position;
                float distance = OtherEnemy.magnitude;
                if (distance < EnemyRadius && distance > 0)
                {
                    Vector3 AvoidanceDirection = transform.position - enemy.transform.position;
                    Vector3 SavePosition = enemy.transform.position + AvoidanceDirection.normalized * EnemyRadius;
                    agent.SetDestination(SavePosition);
                    return;
                }
            }
        }
        moveEnemy();
    }

    private void StanScript()
    {
        if(agent.isStopped == true)
        {
            StopTime += Time.deltaTime;
            animation.SetBool("dizzy", true);
        }

        if(StopTime > 8)
        {
            agent.isStopped = false;
            StopTime = 0;
            animation.SetBool("dizzy", false);
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, EnemyRadius);
    }

    IEnumerator DieInterval()
    {
        yield return new WaitForSeconds(DieTime);
        gameObject.SetActive(false);
    }

}
