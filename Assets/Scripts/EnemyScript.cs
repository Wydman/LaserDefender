using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour {

    private int pointsValue = 150;
    // j'utilise le nom du scipt que je veux stocker pour pouvoir l'utiliser sans le find à chaque fois  *!*
    private ScoreScript scoreScript;
 
    private int health = 200;
    private ParticleSystem ps;
    private float shotsPerSeconds = 0.2f;
    private float gameStartTime;
    private float elapsedTime;
    private float TimeBetweenDifficulties = 10f;
    private float incrementShotPerSeconsRates = 0.1f;
    private GameObject Levelmanager;

    //le son d'explosion sera géré sur levelManager

    private void Start()
    {
        // *!*
        scoreScript = GameObject.Find("Score").GetComponent<ScoreScript>();
        // *!*

        ps = GetComponent<ParticleSystem>();
        Levelmanager = GameObject.FindWithTag("LevelManager");
        gameStartTime = Time.time;
    }

    private void Update()
    {
        ////////////////////////////IMPORTANT//////////////////////
        // mise en place de probabilité de tir par rapport ta la variable shotsPerSeconds
        // EN GROS :
        // si Random.Value(qui est un float random entre 0 et 1) est inférieur à
        // probability(qui est une variable(ici shotsPerSeconds) multipliée par time.deltatime(le temps passé depuis la derniere frame)
        // alors on lance ShootEnemyLaser(); qui balance un tir
        // comme ça en modifiant shotsPerSeconds, on peut modifier comletement la vitesse de tir ! tout en gardant un effet random de probabilité
        float probability = shotsPerSeconds * Time.deltaTime;
        if(Random.value < probability)
        {
            ShootEnemyLaser();
        }
        TimePassed();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == ("Laser"))
        {
            // j'apelle la methode public float GetDamage() elle est sensé renvoyée un float
            health -= col.GetComponent<LaserScript>().GetDamage();

            if (health <= 100)
            {
                GetComponent<ParticleSystem>().Play(true);
            }
            if (health <= 0)
            {
                // *!*
                scoreScript.ScoreCalcul(pointsValue);
                // *!*



                // j'utilise ici une méthode pour envoyer le son d'explo via LevelManager aevc comme argument le transform pour utiliser
                // Audiosource.playClipAtPoint, comme ça je localise le son et en plus ça a reglé tout les soucis de saturation et de bug de son lorsque
                //trop d'ennemis explosais en même temps ! Yeah !



                /////////////////////////////////IMPORTANT/////////////////////////
                // De plus, j'utilisais LevelManager car lorsque l'ennemy était destroy avec le script qui lance le son dessus,
                // le son s'arretait, et ça posait evidemment un soucis, mais en utilisant AudioSource.playClipAtPoint, le son 
                // est lancé en stand alone à un endroit, que le script d'origine soit là ou pas !, Très utile !
                // PAR CONTRE !!!!!!!!!! impossible d'utiliser une audiosource custom avec PlayClipAtPoint

                // du coup au final j'ai utilsié PlayOneShot(); qui regle les soucis de bugs sonores et qui permet d'utiliser les audiosources
                //  et par contre pas de localisation.( et j'ia aps test mais je pense que si on lance le son d'un objet qui se détruit ensuite,
                // le son disparait aussi ?( a test!)

                Levelmanager.GetComponent<LevelManager>().ExploEnemy(transform.position);
                GameObject explosion = Instantiate(Resources.Load("Prefabs\\Entities\\Enemy\\Explo"), transform.position, Quaternion.identity) as GameObject;
              //  Levelmanager.GetComponent<LevelManager>().WinCondition();
                Destroy(gameObject);
            }
        }
    }


    void ShootEnemyLaser()
    {
        GameObject laser = Instantiate(Resources.Load("Prefabs\\Entities\\EnemyLaser"), transform.position, Quaternion.identity) as GameObject;
        laser.transform.SetParent(GameObject.Find("LaserBox").transform);
    }

    void TimePassed()
    {
        elapsedTime = Time.time - gameStartTime;
        if(elapsedTime >= TimeBetweenDifficulties)
        {
            TimeBetweenDifficulties += 10;
            shotsPerSeconds += incrementShotPerSeconsRates;
        }
    }

}
