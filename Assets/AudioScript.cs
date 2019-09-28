using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{

public AudioClip tension_audio;
public AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource.clip = tension_audio;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            audioSource.Play();
        }
    }
}
