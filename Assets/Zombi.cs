using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombi : MonoBehaviour
{
    //This moves the player with a slope of 1 or undefined. Very limited

    public float speed = 2.0f;
    public float attackDistance = 5.0f;
    public GameObject player; //Player object must have the tag 'Player';
    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }
    void FixedUpdate()
    {
        
        if (Vector3.Distance(transform.position, player.transform.position) > attackDistance)
        {
            moveDirection = calculateDirection(transform.position, player.transform.position);
            controller.Move(moveDirection * Time.deltaTime * speed);
        
        }else{
            attack();
        }
    }

    private void attack(){
        Debug.Log("Attacking player");
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

        Debug.Log(direction);
        return direction;
    }

    private float distanceOfTwoPoints(float point1, float point2)
    {
        return Mathf.Abs(point1 - point2);
    }

}
