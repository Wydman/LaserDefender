using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunBackGround : MonoBehaviour {

    public float sunSpeed = 0;


	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += Vector3.down * sunSpeed * Time.deltaTime;
        transform.position += Vector3.left * sunSpeed * Time.deltaTime;
    }
}
