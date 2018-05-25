using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {


    /////////////////////////////////////////////////IMPORTANT//////////////////////////////
    // ABRUTI SI TU MET LA VARIABLE EN PUBLIQUE PENSE QUE SE SERA LA VALEUR SUR LINSPECTEUR QUI SERA PRISE EN COMPTE, PAS DANS LE CODE
    // DONC SI LA MULTIPLICATION AVEC Time.DeltaTime FOIRE C'EST JUSTE QU'ELLE EST BRIDEE PAR CE QUI EST DONNE DANS L'INSPECTEUR !!
    // NAAARBBB
    // du coup je la met en private pour ne plus avoir se genre de soucis pour le moment.
    private float playerSpeed = 10f;
    float xMin;float xMax;
    private float padding = 0.5f;
    private float fireRate = 0.2f;
    private bool cheat = false;
    private int health = 500;
    private bool Shielded = false;
    public float shieldCharge = 300;
    private float shieldUse = 100;
    private float shieldReload = 100;
    public bool shieldUp = true;
    private GameObject Levelmanager;
    private GameObject shield;

    public static bool dead = false;

    //Sound

    private AudioSource audioShootSound;
    private AudioClip audioclipShootSound;

    // ///////////////////////////////////////NE SERA FINALEMENT PAS UTILISE//////////////////////////////////////////////////////+
    
    private AudioSource audioPlayerHit;
    private AudioClip audioclidPlayerHit;
    
    // ///////////////////////////////////////NE SERA FINALEMENT PAS UTILISE//////////////////////////////////////////////////////-

    // ///////////////////////////////////////NE SERA FINALEMENT PAS UTILISE//////////////////////////////////////////////////////+
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
    

    // ///////////////////////////////////////NE SERA FINALEMENT PAS UTILISE//////////////////////////////////////////////////////-


    private void Awake()
    {
        audioShootSound = GetComponent<AudioSource>();
        audioclipShootSound = GetComponent<AudioSource>().clip;

        GameObject.Find("Score").GetComponent<ScoreScript>().playerIsAlive = true;
        GetComponent<EdgeCollider2D>().enabled = true;
        dead = false;

        // ///////////////////////////////////////NE SERA FINALEMENT PAS UTILISE//////////////////////////////////////////////////////+
        /*
         Plein de façon de faire pour ajouter l'audioclip a son audiosource par exemple :

         audioclidPlayerExplo = Resources.Load<AudioClip>("Sounds\\ExploPlayer");
         audioPlayerExplo = AddAudio(audioclidPlayerExplo, false, true, 0.2f);

        ou sans utiliser de variables audioclip : 

        audioPlayerExplo = AddAudio(Resources.Load<AudioClip>("Sounds\\ExploPlayer"), false,true,0.2f);

        ou encore pour  utiliser une variable audioclip, tout en n'utilisant qu'une ligne :

         audioPlayerExplo = AddAudio(audioclidPlayerExplo = Resources.Load<AudioClip>("Sounds\\ExploPlayer"), false,true,0.2f);
         (que j'utilise donc ici)
       */


        audioPlayerHit = AddAudio(audioclidPlayerHit = Resources.Load<AudioClip>("Sounds\\HitPlayer1"), false, true, 0.1f);

        // ///////////////////////////////////////NE SERA FINALEMENT PAS UTILISE//////////////////////////////////////////////////////-

    }

    void Start () {
        // SUPER METHODE POUR DEFINIR LE CHAMP D'ACTION D'UN OBJET !!! A LIRE !!! #CLAMP
        PlayerClampWithCamera();
        Levelmanager = GameObject.FindWithTag("LevelManager");
        shield = GameObject.FindWithTag("Shield");
    }


    void Update()
    {
        if (!dead)
        {
            PlayerControls();



            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                ////////////////////////// IPORTANT///////////////////////
                /*
                 * Ici j'utilise la méthode InvokeReapeating qui permet de lancer une méthode(ici ShootLaser) à un taux de répétition donné
                 * la méthode demande 3 argument, 
                 * -le premier est la méthode appelée EN STRING (oui c'est chelou) : "ShootLaser"
                 * -time est le délai avant le premier appel de la méthode ici il est 0.00001 pour que ce soit immédiat, mais pas 0.0 ca rça peut bugger
                 * donc si on veux un lancement immédiat il faut le mettre a ce genre de valeur : 0.00001
                 * -le 3eme est le taux de répétition en secondes, ici fireRate que j'ai mis à 0.7f, et qui peut changer quand on veut.
                 * 
                 * dans le contexte is se passe : quand on reste appuyé sur Space ou souris(0) la répétition se met en marche à chaque fireRate
                 * ensuite quand on arrete d'appuyer, on lance CancelInvoke("NomDeLaMéthodeEnString") pour arrêter la répétition 
                 */
                InvokeRepeating("ShootLaser", 0.000001f, fireRate);
            }
            if (Input.GetKeyUp(KeyCode.Space) || Input.GetMouseButtonUp(0))
            {
                ////////////////////////// IPORTANT///////////////////////
                // il est important de savoir arrêter le InvokeReapeating(), on le fait de cette manière
                CancelInvoke("ShootLaser");
            }
            //cheat
            if (Input.GetKeyDown(KeyCode.O)) { cheat = !cheat; }
            if (cheat) { fireRate = 0.01f; }

            ShieldManagement();

        }
        
    }

    void PlayerControls()
    {

        if (Input.GetKey(KeyCode.RightArrow)|| Input.GetKey(KeyCode.D))
        {
            // plusieurs méthodes possibles pour le même déplacement
            //transform.position += new Vector3(playerSpeed * Time.deltaTime, 0, 0);
            transform.position += Vector3.right * playerSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.Q))
        {
            // plusieurs méthodes possibles pour le même déplacement
            //transform.position += new Vector3(-playerSpeed * Time.deltaTime, 0, 0);
            transform.position += Vector3.left * playerSpeed * Time.deltaTime;
        }
        float newX = Mathf.Clamp(transform.position.x, xMin, xMax);
        transform.position = new Vector3(newX,transform.position.y, transform.position.z);
    }

    void PlayerClampWithCamera()
    {
        //on calcule la distance en Z entre la camera et le player
        float distanceCamera = transform.position.z - Camera.main.transform.position.z;

        //////////////////////IMPORTANT//////////////////// #CLAMP
        /*
         * Alors ici on va utiliser la camera pour trouver définir l'aire de jeu  du player
         * En gros on créé un nouveau vecteur  qui sera égal à un vecteur un peu particulier
         * Le vecteur3 contenu dans  "Camera.main.ViewportToWorldPoint(new Vector3(0,0,distanceCamera));"
         * A besoin de 3 arguments spéciaux :
         *  le premier est défini entre 0 et 1, ce chiffre correspond à l'axe X pour :
         * 0 =  le plus a gauche de la cam
         * 1 = le plus à droite de la cam
         * 0.5 = le milieu de l'axe X de la cam
         *  le second est le même que el premier avec Y
         * donc imaginons que le vecteur soit : 
         * (0,0,test) , ça correspondera à en abs à gauche de l'écran 
         * (1,0.5,test), correspondera au milieu  à droite  de l'écran
         * (1,1,test), correspondera a en haut à droite de l'écran
         *  le troisième à besoin de la distance entre la cam et l'objet en Z, d'ou le calcul précédent pour le renseigner dans le vecteur.
         * 
         * ensuite on récupere la position x des deux vecteurs(plus a gauche et plus a droite pour clamper)
         * et on l'applique a xMin et xMax pour els utiliser dans la fonction clamp
         * comme ça on certain que la valeur est parfaite par rapport à la caméra !
         * 
         * 
         * je fini par ajouter et enlever "padding" pour faire en sorte que le player reste  toujours entier a l'écran
         * car le sprite sort de moitier de l'écran lorsqu'il est en xMin ou xMax(logique)
         * 
         * En gros l'avantage c'est que unity fais tout les calculs pour nous, il y a juste à ajouter une valeur SUR les variables sûres de ViewportWorldPoint :)
         */

        Vector3 leftMost = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distanceCamera));
        Vector3 rightMost = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distanceCamera));
        xMin = leftMost.x + padding;
        xMax = rightMost.x - padding;
    }

    void ShieldManagement()
    {
        shieldCharge = Mathf.Clamp(shieldCharge, 0, 300);

        if (Input.GetMouseButton(1) && shieldUp && shieldCharge >0)
        {
            Shielded = true;
            shieldCharge -= shieldUse * Time.deltaTime;
            shield.SetActive(true);
        }
       else
        {
            Shielded = false;
            shieldCharge += shieldReload * Time.deltaTime;
            shield.SetActive(false);
        }
        if(shieldCharge <= 0)
        {
            shieldUp = false; 
        }
        if (!shieldUp)
        {
            StartCoroutine(ShieldOverload());
        }
    }

 



    void ShootLaser()
    {
        GameObject laser = Instantiate(Resources.Load("Prefabs\\Entities\\Laser"), transform.position, Quaternion.identity) as GameObject;
        laser.transform.SetParent(GameObject.Find("LaserBox").transform);

        audioShootSound.PlayOneShot(audioclipShootSound);
       // AudioSource.PlayClipAtPoint(audioclipShootSound, transform.position);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == ("EnemyLaser") && !Shielded)
        {
            // j'apelle la methode public float GetDamage() elle est sensé renvoyée un float
            health -= col.GetComponent<EnemyLaserScript>().GetDamage();
            audioPlayerHit.PlayOneShot(audioclidPlayerHit);
            LifeManager();

            if (health <= 100)
            {
              
            }
            if (health <= 0)
            {
                dead = true;
                GameObject.Find("Score").GetComponent<ScoreScript>().playerIsAlive = false;
                GetComponent<EdgeCollider2D>().enabled = false;
                Levelmanager.GetComponent<LevelManager>().GameOver();
                

            }
        }
    }

    public void FlareExplosion()
    {
        GameObject explosion = Instantiate(Resources.Load("Prefabs\\Entities\\Player\\FlareExploPlayer"), transform.position, Quaternion.identity) as GameObject;
    }

    public void Explosion()
    {
        

        GameObject DeathAOEParticles = Instantiate(Resources.Load("Prefabs\\Entities\\DeathAOEParticles"), transform.position, Quaternion.identity) as GameObject;
        GameObject deathAOE = Instantiate(Resources.Load("Prefabs\\Entities\\DeathAOE"), transform.position, Quaternion.identity) as GameObject;
        GameObject explosion = Instantiate(Resources.Load("Prefabs\\Entities\\Player\\ExploPlayer"), transform.position, Quaternion.identity) as GameObject;  
    }

    public void DestroyGameobject()
    {
        Destroy(gameObject);
    }


    // Not Efficient but i wanted to try a switch statement :p
    private void LifeManager()
    {
        switch (health)
        {
            case 500:
                GameObject.Find("Vie5").GetComponent<Image>().enabled = true; GameObject.Find("Vie4").GetComponent<Image>().enabled = true;
                GameObject.Find("Vie3").GetComponent<Image>().enabled = true; GameObject.Find("Vie2").GetComponent<Image>().enabled = true;
                GameObject.Find("Vie1").GetComponent<Image>().enabled = true;
                break;

            case 400:
                GameObject.Find("Vie5").GetComponent<Image>().enabled = false; GameObject.Find("Vie4").GetComponent<Image>().enabled = true;
                GameObject.Find("Vie3").GetComponent<Image>().enabled = true; GameObject.Find("Vie2").GetComponent<Image>().enabled = true;
                GameObject.Find("Vie1").GetComponent<Image>().enabled = true;
                GameObject explosion01 = Instantiate(Resources.Load("Prefabs\\Entities\\Player\\ExploDamagePlayer"), transform.position, Quaternion.identity) as GameObject;
                explosion01.transform.parent = transform;
                break;

            case 300:
                GameObject.Find("Vie5").GetComponent<Image>().enabled = false; GameObject.Find("Vie4").GetComponent<Image>().enabled = false;
                GameObject.Find("Vie3").GetComponent<Image>().enabled = true; GameObject.Find("Vie2").GetComponent<Image>().enabled = true;
                GameObject.Find("Vie1").GetComponent<Image>().enabled = true;
                GameObject explosion02 = Instantiate(Resources.Load("Prefabs\\Entities\\Player\\ExploDamagePlayer"), transform.position, Quaternion.identity) as GameObject;
                explosion02.transform.parent = transform;
                break;

            case 200:
                GameObject.Find("Vie5").GetComponent<Image>().enabled = false; GameObject.Find("Vie4").GetComponent<Image>().enabled = false;
                GameObject.Find("Vie3").GetComponent<Image>().enabled = false; GameObject.Find("Vie2").GetComponent<Image>().enabled = true;
                GameObject.Find("Vie1").GetComponent<Image>().enabled = true;
                GameObject explosion03 = Instantiate(Resources.Load("Prefabs\\Entities\\Player\\ExploDamagePlayer"), transform.position, Quaternion.identity) as GameObject;
                explosion03.transform.parent = transform;
                break;

            case 100:
                GameObject.Find("Vie5").GetComponent<Image>().enabled = false; GameObject.Find("Vie4").GetComponent<Image>().enabled = false;
                GameObject.Find("Vie3").GetComponent<Image>().enabled = false; GameObject.Find("Vie2").GetComponent<Image>().enabled = false;
                GameObject.Find("Vie1").GetComponent<Image>().enabled = true;
                GameObject explosion04 = Instantiate(Resources.Load("Prefabs\\Entities\\Player\\ExploDamagePlayer"), transform.position, Quaternion.identity) as GameObject;
                explosion04.transform.parent = transform;
                break;
            case 0:
                GameObject.Find("Vie5").GetComponent<Image>().enabled = false; GameObject.Find("Vie4").GetComponent<Image>().enabled = false;
                GameObject.Find("Vie3").GetComponent<Image>().enabled = false; GameObject.Find("Vie2").GetComponent<Image>().enabled = false;
                GameObject.Find("Vie1").GetComponent<Image>().enabled = false;
                break;

        }

            

    }

    IEnumerator ShieldOverload()
    {
            yield return new WaitForSeconds(2);
            shieldUp = true;
        
    }
}
