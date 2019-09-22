using System.Collections;
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
    void Start()
    {
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
            }
            else
            {
                attack();
            }
        }
        else if(!isDead)
        {
            isDead = true;
            die();
        }
    }

    private void walkToArcher()
    {
        Debug.Log(controller.isGrounded);
        moveDirection = calculateDirection(transform.position, player.transform.position);
        controller.SimpleMove(moveDirection * speed * Time.deltaTime);
    }

    float calculateHealth()
    {
        return health / maxHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Equals("Arrow(Clone)"))
        {
            health -= 50;
            Debug.Log(calculateHealth());
        }

    }

    private void attack()
    {
    }

    private void die()
    {
        controller.enabled = false;
        gameObject.GetComponent<CapsuleCollider>().isTrigger = false;
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.mass = 80;
        rb.AddForce(calculateDirection(transform.position, player.transform.position) * -1 * 300, ForceMode.Acceleration);

        Destroy(gameObject, 30);
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
