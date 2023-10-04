using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpinningSkillComponent : MonoBehaviour
{
    Animator animation;
    CharacterBattle Character;
    CharacterController CharacterMove;
    FireSkillComponent fireSkill;
    MagicSwordComponent magicSword;
    public float AttackInterval;
    public float currentAttackInterval;

    private void Start()
    {
        animation = GetComponent<Animator>();
        Character= GetComponent<CharacterBattle>();
        fireSkill= GetComponent<FireSkillComponent>();
        CharacterMove = GetComponent<CharacterController>();
        magicSword = FindObjectOfType<MagicSwordComponent>();
    }
    private void Update()
    {
        if(!Character.isDie)
        {
            SpinningSkill();
            OnDebaf();
        }
    }

    // Переменные времени
    public float spinningTime = 0f;
    public float currentSpinningTime;
    public float spinningRecovery = 0f;
    public float currentSpinningRecovery;
    public float DebafTime = 0f;
    public float currentDebafTime;

    // окно с временем восстановления скила и текст
    public GameObject spinningRecoveryWindow;
    public TextMeshProUGUI timeRecovery;
    //Булочки для контроля умения
    public bool isSpinning = false;
    public bool isSpinningRecovery = false;
    public bool isDebaf = false;

    // Параметры для нанесения урона 
    public int DamageSpinning;
    public float RadiusSpinning;


    /// <summary>
    /// Метод крутящиеся персонажа
    /// </summary>
    /// <param name="SpinningSkill">Персонаж крутится в течении определённого времени и наносит урон врагам</param>
    void SpinningSkill()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {

            if (!isSpinningRecovery)
            {
                animation.SetBool("Spinning", true);
                isSpinning = true;
            }
        }

        if (isSpinning)
        {
            spinningTime += Time.deltaTime;
            AttackInterval += Time.deltaTime;

            if (AttackInterval > currentAttackInterval)
            {
                AttackInterval = 0;
                SpinningDamage();
            }

            if (spinningTime > currentSpinningTime)
            {
                animation.SetBool("Spinning", false);
                isSpinning = false;
                isSpinningRecovery = true;
                spinningTime = 0f;
                isDebaf = true;
            }
        }

        if (isSpinningRecovery)
        {
            spinningRecoveryWindow.SetActive(true);
            spinningRecovery -= Time.deltaTime;
            timeRecovery.text = spinningRecovery.ToString("F0");

            if (spinningRecovery < 0)
            {
                spinningRecovery = currentSpinningRecovery;
                isSpinningRecovery = false;
                spinningRecoveryWindow.SetActive(false);
            }
        }
    }

    //Метод Дебафа после использования скила
    void OnDebaf()
    {
        if (isDebaf)
        {
            DebafTime += Time.deltaTime;

            if (DebafTime < currentDebafTime)
            {
                animation.SetBool("dizzy", true);
                CharacterMove.minSpeed = 0;
                CharacterMove.maxSpeed = 0;
                CharacterMove.speedRotation = 0;
                
                if(magicSword.isFireMagic || magicSword.isIceMagic)
                {
                    magicSword.isNonMagic = true;
                    magicSword.isFireMagic = false;
                    magicSword.isIceMagic = false;
                }
            }
            else
            {
                animation.SetBool("dizzy", false);
                isDebaf = false;
                DebafTime = 0f;
                CharacterMove.minSpeed = 100;
                CharacterMove.maxSpeed = 200;
                CharacterMove.speedRotation = 10;
            }
        }
    }
    // Метод нанесения урона
    void SpinningDamage()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, RadiusSpinning);

        foreach (Collider enemy in colliders)
        {
            if (enemy.transform.parent.TryGetComponent(out EnemyScript enemyHit))
            {
                if (!enemyHit.isDie && !Character.isWall && !Character.isDie)
                {
                    enemyHit.TakeDamage(DamageSpinning);
                }
            }
        }
    }
}
