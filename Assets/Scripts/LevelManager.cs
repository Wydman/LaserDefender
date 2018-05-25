using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    public static bool GameIsOver = false;
    public static int autoBonus = 0;



    private AudioSource audioPlayerExplo;
    private AudioClip audioclidPlayerExplo;

    private AudioSource audioExploSound;
    private AudioClip audioclipExploSound;


 



    // ///////////////////////////////////////REUTILISATION de PlayerController(Laser Defender)//////////////////////////////////////////////////////
    // en fait je me suis rendus compte que ce serais mille fois plus simle d'ajouter le son d'explosion au levelmanager
    // comme ça le son ne disparait pas apres le gamedestroy du player.
    // je laisse tout ça ici car ça peut être très utile lorsque que 'lona besoin de multiples AudioSources sur un même objet.
    // methode (tirée d'un tuto en favoris) qui sert à utiliser plusieurs audiosources indépendamment sur le même objet
    // ici je m'en sers pour pouvoir déclencher le son d'explosion, sans foutre la merde avec le son de tir du vaisseau
    // methode relativement simple au final, on met en argument de la methodes des variables qui sont utilisé par
    // le Componnent via les lignes suivantes.( en plus de lier un audioclip avec une audiosource créée précedement)


    public AudioSource AddAudio(AudioClip clip, bool loop, bool playAwake, float vol)
    {
        AudioSource newAudio = gameObject.AddComponent<AudioSource>();
        newAudio.clip = clip;
        newAudio.loop = loop;
        newAudio.playOnAwake = playAwake;
        newAudio.volume = vol;
        return newAudio;
        
    }

    private void Awake()
    {
        audioPlayerExplo = AddAudio(audioclidPlayerExplo = Resources.Load<AudioClip>("Sounds\\ExploPlayer"), false, true, 0.1f);
        audioExploSound = AddAudio(audioclipExploSound = Resources.Load<AudioClip>("Sounds\\ExploEnemy"), false, true, 0.1f);
        
    }

 

    public void LoadLevel(string name)
    {
        //condition pour Reset le jeu  si on relance Start
        if (name == "01-Start")
        {
            Debug.Log("Level load requested name" + name);
            // a utiliser pour changer de scènes, mais il faut ajouter "using UnityEngine.SceneManagement;" en haut
            SceneManager.LoadScene(name);
        }
        if (name == "02-Game")
        {
            Debug.Log("Level load requested name" + name);
            ScoreScript.score = 0;
            SceneManager.LoadScene(name);
        }

        else
        {
            Debug.Log("Level load requested name" + name);
            // a utiliser pour changer de scènes, mais il faut ajouter "using UnityEngine.SceneManagement;" en haut
            SceneManager.LoadScene(name);
        }

    }


    public void OptionMenu()
    {
        GameObject menu = GameObject.Find("Menu");
        GameObject options = GameObject.Find("Options");

        int menuX = Mathf.Clamp(0, -400, 400);
        int optionsX = Mathf.Clamp(0, -400, 400);

        menu.transform.position = new Vector3(menuX * Time.deltaTime, transform.position.y, transform.position.z);
        options.transform.position = new Vector3(optionsX, transform.position.y, transform.position.z);

    }


    public void QuitGame()
    {
        Debug.Log("Quit game");
        Application.Quit();
    }

    public void GameOver()
    {
        audioPlayerExplo.Play();
        GameObject.Find("Player").GetComponent<PlayerController>().FlareExplosion();
        StartCoroutine(LoseDelay());
    }
   
    public void ExploEnemy(Vector3 soundTransform)
    {
        audioExploSound.pitch = Random.Range(0.5f,2f);
        audioExploSound.volume = Random.Range(0.2f, 0.4f);
        // utiliser le play oneshot permet de beneficier de l'audiosource(ce qui n'est aps le cas avec PlayClipAtPoint, et d'enlever les bugs
        //liés a trop de son fait en meme temps avec Play();
        audioExploSound.PlayOneShot(audioclipExploSound);
       // audioExploSound.Play();

     //   AudioSource.PlayClipAtPoint(audioclipExploSound, soundTransform);



    }

    IEnumerator LoseDelay()
    {
        yield return new WaitForSeconds(0.7f);
        GameObject.Find("Player").GetComponent<PlayerController>().Explosion();
      //  yield return new WaitForSeconds(0.3f);
        GameObject.Find("Player").GetComponent<PlayerController>().DestroyGameobject();
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("01-Start");
        GameIsOver = true;
        

    }

}
