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
    public float waitBeforeNextShot = 1f;
    public SpriteRenderer aim_arrow;
    public GameObject aim_arrow_object;
    public PlayerHealth playerHealth;

    private float shootPower = 0;
    private bool aiming = false;
    public float minShootPower = 30;
    public float maxShootPower = 100;

    void Start()
    {
        aim_arrow.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHealth.isAlive)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                setTargetPosition();
            }
            if (Input.GetKeyUp(KeyCode.Mouse0) && aiming && shootable)
            {
                aiming = false;
                aim_arrow.enabled = false;
                if (shootPower > minShootPower)
                    shoot();
                shootPower = 0;
                shootable = false;
                StartCoroutine(ShootingYield());
            }
        }
    }

    void setTargetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000))
        {
            if (Mathf.Abs(hit.point.x - transform.position.x) < 1 && Mathf.Abs(hit.point.z - transform.position.z) < 1)
            {
                aiming = true;
                aim_arrow.enabled = true;
            }
            if (aiming)
            {
                Vector3 target = new Vector3(hit.point.x * -1, transform.position.y, hit.point.z * -1);
                this.transform.LookAt(target);
                float p = Vector3.Distance(hit.point, transform.position) * 10;
                shootPower = max(p, maxShootPower);
                aim_arrow_object.transform.localScale = new Vector3(1, 1, shootPower / maxShootPower);
                aim_arrow_object.transform.LookAt(hit.point);
            }
        }
    }

    private float max(float val, float max)
    {
        if (val > max)
        {
            return max;
        }
        else
        {
            return val;
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
        bullet.GetComponent<Rigidbody>().velocity = -1 * bullet.transform.up * shootPower;
        Destroy(bullet, despawnTime);
    }
}
