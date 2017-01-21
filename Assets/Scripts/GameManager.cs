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

    public AudioClip bgmWater;
    public AudioClip bgmAir;

    AudioSource waterSource;
    AudioSource airSource;

    enum STATECAMERA
    {
        UP,
        SWITCHING,
        DOWN
    }

    float desiredCameraY;
    STATECAMERA curCameraState = STATECAMERA.DOWN;

    public float elementsScrollingSpeed;
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

        waterSource = gameObject.AddComponent<AudioSource>();
        waterSource.clip = bgmWater;
        waterSource.volume = 1.0f;
        waterSource.Play();

        airSource = gameObject.AddComponent<AudioSource>();
        airSource.clip = bgmAir;
        airSource.volume = 0.0f;
        airSource.Play();


        foreach (GameSequence gs in sequences)
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

                    if(desiredCameraY < camera.transform.position.y)
                    {
                        airSource.volume = 1-ratioTimer;
                        waterSource.volume = ratioTimer;
                    }
                    else
                    {
                        airSource.volume = ratioTimer;
                        waterSource.volume = 1-ratioTimer;
                    }
                    
                    camera.transform.position = Vector2.Lerp(new Vector2(camera.transform.position.x, -desiredCameraY), new Vector2(camera.transform.position.x, desiredCameraY), quintInOut(ratioTimer));
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
        UpdateCamera();

        foreach(Transform t in movingObjectsContainer.transform)
        {
            t.position += Vector3.left * elementsScrollingSpeed;
        }
    }
}
