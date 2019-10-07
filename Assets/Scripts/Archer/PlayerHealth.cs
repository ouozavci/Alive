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
    public Button restartButton;
    void Start()
    {
        restartButton.gameObject.SetActive(false);
        isAlive = true;
        health = maxHealth;
        healthText.text = health + "/" + maxHealth;
        playerHealthSlider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            if (restartButton.isActiveAndEnabled == false)
            {
                restartButton.gameObject.SetActive(true);
            }

            if (isAlive)
            {
                die();
            }


        }
    }

    public void getDamage(float damage)
    {
        bloodImage.SetActive(true);
        StartCoroutine(fadeOutImage());
        health -= damage;
        healthText.text = health + "/" + maxHealth;
        playerHealthSlider.value = health / maxHealth;
    }

    IEnumerator fadeOutImage()
    {
        yield return new WaitForSeconds(0.2f);
        bloodImage.SetActive(false);
    }

    void die()
    {
        isAlive = false;
        gameOverSprite.SetActive(true);
        //Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        //rb.mass = 80;
        //rb.AddForce(transform.forward * -1 * 300, ForceMode.Acceleration);
        Debug.Log("Player is dead");

    }

    GameObject GetChildWithName(GameObject obj, string name)
    {
        Transform trans = obj.transform;
        Transform childTrans = trans.Find(name);
        if (childTrans != null)
        {
            return childTrans.gameObject;
        }
        else
        {
            return null;
        }
    }
}
