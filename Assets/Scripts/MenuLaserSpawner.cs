using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuLaserSpawner : MonoBehaviour {

    private float fireRate = 2f;
    private float fireRate2 = 3f;
    private float random1;
    private float random2;

    private void Start()
    {
        InvokeRepeating("ShootLaser", 0.000001f, fireRate);
        InvokeRepeating("ShootLaser2", 0.000001f, fireRate2);
    }


    void ShootLaser()
    {

        random1 = Random.Range(0.5f, 15f);

        GameObject laser = Instantiate(Resources.Load("Prefabs\\Entities\\EnemyLaser"), new Vector3(random1,13,2), Quaternion.identity) as GameObject;
        random1 = Random.Range(0.5f, 15f);
    }

    void ShootLaser2()
    {

        random2 = Random.Range(1f, 14f);

        GameObject laser = Instantiate(Resources.Load("Prefabs\\Entities\\Laser"), new Vector3(random1, -5, 2), Quaternion.identity) as GameObject;
        random2 = Random.Range(1f, 14f);
    }

}
