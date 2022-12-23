using UnityEngine;
using System.Collections.Generic;

public class WorldSpawnerScript : MonoBehaviour {

    public List<Vector2> PossibleAnvilLocs;
    public List<Vector2> AnvilLocs;
    public Vector2[] CrysLocs = new Vector2[4];

    // Use this for initialization
    void Start () {
        SpawnCrystals();
        SpawnAnvils();

        //testing
        //SpawnEnemyCrystals();
	}

    void SpawnCrystals()
    {
        int x = WorldControl.singleton.RNG.Next(2);
        int y = WorldControl.singleton.RNG.Next(3);
        CrysLocs[0] = new Vector2(17.14f * x, 9.87f * y);
        x = WorldControl.singleton.RNG.Next(2);
        y = WorldControl.singleton.RNG.Next(3, 5);
        CrysLocs[1] = new Vector2(17.14f * x, 9.87f * y);
        x = WorldControl.singleton.RNG.Next(2, 4);
        y = WorldControl.singleton.RNG.Next(3);
        CrysLocs[2] = new Vector2(17.14f * x, 9.87f * y);
        x = WorldControl.singleton.RNG.Next(2, 4);
        y = WorldControl.singleton.RNG.Next(3, 5);
        CrysLocs[3] = new Vector2(17.14f * x, 9.87f * y);
        for(int i=0;i<4;i++)
        {
            CrystalSpawner c=(Instantiate(WorldControl.singleton.Spawners[0], CrysLocs[i], Quaternion.identity) as GameObject).GetComponent<CrystalSpawner>();
            c.ID = i+1;
        }
    }

    bool checkspawn(int i, int x, int y)
    {
        return ((new Vector2(17.14f * x, 9.87f * y) - CrysLocs[i]).sqrMagnitude < 1f);
    }

    public void SpawnEnemyCrystals()
    {
        int x = WorldControl.singleton.RNG.Next(2);
        int y = WorldControl.singleton.RNG.Next(3);
        while(checkspawn(0, x, y))
        {
            x = WorldControl.singleton.RNG.Next(2);
            y = WorldControl.singleton.RNG.Next(3);
        }
        WorldControl.singleton.EnemyCrystalSpawns[0] = new Vector2(17.14f * x, 9.87f * y);
        x = WorldControl.singleton.RNG.Next(2);
        y = WorldControl.singleton.RNG.Next(3, 5);
        while (checkspawn(1, x, y))
        {
            x = WorldControl.singleton.RNG.Next(2);
            y = WorldControl.singleton.RNG.Next(3, 5);
        }
        WorldControl.singleton.EnemyCrystalSpawns[1] = new Vector2(17.14f * x, 9.87f * y);
        x = WorldControl.singleton.RNG.Next(2, 4);
        y = WorldControl.singleton.RNG.Next(3);
        while (checkspawn(2, x, y))
        {
            x = WorldControl.singleton.RNG.Next(2, 4);
            y = WorldControl.singleton.RNG.Next(3);
        }
        WorldControl.singleton.EnemyCrystalSpawns[2] = new Vector2(17.14f * x, 9.87f * y);
        x = WorldControl.singleton.RNG.Next(2, 4);
        y = WorldControl.singleton.RNG.Next(3, 5);
        while (checkspawn(3, x, y))
        {
            x = WorldControl.singleton.RNG.Next(2, 4);
            y = WorldControl.singleton.RNG.Next(3, 5);
        }
        WorldControl.singleton.EnemyCrystalSpawns[3] = new Vector2(17.14f * x, 9.87f * y);
        foreach (Vector2 v in WorldControl.singleton.EnemyCrystalSpawns)
        {
           CrystalSpawner cs= (Instantiate(WorldControl.singleton.Spawners[0], v, Quaternion.identity)as GameObject).GetComponent<CrystalSpawner>();
            cs.hostile = true;
        }
        WorldControl.singleton.ShowMessage("Destroy the corruption.", 3f);
        WorldControl.singleton.Invoke("StartSpawningEnemies", .5f);
    }

    void SpawnAnvils()
    {
        for (int i = 0; i < 6; i++)
        {
            if (PossibleAnvilLocs.Count > 0)
            {
                int r = WorldControl.singleton.RNG.Next(PossibleAnvilLocs.Count);
                Instantiate(WorldControl.singleton.Spawners[1], PossibleAnvilLocs[r], Quaternion.identity);
                AnvilLocs.Add(PossibleAnvilLocs[r]);
                PossibleAnvilLocs.RemoveAt(r);
            }
        }
        PossibleAnvilLocs.Clear();
    }

    // Update is called once per frame
    void Update () {
	
	}
}
