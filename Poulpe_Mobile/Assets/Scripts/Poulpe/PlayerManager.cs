using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private float JumpForce;

    [SerializeField]
    private Transform rayStartingPoint;

    [SerializeField]
    private float limitRotation;

    [SerializeField]
    [HideInInspector]
    private bool isOnWall = false;  
    
    [SerializeField]
    private float distanceClicOnPlayer;

    [SerializeField]
    private Transform boneToRotate;

    [SerializeField]
    private float distanceStretch;

    [System.Serializable]
    public class GameEvents
    {
        public GameEvent jump;
        public GameEvent PlayerIsSelected;
        public GameEvent PlayerNotSelected;
        public GameEvent StickOnWall;
        public GameEvent StickToPlatform;
        public GameEvent Restart;
        public GameEvent Win;
        public GameEvent Collected;
    }
    public GameEvents gameEvents;
    
    private Vector3 mousePos;
    private Vector3 direction;
    private Vector3 defaultStickyUp;//donne l'orientation par défaut sur une surface, permet de clamp la rotation
    private Vector3 initialSize;

    private RaycastHit2D hit;
    private Ray2D ray;

    private Rigidbody2D rb;

    private bool playerIsSelected;
    private bool isStretched;

    private int numberOfFingerDown = 0;

    [HideInInspector]
    public bool overrideSelectionTuto; //modifié par le tutoManager


    public float GetdistanceClicOnPlayer()
    {
        return distanceClicOnPlayer;
    }

    // Start is called before the first frame update
    void Start()
    {
        overrideSelectionTuto = true;
        rb = GetComponent<Rigidbody2D>();

        rb.AddForce(transform.up * JumpForce);//on stick le poulpe au premier mur qu'il regarde

        initialSize = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isOnWall)
        {
            rotateTowardVelocity();
        }

        if (!overrideSelectionTuto)
            return;

#if UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0) // petit problème car on peut bouger la caméra avec un doigt au lieu de déplacer le joueur...
        {
            foreach(Touch T in Input.touches)
            {
                if (T.phase == TouchPhase.Moved && isOnWall && playerIsSelected)
                {
                    PrepareJump();

                }
                if (T.phase == TouchPhase.Began && isOnWall)
                {
                    SelectPlayer(true, true); //si le joueur appuie sur le poulpe on le sélectionne
                    numberOfFingerDown++;
                }
                if (T.phase == TouchPhase.Ended && playerIsSelected && isOnWall && numberOfFingerDown == 1)
                {
                    if (isStretched)
                    {
                        RotateTowardPoint(direction.x, direction.y, false, transform, true); // on se tourne dans la direction du doigt
                        JumpTowardMouse();// ajout de la force dans cette direction
                        gameEvents.jump.Raise();
                    }
                    SelectPlayer(false, false); // déselection du personnage (et déstretch)
                }
                if(T.phase == TouchPhase.Ended)
                {
                    numberOfFingerDown--;
                    if (numberOfFingerDown < 0)
                        numberOfFingerDown = 0;
                }
            }
        }
#endif

#if UNITY_EDITOR

        

        if (Input.GetMouseButton(0) && isOnWall && playerIsSelected)//on cherche la direction du saut (rotate poulpe vers point à chaque frame)
        {
            PrepareJump();
        }

        if (Input.GetMouseButtonDown(0) && isOnWall)//le premier appui sur le joueur
        {
            SelectPlayer(true,true); //si le joueur appuie sur le poulpe on le sélectionne
        }
        
        if (Input.GetMouseButtonUp(0) && playerIsSelected && isOnWall)//on saute vers ce point quand on relache le doigt
        {
            if (isStretched)
            {
                RotateTowardPoint(direction.x, direction.y, false, transform, true); // on se tourne dans la direction du doigt
                JumpTowardMouse();// ajout de la force dans cette direction
                gameEvents.jump.Raise();
            }
            SelectPlayer(false,false); // déselection du personnage (et déstretch) 
        }
#endif


       
    }


    private void rotateTowardVelocity()
    {
        Vector2 v = rb.velocity;
        float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg -90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    //gère la (dé)selection du poulpe par le joueur et le stretch du personnage
    public void SelectPlayer(bool select,bool useDistanceCheck)
    {
        if (select)
        {
            Debug.Log("inside Select");

            mousePos = Input.mousePosition;//position de souris sur écran
            mousePos = Camera.main.ScreenToWorldPoint(mousePos); //position souris dans le jeu
            mousePos.z = 0; //on set z=0 au cas où car 2D
            float distance = Vector3.Distance(mousePos, transform.position);

            //Debug.Log(distance);
            if (distance < distanceClicOnPlayer && useDistanceCheck)
            {
                Stretch(true);
                playerIsSelected = true;
                gameEvents.PlayerIsSelected.Raise();
            }
            else if(!useDistanceCheck) 
            {
                Stretch(true);
                playerIsSelected = true;
                gameEvents.PlayerIsSelected.Raise();
            }
        }
        else
        {
            gameEvents.PlayerNotSelected.Raise();
            playerIsSelected = false;
            Stretch(false);
        }
    }

    //étire ou pas le poulpe dans la direction transform
    public void Stretch(bool stretch)
    {
        if (stretch)
        {
            if (!isStretched)
            {
                Debug.Log("stretch");
                boneToRotate.position += transform.up * distanceStretch;
            }
            isStretched = true;
        }
        else
        {
            isStretched = false;
            boneToRotate.localEulerAngles = Vector3.zero;
            boneToRotate.localPosition = Vector3.zero;
            Debug.Log("stop stretch");
        }
    }


    //SI LE POULPE EST SELECTIONNE
    public void PrepareJump()
    {
        mousePos = Input.mousePosition;//position de souris sur écran
        mousePos = Camera.main.ScreenToWorldPoint(mousePos); //position souris dans le jeu
        mousePos.z = 0; //on set z=0 au cas où car 2D
        direction = mousePos - transform.position; //calcul de la direction à partir du poulpe
        RotateTowardPoint(direction.x, direction.y, true, boneToRotate, false); //on se tourne dans cette direction
    }

    //ajout de force dans la direction
    public void JumpTowardMouse()
    {
        isOnWall = false;
        ChangeParent(null);
        Vector2 direction2D = new Vector2(direction.x, direction.y).normalized; // on donne une direction normalisée pour que la vitesse du saut soit toujours la même

        rb.AddForce(transform.up * JumpForce);

        CastNewRay();
    }

    //Casts a ray to get the direction and a calculate a hit to get the collider normal
    private void CastNewRay()
    {
        ray = new Ray2D(rayStartingPoint.position, transform.up);
        hit = Physics2D.Raycast(rayStartingPoint.position, transform.up);
    }

    //se tourne vers une direction / un point
    private void RotateTowardPoint(float x,float y,bool clamp,Transform monTransform, bool offset)
    {
        float angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;

        if (offset)
        {
            angle -= 90;
        }

        //on calcule le produit du vecteur perpendiculaire à la surface et celui dans l'axe de la surface depuis la la position de la souris
        //float crossProduct = Vector3.Cross(new Vector3(defaultStickyUp.y, -defaultStickyUp.x, 0), new Vector3(x, y, 0) + defaultStickyUp).z;

        //if (clamp)
        //{
        //    if(crossProduct - limitRotation > 0)
        //    {
        //        monTransform.rotation = Quaternion.AngleAxis(angle, transform.forward);
        //        SelectPlayer(true,false);//si le joueur était déja sélectionné et qu'on "perd le tracking" en pointant le doigt dans un endroit impossible à atteindre, on utilise pas la distance de check
        //        Debug.Log("try select");
        //    }
        //    else
        //    {
        //        Stretch(false);
        //    }
        //}
        //else
        //{
            monTransform.rotation = Quaternion.AngleAxis(angle, transform.forward);
        //}
    }

    //calcul du vecteur de réflection à partir de la direction et de la normal du collider puis tourne vers le nouveau point
    private void ReversrRotationBounce()
    {
        //if (hit.collider != null)
        //{
        //    Vector2 reflectionVector2D = Vector2.Reflect(ray.direction, hit.normal);
           
        //    Debug.DrawRay(hit.point, hit.normal, Color.red, 3f);
        //    RotateTowardPoint(reflectionVector2D.x, reflectionVector2D.y,false,transform,true);

        //    CastNewRay();
        //}

       

    }

    private void ChangeParent(Transform newParent)
    {
        transform.parent = newParent;
        if(newParent == null)
        {
            transform.localScale = initialSize;
        }
    }

    //quand on touche un ennemi, redémarre le niveau mais garde le GameManager
    private void TakeDamage()
    {
        gameEvents.Restart.Raise();
    }

    //S'agrippe à un mur peu importe son orientation
    private void StickOnWall(Vector3 stickDirection)
    {
        gameEvents.StickOnWall.Raise();
        isOnWall = true;
        defaultStickyUp = stickDirection;
        rb.velocity = new Vector3(0, 0, 0);
        RotateTowardPoint(stickDirection.x,stickDirection.y,false,transform,true);
    }

    private void Win()
    {
        gameEvents.Win.Raise();
        rb.velocity = new Vector3(0, 0, 0);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.tag == "Wall"|| collision.gameObject.tag == "Platform") && !isOnWall)
        {
            StickOnWall(collision.contacts[0].normal);
            ChangeParent(collision.transform);
            if(collision.gameObject.tag == "Platform")
            {
                gameEvents.StickToPlatform.Raise();
            }
        }
        else if (collision.gameObject.tag == "Ennemi")
        {
            TakeDamage();
        }else if(collision.gameObject.tag == "Bouncy")
        {
            //ReversrRotationBounce();
            //géré par la vélocité, marche mieux
        }
        else if(collision.gameObject.tag == "Win")
        {  
            Win();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Collectible")
        {
            gameEvents.Collected.Raise();
            Destroy(collision.gameObject);
        }
    }
}
