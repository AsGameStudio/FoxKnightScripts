using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MagicSwordComponent : MonoBehaviour
{
    public GameObject fireMagicSword;
    public GameObject iceMagicSword;
    public GameObject MagicSwordRecoveryWindow;
    public TextMeshProUGUI MagicSwordTimeRecovery;
    SpinningSkillComponent spinningSkill;
    OnFireComponent fireSkill;
    CharacterBattle character;
    public bool isFireMagic = false;
    public bool isIceMagic = false;
    public bool isNonMagic = false;

    public float MagicRadius = 0f;
    public float MagicSwordRecovery = 0f;
    public float CurrentSwordRecovery;


    private void Start()
    {
        spinningSkill = FindObjectOfType<SpinningSkillComponent>();
        character = FindObjectOfType<CharacterBattle>();
        fireSkill = FindObjectOfType<OnFireComponent>();
    }
    private void Update()
    {
        if(!character.isDie)
        {
            ChangeMagicSword();
            FireMagicSword();
            IceMagicSword();
        }
    }



    // Проверки для смены состояния меча

    bool CheckMagicSword()
    {
        return Input.GetKeyDown(KeyCode.Alpha3)
            && !spinningSkill.isDebaf;
    }

    /// <summary>
    /// Метод изменения магии меча
    /// </summary>
    /// <param name="ChangeMagicSword">Меняется тип магии у оружия персонажа.</param>

    void ChangeMagicSword()
    {
        if (CheckMagicSword())
        {
            if (!isFireMagic && !isIceMagic && !isNonMagic)
            {
                isFireMagic = true;
            }
            else if (isFireMagic && !isIceMagic && !isNonMagic)
            {
                isFireMagic = false;
                isIceMagic = true;
            }
            else if (isIceMagic)
            {
                isIceMagic = false;
                isNonMagic = true;
            }
        }

        if (isFireMagic)
        {
            fireMagicSword.SetActive(true);
        }
        else if (isIceMagic)
        {
            fireMagicSword.SetActive(false);
            iceMagicSword.SetActive(true);
        }
        else
        {
            fireMagicSword.SetActive(false);
            iceMagicSword.SetActive(false);
        }

        if(isNonMagic)
        {
            MagicSwordRecoveryWindow.SetActive(true);
            MagicSwordRecovery -= Time.deltaTime;
            MagicSwordTimeRecovery.text = MagicSwordRecovery.ToString("F0");
            isFireMagic = false;
            isIceMagic = false;

            if (MagicSwordRecovery < 0)
            {
                MagicSwordRecovery = CurrentSwordRecovery;
                MagicSwordRecoveryWindow.SetActive(false);
                isNonMagic = false;
            }
        }
    }


    /// <summary>
    /// Метод огненного меча
    /// </summary>
    /// <param name="FireMagicSword">Позволяет огненному мечу наносить удары и поджигать противников</param>

    void FireMagicSword()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, MagicRadius);

        foreach(Collider enemy in colliders)
        {
            if(enemy.transform.parent.TryGetComponent(out EnemyScript enemies))
            {
                if(isFireMagic && Input.GetKey(KeyCode.Mouse0) && !character.isStopAttack)
                {
                    enemies.isFireMagic = true;
                }
            }
        }
    }

    /// <summary>
    /// Метод Ледяного меча
    /// </summary>
    /// <param name="IceMagicSword">
    /// Позволяет ледяному мечу замедлять противников. Кроме того, в состоянии ледяного замедления
    /// урон от огня по такому противнику увеличивается вдвое
    /// </param>
 
    void IceMagicSword()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, MagicRadius);

        foreach (Collider enemy in colliders)
        {
            if (enemy.transform.parent.TryGetComponent(out EnemyScript enemies))
            {
                if (isIceMagic && Input.GetKey(KeyCode.Mouse0) && !character.isStopAttack)
                {
                    enemies.isIceMagic = true;
                }
            }
        }
    }

}
