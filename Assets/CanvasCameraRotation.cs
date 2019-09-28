using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasCameraRotation : MonoBehaviour
{

    public Camera mainCamera;
    
    private Vector3 startPosition;

    void Start(){
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>() as Camera;
        startPosition = transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.parent.position.x,startPosition.y,transform.parent.position.z);
        transform.LookAt(transform.position + mainCamera.transform.rotation*Vector3.back);
    }
}
