using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    [SerializeField]
    private bool combatEnable;

    [SerializeField]
    private float inputTimer, attack1Radius, attack1Damage;

    [SerializeField]
    private float stunDamageAmount = 1f;

    [SerializeField]
    private Transform attack1HitBoxPos;

    [SerializeField]
    private LayerMask whatisDamageable;

    private bool gotInput, isAttacking, isFirstAttack;

    private float lastInputTime = Mathf .NegativeInfinity;

    private AttackDetails attackDetails;

    private Animator anim;

    private PlayerController Pc;

    private PlayerStats ps;

    private void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("canAttack", combatEnable);
        Pc = GetComponent<PlayerController>();
        ps = GetComponent<PlayerStats>();
    }

    private void Update()
    {
        checkCombatInput();
        checkAttack();
    }

    private void checkCombatInput()
    {
        if(Input .GetMouseButtonDown(0))
        {
            if(combatEnable)
            {
                //attempt combat
                gotInput = true;
                lastInputTime = Time.time;
            }
        }
    }

    private void checkAttack()
    {
        if(gotInput)
        {
            //perform attack1
            if(!isAttacking)
            {
                gotInput = false;
                isAttacking = true;
                isFirstAttack = !isFirstAttack;

                anim.SetBool("attack1", true);
                anim.SetBool("firstAttack", isFirstAttack);
                anim.SetBool("isAttacking", isAttacking);
            }
        }

        if(Time.time >= lastInputTime + inputTimer)
        {
            //wait for new input
            gotInput = false;
        }
    }

    private void checkAttackHitBox()
    {
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attack1HitBoxPos .position , attack1Radius ,whatisDamageable);

        attackDetails.damageAmount  = attack1Damage;
        attackDetails.position  = transform.position;
        attackDetails.stunDamageAmount = stunDamageAmount;
        
        foreach (Collider2D collider in detectedObjects)
        {
            // calling "Damage" function of basic enemy controller
            collider.transform.parent.SendMessage("Damage", attackDetails );

            //instantiate hit particle
        }
    }

    private void FinishAttack1()
    {
        isAttacking = false;
        anim.SetBool("isAttacking", isAttacking);
        anim.SetBool("attack1", false);

    }

    private void Damage(AttackDetails  attackDetails)
    {
        if (!Pc.getDashStatus())
        {
            int direction;
            ps.DecreaseHealth(attackDetails.damageAmount );

            // damage player using attackdetails

            if (attackDetails.position.x < transform.position.x)
            {
                direction = 1;
            }
            else
            {
                direction = -1;
            }

            Pc.Knockback(direction);
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attack1HitBoxPos.position, attack1Radius);
    }


}
