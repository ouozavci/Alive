using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Zombi : MonoBehaviour
{
    //This moves the player with a slope of 1 or undefined. Very limited
    public PointCount pointCount;
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

    public AudioSource audioSource;
    public AudioClip idle_clip1;
    public AudioClip idle_clip2;
    public AudioClip attack_clip1;
    public AudioClip death_clip1;
    public AudioClip death_clip2;
    public AudioClip death_clip3;
    private List<AudioClip> idleClips;
    private List<AudioClip> attackClips;
    private List<AudioClip> deathClips;
    public float waitForNextAttack = 1f;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>() as PlayerHealth;

        pointCount = GameObject.FindWithTag("GameController").GetComponent<PointCount>() as PointCount;

        idleClips = new List<AudioClip>();
        attackClips = new List<AudioClip>();
        deathClips = new List<AudioClip>();
        idleClips.Add(idle_clip1);
        idleClips.Add(idle_clip2);

        attackClips.Add(attack_clip1);

        deathClips.Add(death_clip1);
        deathClips.Add(death_clip2);
        deathClips.Add(death_clip3);

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
                if (!audioSource.isPlaying)
                {
                    audioSource.clip = idleClips[Random.Range(0, idleClips.Count)];
                    audioSource.Play();
                }
                animator.SetBool("isWalking", true);
            }
            else
            {
                animator.SetBool("isWalking", false);
                if (attackable && playerHealth.isAlive)
                {


                    audioSource.Stop();
                    audioSource.clip = attackClips[Random.Range(0, attackClips.Count)];
                    audioSource.Play();


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
        transform.LookAt(player.transform.position);
        moveDirection = calculateDirection(transform.position, player.transform.position);
        controller.SimpleMove(moveDirection * speed * Time.deltaTime);
    }

    float calculateHealth()
    {
        return health / maxHealth;
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
                arrowRb.velocity = arrowRb.velocity * 0.5f;
                if (arrowRb.velocity.magnitude < 30f)
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
    private void attack()
    {
        playerHealth.getDamage(10);
    }

    private void die()
    {
        pointCount.addScore();
        audioSource.Stop();
        audioSource.clip = deathClips[Random.Range(0, deathClips.Count)];
        audioSource.Play();

        isDead = true;
        animator.SetBool("isAlive", false);
        healthSlider.gameObject.SetActive(false);
        Destroy(gameObject, 2);
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
