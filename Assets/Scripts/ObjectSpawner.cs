using System.Collections;
using System;
using UnityEngine;

[Serializable]
public enum ObjectType
{
    O_OBSTRUCTION,
    O_OURSIN,
    O_ROCHER,
    O_GEYSER,
    O_ALGUES,
    O_NUAGE
}

[Serializable]
public class ObjectSpawner
{
    public Vector2 position;
    public ObjectType type;
    public GameObject ifObstructable;


    public ObjectSpawner(Vector2 _position, ObjectType _type)
    {
        position = _position;
        type = _type;
    }
}
