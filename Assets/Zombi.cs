﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Zombi : MonoBehaviour
{
    //This moves the player with a slope of 1 or undefined. Very limited

    public float maxHealth;
    private float health;
    public GameObject healthBarUI;
    public Slider healthSlider;
    public float speed = 2.0f;
    public float attackDistance = 5.0f;
    public GameObject player; //Player object must have the tag 'Player';
    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;
    private bool isDead = false;
    public PlayerHealth playerHealth;
    private bool attackable = true;
    private Animator animator;

    public float waitForNextAttack = 1f;
    void Start()
    {
        Physics.IgnoreLayerCollision(8, 9);
        animator = GetComponentInChildren<Animator>();
        animator.SetBool("isAlive", true);
        animator.SetBool("isWalking", false);
        animator.SetBool("isAttacking", false);

        health = maxHealth;
        healthSlider.value = calculateHealth();
        controller = GetComponent<CharacterController>();
    }
    void FixedUpdate()
    {
        healthSlider.value = this.calculateHealth();
        if (this.calculateHealth() > 0)
        {
            if (Vector3.Distance(transform.position, player.transform.position) > attackDistance)
            {
                walkToArcher();
                animator.SetBool("isWalking", true);
            }
            else
            {
                animator.SetBool("isWalking", false);
                if (attackable && playerHealth.isAlive)
                {
                    animator.SetBool("isAttacking", true);
                    attack();
                    attackable = false;
                    StartCoroutine(AttackingYield());
                }

            }
        }
        else if (!isDead)
        {
            isDead = true;
            die();
        }
    }

    IEnumerator AttackingYield()
    {
        yield return new WaitForSeconds(waitForNextAttack);
        attackable = true;
    }

    private void walkToArcher()
    {
        moveDirection = calculateDirection(transform.position, player.transform.position);
        controller.SimpleMove(moveDirection * speed * Time.deltaTime);
    }

    float calculateHealth()
    {
        return health / maxHealth;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.collider.name.Equals("Arrow(Clone)"))
        {
            /*Physics.IgnoreCollision(other.collider, GetComponent<Collider>());
            Debug.Log("Hit: " + other.collider.name);*/

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (isDead)
        {
            return;
        }
        if (other.name.Equals("Arrow(Clone)"))
        {
            Rigidbody arrowRb = other.attachedRigidbody;
            if (arrowRb.velocity.magnitude > 30f)
            {
                health -= arrowRb.velocity.magnitude;
                arrowRb.velocity -= arrowRb.velocity * 0.1f;
                if (arrowRb.velocity.magnitude < 30f)
                {
                    {
                        arrowRb.velocity = Vector3.zero;
                        other.gameObject.SetActive(false);
                    }
                }
                else
                {
                    arrowRb.velocity = Vector3.zero;
                    other.gameObject.SetActive(false);
                }

            }

        }
    }
    private void attack()
    {
        playerHealth.getDamage(10);
    }

    private void die()
    {
        isDead = true;
        animator.SetBool("isAlive", false);
        healthSlider.gameObject.SetActive(false);
        Destroy(gameObject, 10);
    }

    private Vector3 calculateDirection(Vector3 startPoint, Vector3 destination)
    {
        float startX = startPoint.x;
        float startZ = startPoint.z;
        float destinationX = destination.x;
        float destinationZ = destination.z;

        float xDiff = distanceOfTwoPoints(startX, destinationX);
        float zDiff = distanceOfTwoPoints(startZ, destinationZ);


        if (xDiff == 0 && zDiff == 0) return Vector3.zero;
        Vector3 direction = new Vector3(xDiff / (xDiff + zDiff), 0, zDiff / (xDiff + zDiff));
        if (startX > destinationX)
            direction = Vector3.Scale(direction, new Vector3(-1, 1, 1));
        if (startZ > destinationZ)
            direction = Vector3.Scale(direction, new Vector3(1, 1, -1));
        return direction;
    }

    private float distanceOfTwoPoints(float point1, float point2)
    {
        return Mathf.Abs(point1 - point2);
    }

}
