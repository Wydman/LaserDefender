using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {


    private void Update()
    {
        if (LevelManager.GameIsOver)
        {
            Animator anim = GetComponent<Animator>();
            anim.SetBool("GameOver", true);
        }
    }



    public void DisableOptionMenu(Animator anim)
    {
        anim.SetBool("Option", false);
    }
    public void EnableOptionMenu(Animator anim)
    {
        anim.SetBool("Option", true);
    }
    public void DisableHowtoPlayMenu(Animator anim)
    {
        anim.SetBool("HowToPlay", false);
    }
    public void EnableHowtoPlayMenu(Animator anim)
    {
        anim.SetBool("HowToPlay", true);
    }
    public void DisableGameOverMenu(Animator anim)
    {
        anim.SetBool("GameOver", false);
        LevelManager.GameIsOver = false;
    }
    public void EnableGameOverMenu(Animator anim)
    {
        anim.SetBool("GameOver", true);
    }

}
