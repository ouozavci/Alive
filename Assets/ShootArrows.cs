using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootArrows : MonoBehaviour
{
    public GameObject arrow;
    public GameObject bow;
    public float arrowSpeed;
    public float despawnTime = 3.0f;
    public bool shootable = true;
    public float waitBeforeNextShot = 0.25f;



    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && shootable)
        {
            setTargetPosition();
            shoot();
            shootable = false;
            StartCoroutine(ShootingYield());
        }
    }

    void setTargetPosition(){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, 1000)){
            Vector3 target = hit.point;
            this.transform.LookAt(target);
        }
    }

    IEnumerator ShootingYield()
    {
        yield return new WaitForSeconds(waitBeforeNextShot);
        shootable = true;
    }

    void shoot()
    {
        var bullet = Instantiate(arrow, bow.transform.position, bow.transform.rotation);
        bullet.GetComponent<Rigidbody>().velocity = -1 * bullet.transform.up * arrowSpeed ;

        Destroy(bullet, despawnTime);
    }
}
