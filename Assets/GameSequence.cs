using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSequence : ScriptableObject
{
    private List<ObjectSpawner> spawners;
    public int nbScreens;

    public List<ObjectSpawner> Spawners
    {
        get
        {
            return spawners;
        }
        set
        {
            spawners = value;
        }
    }

    public GameSequence()
    {
        Spawners = new List<ObjectSpawner>();
    }

    public void AddSpawner(ObjectSpawner Spawner)
    {
        Spawners.Add(Spawner);
    }

    public void RemoveSpawner(ObjectSpawner Spawner)
    {
        Spawners.Remove(Spawner);
    }
    
    public void ClearSpawners()
    {
        Spawners.Clear();
    }
}
