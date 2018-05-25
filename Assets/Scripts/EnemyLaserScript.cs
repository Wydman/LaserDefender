using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaserScript : MonoBehaviour {
    private float laserSpeed = 10f;

    public int damage = 100;


    private void Update()
    {
        transform.position += Vector3.down * laserSpeed * Time.deltaTime;
    }

    // cette méthode n'est pas en void masi en public float car elle renvois une variable(un float)
    // du coup dans EnemyScript(et partout) on peut simplement l'appeler et elle renvera  sa variable
   public  int GetDamage()
    {
        return damage;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == ("LaserStopDown"))
        {
            Destroy(gameObject);
        }
        if (col.gameObject.tag == ("Player"))
        {
            Destroy(gameObject);
        }
        if (col.gameObject.name == ("Shield"))
        {
            Destroy(gameObject);
        }
        if (col.gameObject.tag == ("Laser") && col.GetComponent<LaserScript>().deathAOE == true)
        {
            Destroy(gameObject);
        }
    }



    }
