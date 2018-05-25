using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour {

    public bool playerExplosion = false;
    public bool flarePlayerExplosion = false;


    void Start () {
        
        StartCoroutine(DeleteClone());

        if (!playerExplosion)
        {
            StartCoroutine(ExplosionFade(1f));
        }
        if (flarePlayerExplosion)
        {
            StartCoroutine(FlarePlayerExplosionFade(0.2f));
        }
        else
        {
            StartCoroutine(PlayerExplosionFade(1f));
        }

    }

	void Update () {
     //  transform.position += Vector3.up * 0.5f * Time.deltaTime;
    }

    IEnumerator DeleteClone()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    IEnumerator ExplosionFade(float aTime)
    {
        
        for (float t = 1f; t > 0.0f; t -= Time.deltaTime / aTime)
        {
            GetComponentInChildren<LensFlare>().brightness = t;
            yield return null;
        }
    }
    IEnumerator PlayerExplosionFade(float aTime)
    {
        for (float t = 1f; t > 0.0f; t -= Time.deltaTime / aTime)
        {
            GetComponentInChildren<LensFlare>().brightness = t;
            yield return null;
        }
    }

    IEnumerator FlarePlayerExplosionFade(float aTime)
    {
        for (float t = 3f; t > 0.0f; t -= Time.deltaTime / aTime)
        {
            GetComponentInChildren<LensFlare>().brightness = t;
            yield return null;
        }
    }

}
