using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.Image;


public enum SwordState
{
    attack,
    nonAttack
}
public class FriendlySword : MonoBehaviour
{
    public float radius = 1f;
    public int damage = 0;
    public int Energy = 20;
    public int EnergyRecovery = 5;

    public float TimeAttack = 0;
    public float currentTimeAttack;
    public float TimeRecovery = 0;
    public float currentTime;

    public LayerMask LayerEnemies;
    public SwordState CurrentState;
    public CharacterEnergyBar energy;
    CharacterBattle CharacterBattle;
    public MainMenuScript menu;

    // Скрипты скиллов персонажа
    SpinningSkillComponent SpinningSkill;
    FireSkillComponent fireSkill;

    public bool isAttacking = false;

    private void Awake()
    {
        CharacterBattle = FindObjectOfType<CharacterBattle>();
        // Инициализация скиллов.
        SpinningSkill = FindObjectOfType<SpinningSkillComponent>();
        fireSkill = FindObjectOfType<FireSkillComponent>();
    }
    private void Update()
    {
        attack();
        EnergyTake();
        RecoveryEnergy();
    }

    bool CheckEnergy()
    {
        return Input.GetKeyDown(KeyCode.Mouse0)
            && CharacterBattle.isAttack
            && !isAttacking
            && !menu.isMenu
            && !SpinningSkill.isSpinning
            && !SpinningSkill.isDebaf
            /*&& !fireSkill.isFire*/
            && CharacterBattle.isAttack
            && !CharacterBattle.isDie;
    }
    private void EnergyTake()
    {
        if (CheckEnergy())
        {
            currentTime = 0;
        }

        if (CharacterBattle.currentEnergy != 100)
        {
            currentTime += Time.deltaTime;
        }

        if (CheckEnergy())
        {
            CharacterBattle.TakeEnergy(Energy);
            isAttacking = true;

            if (CharacterBattle.isAttack)
            {
                CharacterBattle.isStopAttack = false;
            }
        }
    }

    void RecoveryEnergy()
    {
        if (isAttacking)
        {
            currentTimeAttack += Time.deltaTime;
        }

        if (currentTimeAttack > TimeAttack)
        {
            isAttacking = false;
            currentTimeAttack = 0;
        }

        if (currentTime >= TimeRecovery)
        {
            CharacterBattle.EnergyRecovery(EnergyRecovery);
        }
    }


    // проверки для нанесения атаки.
    bool CheckAttack()
    {
        return CurrentState == SwordState.attack
            && !CharacterBattle.isWall
            && !CharacterBattle.isDie
            && CharacterBattle.isAttack
            && !SpinningSkill.isSpinning
            && !SpinningSkill.isDebaf;
    }
    private void attack()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, LayerEnemies);

        CurrentState = Input.GetKeyDown(KeyCode.Mouse0) ? SwordState.attack : SwordState.nonAttack;

        foreach (Collider enemy in colliders)
        {
            if (enemy.transform.parent.TryGetComponent(out EnemyScript enemyScript))
            {
                if(!enemyScript.isDie && CheckAttack())
                {
                    enemyScript.TakeDamage(damage);
                }
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, radius);
    }

}
