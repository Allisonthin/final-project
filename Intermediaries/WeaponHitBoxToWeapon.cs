using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHitBoxToWeapon : MonoBehaviour
{
    private AggressiveWeapon weapon;

    private void Awake()
    {
        weapon = GetComponentInParent<AggressiveWeapon>();
        //Debug.Log("WeaponHitBoxToWeapon");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("onTriggerEnter2d");
        weapon.AddToDetected(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Debug.Log("onTriggerExit2d");
        weapon.RemoveFromDetected(collision);   
    }
}
