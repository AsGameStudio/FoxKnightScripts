// Весь этот треш писался в начале разработки, при недостаточном уровне навыков. Коментарии не писал, пренебрегал
// код местами(А может и вообще) выглядит не очень, но его трогать или переписывать не буду, поскольку это хороший
// способ увидеть свой рост и смеяться в будущем над тем, что я тут понаписывал.

using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.EventSystems.EventTrigger;

public class CharacterBattle : MonoBehaviour
{
    public float attackPower = 1.0f;
    public float radiusAttack = 1f;
    public float attackAngle = 45f;
    public int maxhealth = 100;
    public int maxArmor = 100;
    public float maxEnergy = 100f;
    public float minEnergy = 15f;
    public int currentHealth;
    public int currentArmor;
    public float currentEnergy;
    public float TimeRecovery = 1f;
    public float currentTime;
    public float RadiusWall = 1f;

    public bool isDie = false;
    public bool isWall = false;
    public bool isStopAttack = false;
    public bool isAttack = true;

    public CinemachineFreeLook Camera;

    public List<Transform> enemies = new List<Transform>();

    Animator animator;
    public CharacterHealthBar health;
    CharacterArmorBar armor;
    CharacterEnergyBar energy;
    ShieldDefence shield;
    private CharacterController speed;
    private CharacterAnimation die;
    EnemyScript enemyHealth;

    public LayerMask LayerWall;

    private void Awake()
    {
        armor= FindObjectOfType<CharacterArmorBar>();
        energy = FindObjectOfType<CharacterEnergyBar>();
    }
    private void Start()
    {
        currentHealth = maxhealth;
        currentArmor = maxArmor;
        currentEnergy = maxEnergy;
        health.SetMaxHealth(maxhealth);
        animator = GetComponent<Animator>();
        speed= GetComponent<CharacterController>();
        die = GetComponent<CharacterAnimation>();
        enemyHealth = FindObjectOfType<EnemyScript>();
        shield = FindObjectOfType<ShieldDefence>();
    }

    private void Update()
    {
        CheckWall();
        health.Sethealth(currentHealth);
        armor.SetArmorBar(currentArmor);
        energy.SetEnergy(currentEnergy);
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        health.Sethealth(currentHealth);

        if (currentHealth <= 0)
        {
            die.Die();
            speed.enabled = false;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
            Camera.enabled = false;
            die.enabled = false;
            isDie = true;
            currentHealth = 0;
            
        }

        if(currentHealth > maxhealth)
        {
            currentHealth = 100;
        }
    }
    public void TakeArmorDamage(int damage)
    {
        currentArmor -= damage;
        armor.SetArmorBar(currentArmor);

        if(currentArmor <= 0)
        {
            currentArmor = 0;
        }

        if(currentArmor == 0)
        {
            shield.isShield= false;
        }

        if(currentArmor > maxArmor)
        {
            currentArmor = 100;
        }

    }

    public void TakeEnergy(int Energy)
    {
        currentEnergy -= Energy;
        energy.SetEnergy(currentEnergy);

        if(currentEnergy <= 0)
        {
            isStopAttack = true;
            isAttack = false;
        }

        if(currentEnergy < 0)
        {
            currentEnergy = 0;
        }

        if(currentEnergy > maxEnergy)
        {
            currentEnergy = maxEnergy;
            isStopAttack = false;
        }
    }

    public void EnergyRecovery(float recovery)
    {
        currentEnergy += recovery * Time.deltaTime;

        if (currentEnergy > minEnergy)
        {
            isAttack = true;
        }



        if (currentEnergy > maxEnergy)
        {
            currentEnergy = maxEnergy;
        }
    }
    public void CheckWall()
    {
        RaycastHit hit;
        Vector3 direction = transform.forward;
        if(Physics.Raycast(transform.position, direction, out hit, RadiusWall, LayerWall))
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
}
