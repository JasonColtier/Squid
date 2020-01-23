using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{

    [SerializeField]
    private Transform player;

    [SerializeField]
    private GameObject BoundsCamera;

    [SerializeField]
    private float distance;

    [SerializeField]
    private float dragSpeed;

    [SerializeField]
    private float cameraFollowSpeed;

    [SerializeField]
    private float zoomSize;

    [SerializeField]
    private float zoomSpeed;

    [System.Serializable]
    public class GameEvents
    {
        public GameEvent isPanningCamera;
    
    }
    public GameEvents gameEvents;

    private PlayerManager playerManager;

    private bool followPlayer;
    private bool lockOnGoal;
    private bool stickToPlatform;
    public bool pan;
    private bool distanceSelectCameraOK;
    private bool zoom;


    //to move the camera
    private Vector3 mousePos;
    private Vector3 oldPos;
    private Vector3 panOrigin;

    //bounds
    private Vector3 posTopLeftBound;
    private Vector3 posBotRightBound;

    private float originalZoom;

    private Camera cam;

    private GameObject palourdeWin; // récupéré dans le start par son nom

    [HideInInspector]
    public bool overridePanTuto;

    private float distanceCheck;

    // Start is called before the first frame update
    void Start()
    {


        //bounds
        posTopLeftBound = BoundsCamera.transform.Find("TopLeft").transform.position; 
        posBotRightBound = BoundsCamera.transform.Find("BotRight").transform.position; 

         overridePanTuto = true; // le défaut si c'est pas modifié par le tuto
        playerManager = player.GetComponent<PlayerManager>();
        distanceCheck = playerManager.GetdistanceClicOnPlayer();
        cam = GetComponent<Camera>();
        pan = true;
        followPlayer = false;
        transform.position = new Vector3(player.position.x, player.position.y, distance);
        originalZoom = cam.orthographicSize;

        palourdeWin = GameObject.FindGameObjectWithTag("Win");
    }

    // Update is called once per frame
    void Update()
    {
        if(pan && overridePanTuto)
        {

            if (Input.GetMouseButtonDown(0))
            {
                mousePos = Input.mousePosition;//position de souris sur écran
                mousePos = Camera.main.ScreenToWorldPoint(mousePos); //position souris dans le jeu
                mousePos.z = 0; //on set z=0 au cas où car 2D
             

                if (Vector3.Distance(mousePos, player.position) > distanceCheck)
                {
                    gameEvents.isPanningCamera.Raise();
                    oldPos = transform.position;
                    panOrigin = Camera.main.ScreenToViewportPoint(Input.mousePosition);
                    distanceSelectCameraOK = true;
                }
                else
                {
                    distanceSelectCameraOK = false;
                }

                
            }
            if (Input.GetMouseButton(0))
            {
                if(distanceSelectCameraOK)
                    PanCamera();
            }
        }

        if (zoom)
        {
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, zoomSize, Time.deltaTime * zoomSpeed);
        }
        else
        {
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, originalZoom, Time.deltaTime * zoomSpeed);
        }

        if (stickToPlatform)
        {
            LockOnTarget(player);
        }
       
        
    }

    private void FixedUpdate()
    {
        if (followPlayer)
        {
            LockOnTarget(player);
        }
        if (lockOnGoal)
        {
            LockOnTarget(palourdeWin.transform);
        }
    }

    private void LockOnTarget(Transform targetToLock)
    {
        Vector3 target = new Vector3(targetToLock.position.x, targetToLock.position.y, distance);

        transform.position = Vector3.Lerp(transform.position, target, cameraFollowSpeed * Time.deltaTime);
    }

    private void PanCamera()
    {

        followPlayer = false;
        stickToPlatform = false;

        Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition) - panOrigin;
        if (InNewPosInBounds(oldPos -pos * dragSpeed, 5))
        {
            transform.position = oldPos -pos * dragSpeed;
        }
        else
        {
            oldPos = transform.position;
            panOrigin = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        }    
        transform.position = new Vector3(transform.position.x, transform.position.y, distance);
    }

    private bool InNewPosInBounds(Vector3 pos, float margin)
    {

        bool isInBounds = true;
        if (pos.x + margin > posBotRightBound.x)
        {
            Debug.Log("OUT OF BOUNDS posBotRightBound X");
            isInBounds = false;
        }
        if (pos.y - margin < posBotRightBound.y)
        {
            Debug.Log("OUT OF BOUNDS posBotRightBound.y");
            isInBounds = false;
        }
        if (pos.x - margin < posTopLeftBound.x)
        {
            Debug.Log("OUT OF BOUNDS posTopLeftBound.x");
            isInBounds = false;
        }
        if (pos.y + margin > posTopLeftBound.y)
        {
            Debug.Log("OUT OF BOUNDS  posTopLeftBound.y");
            isInBounds = false;
        }

        return isInBounds;
    }


    public void FollowPlayer()
    {
        GoBackToPlayer();
        DisablePan();
    }

    public void GoBackToPlayer()
    {
        followPlayer = true;
        StopZoom();
    }

    public void StopZoom()
    {
        zoom = false;
    }

    public void FocusOnPlayer()
    {
        pan = false;
        zoom = true;
    }

    public void StopFocus()
    {
        followPlayer = false;
        stickToPlatform = false;
    }

    public void EnablePan()
    {
        pan = true;
    }

    public void DisablePan()
    {
        pan = false;
    }

    public void StickOnPlatform()
    {
        stickToPlatform = true;
    }

    public void FocusOnGoal()
    {
        lockOnGoal = true;
    }

}
