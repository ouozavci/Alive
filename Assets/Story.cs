using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Story : MonoBehaviour
{

    public Image storyPaper;
    public Text centerText;

    // Start is called before the first frame update
    void Start()
    {
        storyPaper.gameObject.SetActive(true);
        Time.timeScale = 0f;
    }



    public void continueGame(){
        storyPaper.gameObject.SetActive(false);
        StartCoroutine(FadeInAndOut(1f,centerText));
        Time.timeScale = 1f;
    }

       public IEnumerator FadeInAndOut(float t, Text i)
    {
        i.gameObject.SetActive(true);
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }
        StartCoroutine(FadeTextToZeroAlpha(1f,centerText));
    }
 
    public IEnumerator FadeTextToZeroAlpha(float t, Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
        i.gameObject.SetActive(false);
    }
}
