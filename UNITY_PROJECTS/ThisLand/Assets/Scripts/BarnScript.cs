using UnityEngine;
using System.Collections;

public class BarnScript : MonoBehaviour {

    public float Delay;
    float counter;
    GameControl GC;
    
    void SpawnNatRes()
    {      
            Vector2 Pos = transform.position - GC.transform.position;
            Pos = new Vector2(Mathf.Round(Pos.x), Mathf.Round(Pos.y));
        if (GC.World[(int)Pos.x][(int)Pos.y].NaturalRes.Count < 8)
        {
            float Xoff = GC.RNG.Next(-24, 25);
            float Yoff = GC.RNG.Next(-18, 17);
            Xoff = Xoff / 100f;
            Yoff = Yoff / 100f;
            GameObject Res = Instantiate(GC.ResourceList[GC.RNG.Next(GC.ResourceList.Count)], (Vector2)transform.position + new Vector2(Xoff, Yoff), Quaternion.identity) as GameObject;
            Res.transform.SetParent(GC.World[(int)Pos.x][(int)Pos.y].transform);
            GC.World[(int)Pos.x][(int)Pos.y].NaturalRes.Add(Res);
        }
    }
    

	// Use this for initialization
	void Start () {
        GC = (GameControl)GameObject.Find("GameControlOBJ").GetComponent(typeof(GameControl));
    }
	
	// Update is called once per frame
	void Update () {
        counter += Time.deltaTime;
        if (counter >= Delay)
        {
            SpawnNatRes();
            counter = 0;
        }
	
	}
}
