using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggressiveWeapon : Weapon
{
    protected SO_AggressiveWeaponData aggressiveWeaponData;
    protected SO_Fist_Aggressive_weapon FistaggressiveWeaponData;


    private List<IDamageable> detectedDamageable = new List<IDamageable>();

    protected override void Awake()
    {
        base.Awake();
        //Debug.Log("dhasdf");

        if (weaponData.GetType() == typeof(SO_AggressiveWeaponData))
        {
            aggressiveWeaponData = (SO_AggressiveWeaponData)weaponData;
            //Debug.Log("sword");
        }
        /*else if (weaponData.GetType() == typeof(SO_Fist_Aggressive_weapon))
       {
            FistaggressiveWeaponData = (SO_Fist_Aggressive_weapon)weaponData;
            Debug.Log("fist");
        }*/
        else
        {
            Debug.LogError("wrong data for the weapon");
        }
    }

    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();

        CheckMeleeAttack();
        //Debug.Log("AnimationActionTrigger");
    }

    private void CheckMeleeAttack()
    {
        //Debug.Log(detectedDamageable);
        WeaponAttackDetails sdetails = aggressiveWeaponData.AttackDetails[attackCounter];
        //WeaponAttackDetails Fdetails = aggressiveWeaponData.AttackDetails[attackCounter];


        foreach (IDamageable item in detectedDamageable)
        {
            item.Damage(sdetails.damageAmount);
            //item.Damage(sdetails.damageAmount);

            //Debug.Log("IDamageable");
        }
    }

    public void AddToDetected(Collider2D collision)
    {
        //Debug.Log("AddToDetected");
        IDamageable damageable = collision.GetComponent<IDamageable>();

        if(damageable != null)
        {
            detectedDamageable.Add(damageable);
            //Debug.Log(detectedDamageable.Count);
        }
    }
    public void RemoveFromDetected(Collider2D collision)
    {
        //Debug.Log("RemoveFromDetected");
        IDamageable damageable = collision.GetComponent<IDamageable>();

        if (damageable != null)
        {
            //Debug.Log("remove");
            detectedDamageable.Remove(damageable);
        }
    }
}
