using UnityEngine;
using System.Collections.Generic;

public class RoomScript : MonoBehaviour {


	// Use this for initialization
	void Start () {
        PlacePlatforms();
        Destroy(this);
	}

    public void PlacePlatforms()
    {
        bool AnvilPossible=false;
        int count = WorldControl.singleton.RNG.Next(5, 20);
        for (int i = 0; i < count; i++)
        {
            List<Vector3> PossibleSpikes = new List<Vector3>();
            float y = 1.5f * WorldControl.singleton.RNG.Next(3);
            y *= WorldControl.singleton.RNG.Next(-1, 2);
            y += transform.position.y;
            float x = transform.position.x+WorldControl.singleton.RNG.Next(-6, 7);
            Transform T=(Instantiate(WorldControl.singleton.Platform, new Vector2(x, y), Quaternion.identity) as GameObject).transform;
            PossibleSpikes.Add(new Vector3(x - .577f, y + .3f, 0));
            PossibleSpikes.Add(new Vector3(x + .2885f, y + .3f, 0));
            PossibleSpikes.Add(new Vector3(x, y + .3f, 0));
            PossibleSpikes.Add(new Vector3(x + .577f, y + .3f, 0));
            PossibleSpikes.Add(new Vector3(x - .2885f, y + .3f, 0));
            PossibleSpikes.Add(new Vector3(x - .577f, y - .3f, 180));
            PossibleSpikes.Add(new Vector3(x + .2885f, y - .3f, 180));
            PossibleSpikes.Add(new Vector3(x, y - .3f, 180));
            PossibleSpikes.Add(new Vector3(x + .577f, y - .3f, 180));
            PossibleSpikes.Add(new Vector3(x - .2885f, y - .3f, 180));
            PossibleSpikes.Add(new Vector3(x - .88f, y - .025f, 90));
            PossibleSpikes.Add(new Vector3(x + .88f, y - .025f, -90));
            foreach (Vector3 v in PossibleSpikes)
            {
                if (WorldControl.singleton.RNG.Next(100) <= 34)
                    Instantiate(WorldControl.singleton.Spikes, new Vector2(v.x, v.y), Quaternion.Euler(new Vector3(0, 0, v.z)), T);
                else if (WorldControl.singleton.RNG.Next(14) == 4)
                    DropControl.singleton.RandomDrop(new Vector2(v.x, v.y));
                else if(v.z==0 && WorldControl.singleton.RNG.Next(5)==4 && !AnvilPossible)
                {
                    AnvilPossible = true;
                    WorldControl.singleton.GetComponent<WorldSpawnerScript>().PossibleAnvilLocs.Add(v);
                }
            }

        }

    }

    // Update is called once per frame
    void Update () {
	
	}
}
