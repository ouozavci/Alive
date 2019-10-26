using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootArrows : MonoBehaviour {
    public GameObject arrow;
    public GameObject fireArrow;
    public GameObject iceArrow;
    public GameObject bow;
    public float arrowSpeed;
    public float despawnTime = 5.0f;
    public bool shootable = true;
    public float waitBeforeNextShot = 1f;
    public SpriteRenderer aim_arrow;
    public GameObject aim_arrow_object;
    public PlayerHealth playerHealth;

    public Text FireCountText;
    public Text IceCountText;

    public int fireArrowCount = 5;
    public int iceArrowCount = 5;

    private float shootPower = 0;
    private bool aiming = false;
    public float minShootPower = 30;
    public float maxShootPower = 100;
    public AudioSource audioSource;
    public AudioClip tensionClip;
    public AudioClip shootClip;
    private Animator animator;

    public bool isFireArrow = false;
    public bool isIceArrow = false;

    void Start () {
        aim_arrow.enabled = false;
        animator = GetComponentInChildren<Animator> ();
        animator.SetBool ("isAiming", false);
    }

    // Update is called once per frame
    void Update () {
        FireCountText.text = fireArrowCount.ToString ();
        IceCountText.text = iceArrowCount.ToString ();
        if (playerHealth.isAlive) {
            if (Input.GetKey (KeyCode.Mouse0)) {
                setTargetPosition ();
            } else if ((aiming || Input.GetKeyUp (KeyCode.Mouse0)) && shootable) {
                aiming = false;
                aim_arrow.enabled = false;
                if (shootPower > minShootPower) {
                    audioSource.Stop ();
                    audioSource.clip = shootClip;
                    shoot ();
                    audioSource.Play ();
                }
                shootPower = 0;
                shootable = false;
                StartCoroutine (ShootingYield ());
            }
        }
    }

    void setTargetPosition () {
        Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast (ray, out hit, 1000)) {
            if (Mathf.Abs (hit.point.x - transform.position.x) < 2 && Mathf.Abs (hit.point.z - transform.position.z) < 2) {
                aiming = true;
                animator.SetBool ("isAiming", true);
                audioSource.clip = tensionClip;
                audioSource.Play ();
                aim_arrow.enabled = true;
            }
            if (aiming) {
                Vector3 target = new Vector3 (hit.point.x * -1, transform.position.y, hit.point.z * -1);
                this.transform.LookAt (target);
                float p = Vector3.Distance (hit.point, transform.position) * 10;
                shootPower = max (p, maxShootPower);
                aim_arrow_object.transform.localScale = new Vector3 (1, 1, shootPower / maxShootPower);
                aim_arrow_object.transform.LookAt (hit.point);
            }
        }
    }

    private float max (float val, float max) {
        if (val > max) {
            return max;
        } else {
            return val;
        }
    }
    IEnumerator ShootingYield () {
        yield return new WaitForSeconds (waitBeforeNextShot);
        shootable = true;
    }

    public void setFireArrow () {
        if (isFireArrow) {
            isFireArrow = false;
            isIceArrow = false;
        } else {
            isFireArrow = true;
            isIceArrow = false;
        }
    }

    public void setIceArrow () {
        if (isIceArrow) {
            isIceArrow = false;
            isFireArrow = false;
        } else {
            isIceArrow = true;
            isFireArrow = false;
        }
    }

    void shoot () {
        animator.SetBool ("isAiming", false);
        GameObject bullet;
        if (isFireArrow && fireArrowCount > 0) {
            bullet = Instantiate (fireArrow, bow.transform.position, bow.transform.rotation);
            fireArrowCount--;
        } else if (isIceArrow && iceArrowCount > 0) {
            bullet = Instantiate (iceArrow, bow.transform.position, bow.transform.rotation);
            iceArrowCount--;
        } else {
            bullet = Instantiate (arrow, bow.transform.position, bow.transform.rotation);
        }
        bullet.GetComponent<Rigidbody> ().velocity = -1 * bullet.transform.up * shootPower;
        //Destroy (bullet, despawnTime);
    }
}