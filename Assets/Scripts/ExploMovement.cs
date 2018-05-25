using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploMovement : MonoBehaviour {

    public bool playerExplosion = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!playerExplosion)
        {
            transform.position += Vector3.up * 1f * Time.deltaTime;
        }
    }
}
