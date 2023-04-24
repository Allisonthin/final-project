using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
        else if (weaponData.GetType() == typeof(SO_Fist_Aggressive_weapon))
       {
            FistaggressiveWeaponData = (SO_Fist_Aggressive_weapon)weaponData;
            //Debug.Log("fist");
        }
        else
        {
            Debug.LogError("wrong data for the weapon");
        }
    }

    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();

        //Debug.Log("before");
        CheckMeleeAttack();
        //Debug.Log("AnimationActionTrigger");
    }

    private void CheckMeleeAttack()
    {

        if (weaponData.GetType() == typeof(SO_Fist_Aggressive_weapon))
        {
            WeaponAttackDetails fdetails = FistaggressiveWeaponData.AttackDetails[attackCounter];
           
            foreach (IDamageable item in detectedDamageable.ToList())
            {
               
                item.Damage(fdetails.damageAmount);
              
            }

        }
        else if (weaponData.GetType() == typeof(SO_AggressiveWeaponData))
        {
            WeaponAttackDetails sdetails = aggressiveWeaponData.AttackDetails[attackCounter];
            foreach (IDamageable item in detectedDamageable.ToList())
            {

                item.Damage(sdetails.damageAmount);

            }
        }
        
    }

    public void AddToDetected(Collider2D collision)
    {
        //Debug.Log("AddToDetected");
        IDamageable damageable = collision.GetComponent<IDamageable>();

        if(damageable != null)
        {
            detectedDamageable.Add(damageable);
           // Debug.Log("dam");
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
