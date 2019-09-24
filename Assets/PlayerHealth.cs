using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHealth : MonoBehaviour
{

    public float maxHealth;
    public float health;
    public Slider playerHealthSlider;
    public Text healthText;
    public bool isAlive;
    // Start is called before the first frame update
    public GameObject gameOverSprite;
    public GameObject bloodImage;
    void Start()
    {
        isAlive = true;
        health = maxHealth;
        healthText.text = health + "/" + maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(health <= 0){
            die();
        }
    }

    public void getDamage(float damage){
        bloodImage.SetActive(true);
        StartCoroutine(fadeOutImage());
        health-=damage;
        healthText.text = health + "/" + maxHealth;
        playerHealthSlider.value = health/maxHealth;
    }

    IEnumerator fadeOutImage()
    {
        yield return new WaitForSeconds(0.2f);
        bloodImage.SetActive(false);
    }

    void die(){
        isAlive = false;
        gameOverSprite.SetActive(true);
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.mass = 80;
        rb.AddForce(transform.forward * -1 * 300, ForceMode.Acceleration);
        Debug.Log("Player is dead");
    }
}
