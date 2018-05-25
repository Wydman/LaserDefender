using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour {

    public bool deathAOE = false;
    public float enemyLaserSpeed = 15f;
    public int damage = 100;
    private float increaseAoeRadius = 0.4f;


    private void Update()
    {
       
        if (deathAOE)
        {
            GetComponent<CircleCollider2D>().radius += increaseAoeRadius * Time.deltaTime;
        }
        else
        {
            transform.position += Vector3.up * enemyLaserSpeed * Time.deltaTime;
        }



    }

    // cette méthode n'est pas en void masi en public float car elle renvois une variable(un float)
    // du coup dans EnemyScript(et partout) on peut simplement l'appeler et elle renvera  sa variable
   public  int GetDamage()
    {
        return damage;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name == ("LaserStopUp"))
        {
            Destroy(gameObject);
        }

        if (!deathAOE)
        {
            if (col.gameObject.tag == ("Enemy"))
            {
                Destroy(gameObject);
            }
        }
    }
}
