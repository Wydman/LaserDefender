using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {


    public static bool NextWave = false;

    public float width = 12f;
    public float height = 10f;

    private float respawnTiming = 2f;

    private float spawnDelay = 0.1f;
    private Vector3 SpawnOrigin;

    private ParticleSystem starfield1;
    private ParticleSystem starfield2;


    private float enemySpeed = 0.2f;
    private float enemyIncrementSpeed = 0.05f; 
    float xMin; float xMax;
    private bool border = false;
    private bool OnPos = false;
    private GameObject enemy;

    private bool RespawnOnce = false;

    // private Vector3 childPos;

    // Sert juste a faire apparaitre un gizmo dans la scène UNITY (une wireSphere en l'occurence)
    // tout ça juste pour positioner les "positions" facilement  en voyant toujours les autres grace au gizmo 

    void Start () {
      //  SpawnEnnemy();
        SpawnUntilFull();

        starfield1 = GameObject.Find("StarField1").GetComponent<ParticleSystem>();
        starfield2 = GameObject.Find("StarField2").GetComponent<ParticleSystem>();


    }



    public void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(width, height));
    }

    void Update () {
        EnemyFormationControls();
        EnemyFormationClampWithCamera();



        ////////////////////IMPORTANT//////////////
        // il faut comprendre que mettre ces méthodes (qui renvois qqch et ne sont pas void) en condition 
        // lance la méthode elle même, en gros pas besoin de la lancer normallement comme ça : "allMembersDead();"
        // le fait de la mettre en condition lance la méthode pour vérifier la condition
        // et comme elle sont ici dans l'update, elle se lancent à chaque frame pour vérifier leur contenu.

        if (AllMembersDead())
        {

            if (!RespawnOnce && PlayerController.dead == false)
            {
                StartCoroutine(RespawnTime(respawnTiming));
                enemySpeed = 0.2f;
                RespawnOnce = true;
            }


            // SpawnUntilFull();
        }
        /*
        if (NextFreePosition() != null)
        {
         //   Debug.Log(NextFreePosition());
        // SpawnUntilFull();
        }
        */
    }




    Transform NextFreePosition()
    {
        // pour chaque enfants de EnnemyFormation( le support de ce script) fais :
        foreach (Transform childPositionGameObjet in transform)
        {
            // comprendre ici : 
            // donc là on passe en revu chaque enfant de ennemyFormation déjà, donc avec cette condition,
            // on vérifie si les enfants de ennemyFormation ont un enfant, 
            // dans ce cas précis, avec "childCount", on vérifie si  il y à 1 enfant ou 0, et on lance le code si
            // le childcount est égal a 0 et on retourne la position non pas de l'enfant de l'enfant mais de l'enfant de ennemyFormation passé en revu avec le foreach
            // du coup si il n'y à pas d'enfant dans la position enfante de ennemyformation, on renvois la position
            //sinon on return null
            if (childPositionGameObjet.childCount == 0)
            {
                RespawnOnce = false;
                return childPositionGameObjet;
            }
            
        }

        return null;

    }




    bool AllMembersDead()
    {
        // ici c'est un peu différent, de la meme façon on passe ne revu tout els enfants de ennemyFormation
        // ensuite on vérifie avec la condition si l'enfant de EnnemyFormation à un enfant, si oui, on renvois false
        // si le childcount est égal à 0 et donc vide, on ne retourne pas true
        // la seule chose qui peut faire que allmemberdead renvois true, c'est que la boucle foreach passe en revue TOUT
        // les enfants de ennemyformation, et que TOUS les enfants n'ai pas d'enfant, De cette façon, la valeur ne retourne jamais false
        // et à la fin de la boucle, la seule chose a retourner est true
        //  à mon avis il faut commprendre qu'a chaque itération de la méthode elle ne peut renvoyer qu'une valeur, soit true soit false
        // alors si on trouve un child du child de ennemy formation, la valeur est mise en false de base et quoi qu'il se passe, elle renvera false
        // sans enfant de l'enfant, la methode renvois null, jusqu'a ce qu'on lui fasse retourner true

        // dans le if, il faut comprendre qu'on vérifie les enfants des enfants de ennemyformation, la valeur( pour cet exemple
        // ou il ne peut y avoir qu'un ennemi par position) ne peut renvoyer que 1 ou 0
        // on pourrais très bien mettre la condition  à "== 1" au lieu de "> 0"

        foreach(Transform childPositionGameObjet in transform)
        {
           if (childPositionGameObjet.childCount > 0)
            {
                return false;
            }
        }

        
        //  SpawnUntilFull();
        return true;

    }




    void EnemyFormationControls()
    {
        if (!border)
        {
            transform.position += Vector3.right * enemySpeed * Time.deltaTime;
        }
        else
        {
            transform.position += Vector3.left * enemySpeed * Time.deltaTime;
        }

        if (transform.position.x >= xMax)
        {
            border = true;
            // fais descendre les ennemis( pas utile sur ce jeu)
            //transform.position -= -Vector3.down / 10;
            enemySpeed += enemyIncrementSpeed;
            //Debug.Log(enemySpeed);
        }
        else if (transform.position.x <= xMin)
        {
            border = false;
            // fais descendre les ennemis( pas utile sur ce jeu)
            //transform.position -= -Vector3.down / 10;
            enemySpeed += enemyIncrementSpeed;
            //Debug.Log(enemySpeed);
        }
        float newX = Mathf.Clamp(transform.position.x, xMin, xMax);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }


    void EnemyFormationClampWithCamera()
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
        // au lieu d'utiliser une variable pour ajuster  quand rebondissent les ennemis,
        // je j'ajoute la moitié de la largeur de l'écran car l'origine de EnemyFormation se trouve au milieu de l'écran !
        //( ça reste ptete plus précis ave cune variable dediée? a voir..)
        xMin = leftMost.x + (0.5f * width);
        xMax = rightMost.x - (0.5f * width);
    }


    /* ///////////////LERP IMPORTANT////////////
     * en plaçant la methode de lerp dans un ienumerator, le lerp se met en palce douvement pas comme dans une autre methode ou 
     * l'appli essaye de calculer la boucle le plus vite possible
     * je l'utilise pas pour le moment
     */
     /*
    IEnumerator Anim()
    {
            for (float t = 0.01f; t < 1; t += 0.01f)
            {
                if (enemy && NextFreePosition())
                {
                    enemy.transform.position = Vector3.Lerp(SpawnOrigin, NextFreePosition().position, t);
                    yield return null;
                }
        }
    }
    */

        void SpawnUntilFull()
    {

        // oui, vu qu'on utilise une méthode qui renvois un transform, on peut directement l'utiliser comme une variable
        Transform freePosition = NextFreePosition();
        // on vérifie que freePosition n'est pas null pour pas bugger la machine quand il n'y en a pas
        if (freePosition)
        {
            SpawnOrigin = new Vector3(Random.Range(-6f, 6f), 5, 0);
            enemy = Instantiate(Resources.Load("Prefabs\\Entities\\Enemy\\Enemy"), freePosition.position, Quaternion.identity) as GameObject;
          //  enemy = Instantiate(Resources.Load("Prefabs\\Entities\\Enemy\\Enemy"), SpawnOrigin, Quaternion.identity) as GameObject;
            enemy.transform.parent = freePosition;
          //  StartCoroutine (Anim());
        }
        if (NextFreePosition())
        {
            Invoke("SpawnUntilFull", spawnDelay);
        }
    }


    
    
    void SpawnEnnemy()
    {

        // pour chaque enfants de l'objet( ici l'objet est EnemyFormation et ses enfants toutes les positions à l'itnerieur) je lance le code
        foreach (Transform child in transform)
        {

            // j'instancie le prefab sur la position de l'enfant
            enemy = Instantiate(Resources.Load("Prefabs\\Entities\\Enemy\\Enemy"), child.transform.position, Quaternion.identity) as GameObject;
            // ensuite je place Enemy en tant qu'enfant de l'enfant de Enemy Formation
            enemy.transform.parent = child;

            // childPos = child.transform.position;
            // enemy = Instantiate(Resources.Load("Prefabs\\Entities\\Enemy\\Enemy"), new Vector3(10, 10, child.transform.position.z), Quaternion.identity) as GameObject;
            // StartCoroutine(Anim());
        }
    }
    
    IEnumerator RespawnTime(float time)
    {
        


        ParticleSystem ps = GameObject.Find("SpeedParticlesEffect").GetComponent<ParticleSystem>();
      //  ParticleSystem ps2 = GameObject.Find("SpeedParticlesEffect2").GetComponent<ParticleSystem>();
                       
        var emission = ps.emission;
      //  var emission2 = ps2.emission;

        emission.enabled = true;
        //   emission2.enabled = true;
        yield return new WaitForSeconds(time/2);
        NextWave = true;



        yield return new WaitForSeconds(time);

        emission.enabled = false;


        yield return new WaitForSeconds(time/4);

        SpawnUntilFull();
        NextWave = false;

        //   emission2.enabled = false;




        /*
       
        var main1 = starfield1.main;
        var main2 = starfield2.main;
        var emission1 = starfield1.emission;
        var emission2 = starfield2.emission;
        var trails1 = starfield1.trails;
        var trails2 = starfield2.trails;

        main1.startSpeed = 10f;
        main2.startSpeed = 5f;
        //main.startSize = 0.2f;
        //main2.startSize = 0.2f;
        emission1.rateOverTime = 120f;
        emission2.rateOverTime = 120f;
        trails1.enabled = true;
        trails2.enabled = true;
       // trails.widthOverTrail = 10f * Time.deltaTime;
       // trails2.widthOverTrail = 10f * Time.deltaTime;

 


        main1.startSpeed = 5f;
        main2.startSpeed = 1f;
        //main.startSize = 0.01f;
        //main2.startSize = 0.01f;
        emission1.rateOverTime = 60f;
        emission2.rateOverTime = 60f;
       // trails.widthOverTrail = 1f * -Time.deltaTime;
       // trails2.widthOverTrail = 1f * -Time.deltaTime;
        yield return new WaitForSeconds(1f);
        trails1.enabled = false;
        trails2.enabled = false;
        */

    }



}
