using UnityEngine;
using System.Collections;
public class DamageScript : MonoBehaviour
{
    public int damageAmount = 1;
    public float attackCooldown = 1f;
    public float maxCooldown = 1f;


    void Update()
    {
        if (attackCooldown >= maxCooldown)
        {

        }
        attackCooldown += Time.deltaTime;
    }
}