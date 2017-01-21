using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerHelper : MonoBehaviour
{

    [HideInInspector]
    public ObjectType type;
    [HideInInspector]
    public Vector2 position;
    private GameSequence papa;
    public GameObject ifObstructable;

    public void init(Vector2 _position, ObjectType _type)
    {
        position = _position;
        type = _type;
    }
    

    public void OnDestroy()
    {

    }
}
