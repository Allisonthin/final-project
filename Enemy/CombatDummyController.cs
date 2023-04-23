/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatDummyController : MonoBehaviour
{
    [SerializeField]
    private float maxHealth, knockbackspeedX, knockbackspeedY, knockbackduration, knockbackDeadSpeedX, KnockbackDeadSpeedY, deathTorque;

    [SerializeField]
    private bool applyKnockback;

    [SerializeField]
    private GameObject hitParticle;

    private float currentHealth, knockbackstart;

    private int playerFacingDirection;

    private bool playerOnLeft, knockback;

    private PlayerController pc;

    private GameObject aliveGo, brokenBotGO, brokenTopGO;

    private Rigidbody2D rbAlive, rbBrokenTop, rbBrokenBot;

    private Animator aliveAnim;

    private void Start()
    {
        currentHealth = maxHealth;

        //move to heirachy and return the first find gameobject with name "player" 
        pc = GameObject.Find("Player").GetComponent<PlayerController>();


        //same as above but instead of looking to heirarchy it look the child gameobject which is attached to
        aliveGo = transform.Find("Alive").gameObject;
        brokenTopGO = transform.Find("Broken Top").gameObject;
        brokenBotGO = transform.Find("Broken Bottom").gameObject;

        aliveAnim = aliveGo.GetComponent<Animator>();
        rbAlive = aliveGo.GetComponent<Rigidbody2D>();
        rbBrokenTop = brokenTopGO.GetComponent<Rigidbody2D>();
        rbBrokenBot = brokenBotGO.GetComponent<Rigidbody2D>();

        aliveGo.SetActive(true);
        brokenTopGO.SetActive(false);
        brokenBotGO.SetActive(false);

    }

    private void Update()
    {
        checkKnockback();
    }

    private void Damage(AttackDetails  details)
    {
        currentHealth -= details.damageAmount ;

        if (details.position .x < aliveGo.transform.position.x)
        {
            playerFacingDirection = 1;
        }
        else
        {
            playerFacingDirection = -1;
        }


       Instantiate(hitParticle, aliveGo.transform.position , Quaternion.Euler (0.0f,0.0f,Random .Range (0.0f,360.0f)));

        if(playerFacingDirection == 1)
        {
            playerOnLeft = true;

        }
        else
        {
            playerOnLeft = false;
        }

        aliveAnim.SetBool("playerOnLeft", playerOnLeft);
        aliveAnim.SetTrigger("damage");

        if (applyKnockback && currentHealth > 0.0f)
        {
            //knockback
            knockBack();
        }

        if(currentHealth <= 0.0f)
        {
            //die
            Die();
        }
    }

    private void knockBack()
    {
        knockback = true;
        knockbackstart = Time.time;
        rbAlive.velocity = new Vector2(knockbackspeedX * playerFacingDirection, knockbackspeedY);
    }

    private void checkKnockback()
    {
        if(Time.time >= knockbackstart + knockbackduration &&  knockback)
        {
            knockback = false;
            rbAlive.velocity = new Vector2(0.0f, rbAlive.velocity.y);
        }
    }

    private void Die()
    {
        aliveGo.SetActive(false);
        brokenTopGO.SetActive(true);
        brokenBotGO.SetActive(true);

        brokenTopGO.transform.position = aliveGo.transform.position;
        brokenBotGO.transform.position = aliveGo.transform.position;


        rbBrokenBot.velocity = new Vector2(knockbackspeedX * playerFacingDirection, knockbackspeedY);
        rbBrokenTop.velocity = new Vector2(knockbackDeadSpeedX * playerFacingDirection, KnockbackDeadSpeedY);
        rbBrokenTop.AddTorque(deathTorque * -playerFacingDirection, ForceMode2D.Impulse);
    }
}
*/