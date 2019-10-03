using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
public class RestartGame : MonoBehaviour
{
    public void restartGame(){
        Debug.Log("Clicked");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
