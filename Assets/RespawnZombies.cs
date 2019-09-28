using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnZombies : MonoBehaviour
{
    public GameObject zombie;
    public int level;
    public int maxZombieCount;
    public float respawnOffset;

    private int zombieCount;
    public float minX;
    public float maxX;
    public float minZ;
    public float maxZ;
    private bool respawnable;
    // Start is called before the first frame update
    void Start()
    {
        zombieCount = 0;
        respawnable = true;
        level = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (respawnable && zombieCount < maxZombieCount)
        {

            respawnable = false;
            respawnZombie();
            StartCoroutine(RespawnYield());
        }
    }

    private void respawnZombie()
    {
        zombieCount++;
        Instantiate(zombie, new Vector3(Random.Range(minX, maxX), 0f, Random.Range(minZ, maxZ)), zombie.transform.rotation);
    }

    IEnumerator RespawnYield()
    {
        yield return new WaitForSeconds(respawnOffset);
        respawnable = true;
    }
}
