using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointCount : MonoBehaviour
{
    public static int score;
    public Text scoreText;

    private RespawnZombies respawn;
    private GameObject mainCanvas;
    public Button restartButton;
    void Start()
    {
        score = 0;
        respawn = GameObject.FindWithTag("GameController").GetComponent<RespawnZombies>() as RespawnZombies;
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + score;
        if (score == respawn.maxZombieCount)
        {
            restartButton.gameObject.SetActive(true);
        }
    }

    public void addScore()
    {
        score++;
    }
}
