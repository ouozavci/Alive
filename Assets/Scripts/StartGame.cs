using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public Text loadingText;
    public Button startButton;
    // Start is called before the first frame update

    // Update is called once per frame
    public void start()
    {
        startButton.gameObject.SetActive(false);
        StartCoroutine(startLoading());
    }

    IEnumerator startLoading()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(1);
        while (!async.isDone)
        {
            loadingText.text = "Loading: %" + (int)(((async.progress + 0.1f) * 100f));
            yield return null;
        }
    }

}
