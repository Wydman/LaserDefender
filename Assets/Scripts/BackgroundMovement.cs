using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMovement : MonoBehaviour {

    private float speed = 0.1f;
    Vector2 offset;

    void Update()
    {
        offset = new Vector2(0, Time.deltaTime * speed/10);
        GetComponent<Renderer>().material.mainTextureOffset += offset;

        if (EnemySpawner.NextWave)
        {
            offset = new Vector2(0, Time.deltaTime * speed);
            GetComponent<Renderer>().material.mainTextureOffset += offset;
          //  EnemySpawner.NextWave = false;
        }
    }






}
