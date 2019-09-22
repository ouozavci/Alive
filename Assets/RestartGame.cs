using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class RestartGame : MonoBehaviour
{
    public Button button;

    void start(){
        button.onClick.AddListener(restartGame);
    }

    void update(){
        Debug.Log("");
    }
    public void restartGame(){
        Debug.Log("Clicked");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
