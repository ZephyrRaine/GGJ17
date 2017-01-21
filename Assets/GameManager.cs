using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    PlayerController player;
    Camera camera;
    Bounds cameraBounds;
    SpriteRenderer background;
    GameObject movingObjectsContainer;
    public List<GameSequence> sequences;
    enum STATECAMERA
    {
        UP,
        SWITCHING,
        DOWN
    }

    float desiredCameraY;
    STATECAMERA curCameraState = STATECAMERA.DOWN;

    public float scrollingSpeed;
    public float transitionTimer = 0.5f;
    float transitionCamera;
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        if (player == null)
        {
            player = new GameObject("Player", typeof(PlayerController)).GetComponent<PlayerController>();
        }

        camera = Camera.main;
        if (camera == null)
        {
            Debug.Break();
            Debug.Log("help");
        }

        movingObjectsContainer = new GameObject("objectsContainer");
        
        foreach(GameSequence gs in sequences)
        {
            foreach(ObjectSpawner o in gs.Spawners)
            {
                GameObject go;
                switch (o.type)
                {
                    case ObjectType.O_OBSTRUCTION:
                        if (o.ifObstructable != null)
                            go = Instantiate(o.ifObstructable);
                        else
                            go = Instantiate(Resources.Load("GameElements/Obstruction") as GameObject);
                        break;
                    case ObjectType.O_OURSIN:
                        go = Instantiate(Resources.Load("GameElements/Oursin") as GameObject);
                        break;
                    case ObjectType.O_ROCHER:
                        go = Instantiate(Resources.Load("GameElements/Rocher") as GameObject);
                        break;
                    case ObjectType.O_GEYSER:
                        go = Instantiate(Resources.Load("GameElements/Geyser") as GameObject);
                        break;
                    case ObjectType.O_ALGUES:
                        go = Instantiate(Resources.Load("GameElements/Algues") as GameObject);
                        break;
                    case ObjectType.O_NUAGE:
                        go = Instantiate(Resources.Load("GameElements/Nuage") as GameObject);
                        break;
                    default:
                        go = null;
                        break;
                }
                if (go != null)
                {
                    go.transform.parent = movingObjectsContainer.transform;
                    go.transform.position = o.position;
                }
            }
        }

    }
    public static float quintInOut(float t)
	{
		return ((t *= 2) < 1) ? (t* t * t* t * t) / 2 : ((t -= 2) * t* t * t* t + 2) / 2;

    }
    void UpdateCamera()
    {
        switch (curCameraState)
        {
            case STATECAMERA.UP:
                if(player.transform.position.y <= 0)
                {
                    desiredCameraY = -3.8f;
                    curCameraState = STATECAMERA.SWITCHING;
                    transitionCamera = transitionTimer;
                }
                break;
            case STATECAMERA.SWITCHING:
                if(camera.transform.position.y != desiredCameraY)
                {
                    transitionCamera -= Time.deltaTime;
                    float ratioTimer = 1.0f - transitionCamera / transitionTimer;

                    camera.transform.position = Vector2.Lerp(new Vector2(camera.transform.position.x, -desiredCameraY), new Vector2(camera.transform.position.x, desiredCameraY), quintInOut(ratioTimer)   );
                }
                else
                {
                    if (desiredCameraY == 3.8f) curCameraState = STATECAMERA.UP;
                    if (desiredCameraY == -3.8f) curCameraState = STATECAMERA.DOWN;
                }
                break;
            case STATECAMERA.DOWN:
                if(player.transform.position.y >= 0)
                {
                    desiredCameraY = 3.8f;
                    curCameraState = STATECAMERA.SWITCHING;
                    transitionCamera = transitionTimer;

                }
                break;
            default:
                break;
        }
		
        //camera.transform.position = new Vector2(player.transform.position.x, camera.transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 tar = new Vector3(0, 0, -10);

        UpdateCamera();

        foreach(Transform t in movingObjectsContainer.transform)
        {
            t.position += Vector3.left * scrollingSpeed;
        }

        if(!player.colliding) player.transform.position += Vector3.right * scrollingSpeed;
        /*
        tar.x = Mathf.Lerp(background.bounds.min.x, background.bounds.max.x, Mathf.InverseLerp(0, Screen.width, Input.mousePosition.x));
        tar.y = Mathf.Lerp(background.bounds.min.y, background.bounds.max.y, Mathf.InverseLerp(0, Screen.height, Input.mousePosition.y));
        Camera.main.transform.position = tar;

        if (Camera.main.pixelRect.xMin < Camera.main.WorldToScreenPoint(cameraBounds.min).x) tar.x = Camera.main.ScreenToWorldPoint(Camera.main.WorldToScreenPoint(cameraBounds.min) + new Vector3(Camera.main.pixelWidth / 2, 0)).x;
        if (Camera.main.pixelRect.xMax > Camera.main.WorldToScreenPoint(cameraBounds.max).x) tar.x = Camera.main.ScreenToWorldPoint(Camera.main.WorldToScreenPoint(cameraBounds.max) - new Vector3(Camera.main.pixelWidth / 2, 0)).x;

        if (Camera.main.pixelRect.yMin < Camera.main.WorldToScreenPoint(cameraBounds.min).y) tar.y = Camera.main.ScreenToWorldPoint(Camera.main.WorldToScreenPoint(cameraBounds.min) + new Vector3(0, Camera.main.pixelHeight / 2)).y;
        if (Camera.main.pixelRect.yMax > Camera.main.WorldToScreenPoint(cameraBounds.max).y) tar.y = Camera.main.ScreenToWorldPoint(Camera.main.WorldToScreenPoint(cameraBounds.max) - new Vector3(0, Camera.main.pixelHeight / 2)).y;

        Camera.main.transform.position = tar;*/
    }
}
