using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDebafComponent : MonoBehaviour
{
    EnemyScript Enemies;
    CharacterBattle Character;
    OnFireComponent FireSkill;
    public GameObject ParticleFire;
    public GameObject ParticleIce;
    // Таймы для огненного дебафа
    public float FireMagicTime = 0f;
    public float currentMagicTime = 5f;
    public float FireMagicDamageInterval;
    public float CurrentDamageInterval = 0.5f;
    public float FireDamage = 0f;

    // Таймы для ледяного дебафа

    public float IceMagicTime = 0f;
    public float CurrentIceMagicTime;
    public float IceDebafSpeed = 0f;
    public float IceDebafFireDamage = 0f;
    private void Start()
    {
        Enemies= GetComponent<EnemyScript>();
        Character = FindObjectOfType<CharacterBattle>();
        FireSkill = FindObjectOfType<OnFireComponent>();
    }

    private void Update()
    {
        FireMagicDamage();
        IceMagicDamage();
    }

    bool CheckForDamage()
    {
        return !Enemies.isDie
               && !Character.isDie
               && FireMagicDamageInterval > CurrentDamageInterval;
    }

    /// <summary>
    /// Метод урона от огня
    /// </summary>
    /// <param name="FireMagicDamage">
    /// Отвечает за нанесения врагам урона огнём, вызванным огненным мечом.
    /// </param>
    void FireMagicDamage()
    {
        if (Enemies.isFireMagic)
        {
            Enemies.agent.isStopped = true;
            if (FireMagicTime < currentMagicTime && !Character.isDie)
            {
                FireMagicTime += Time.deltaTime;
                FireMagicDamageInterval += Time.deltaTime;
                ParticleFire.SetActive(true);
            }

            else if (FireMagicTime > currentMagicTime)
            {
                FireMagicTime = 0;
                FireMagicDamageInterval = 0;
                Enemies.isFireMagic = false;
                ParticleFire.SetActive(false);
            }

            if (CheckForDamage() && !Enemies.isIceMagic)
            {
                FireMagicDamageInterval = 0;
                Enemies.TakeDamage(FireDamage);
            }

            if(CheckForDamage() && Enemies.isIceMagic)
            {
                FireMagicDamageInterval = 0;
                Enemies.TakeDamage(IceDebafFireDamage);
            }
            
        }
    }

    /// <summary>
    /// Метод замедления от холода
    /// </summary>
    /// <param name="IceMagicDamage">
    /// Позволяет замедлить противника, окутывая его холодом, а также увеличить урон по этому противнику от огня.
    /// </param>
    /// 

    void IceMagicDamage()
    {
        if(Enemies.isIceMagic)
        {
            Enemies.agent.speed = IceDebafSpeed;
            
            if(IceMagicTime < CurrentIceMagicTime && !Character.isDie)
            {
                IceMagicTime += Time.deltaTime;
                ParticleIce.SetActive(true);
            }

            else if (IceMagicTime > CurrentIceMagicTime)
            {
                IceMagicTime = 0;
                Enemies.isIceMagic = false;
                ParticleIce.SetActive(false);
                Enemies.agent.speed = 2.5f;
            }
            
        }
    }
}
