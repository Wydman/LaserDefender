using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Interface : MonoBehaviour {

    private float Shield;

    private Image shieldBar;

    private void Start()
    {
        shieldBar = GameObject.FindWithTag("ShieldBar").GetComponent<Image>();
    }

    private void Update()
    {
        if (GameObject.FindWithTag("Player"))
        {
            Shield = GameObject.FindWithTag("Player").GetComponent<PlayerController>().shieldCharge;
            shieldBar.fillAmount = Shield / 300;

            if (GameObject.FindWithTag("Player").GetComponent<PlayerController>().shieldUp == false)
            {
                shieldBar.color = new Color(1,0.3f,0.3f);
                //shieldBar.color = Color.red;


            }
            else
            {
                if (shieldBar.fillAmount >= 0.3f)
                {
                    shieldBar.color = Color.white;
                }
                if (shieldBar.fillAmount <= 0.3f)
                {
                    shieldBar.color = Color.cyan;
                }
            }


        }
    }


}
