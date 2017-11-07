using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    PlayerController player;
    Camera myCamera;
    Bounds cameraBounds;
    SpriteRenderer background;
    GameObject movingObjectsContainer;
    public List<GameSequence> sequences;

    public AudioClip bgmWater;
    public AudioClip bgmAir;

    AudioSource waterSource;
    AudioSource airSource;

    SoundManager sm;

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

		myCamera = Camera.main;
		if (myCamera == null)
        {
            Debug.Break();
            Debug.Log("Missing main camera");
        }

        movingObjectsContainer = GameObject.Find("objectContainer");
        if(movingObjectsContainer == null)
        {
            movingObjectsContainer = new GameObject("objectContainer");
        }

        waterSource = gameObject.AddComponent<AudioSource>();
        waterSource.clip = bgmWater;
        waterSource.volume = 1.0f;
        waterSource.Play();

        airSource = gameObject.AddComponent<AudioSource>();
        airSource.clip = bgmAir;
        airSource.volume = 0.0f;
        airSource.Play();

        sm = gameObject.GetComponent<SoundManager>();

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
                    waterSource.PlayOneShot(sm.diveIn);
                }
                break;
            case STATECAMERA.SWITCHING:
				if(myCamera.transform.position.y != desiredCameraY)
                {
                    transitionCamera -= Time.deltaTime;
                    float ratioTimer = 1.0f - transitionCamera / transitionTimer;

					if(desiredCameraY < myCamera.transform.position.y)
                    {
                        airSource.volume = 1-ratioTimer;
                        waterSource.volume = ratioTimer;
                    }
                    else
                    {
                        airSource.volume = ratioTimer;
                        waterSource.volume = 1-ratioTimer;
                    }
					Vector3 desiredCameraPos = myCamera.transform.position;
					desiredCameraPos.y = desiredCameraY;
					myCamera.transform.position = Vector3.Lerp(myCamera.transform.position, desiredCameraPos, quintInOut(ratioTimer));
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
                    waterSource.PlayOneShot(sm.jumpOut);
                    transitionCamera = transitionTimer;
                }
                break;
        }
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
