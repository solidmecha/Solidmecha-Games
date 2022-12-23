using UnityEngine;
using System.Collections;

public class ResourceScript : MonoBehaviour {

    public int Index;
    public GameControl GC;
    public float lifeTime;


    void OnCollisionEnter2D(Collision2D other)
    {
        if(Index==0)
            GC.ResourceAmount[Index]+=5;
        else
            GC.ResourceAmount[Index]+=25;
        GC.UpdateGUI();
        Destroy(gameObject);
    }

	// Use this for initialization
	void Start () {
        GC = (GameControl)GameObject.Find("GameControlOBJ").GetComponent(typeof(GameControl));
	
	}
	
	// Update is called once per frame
	void Update () {
        lifeTime -= Time.deltaTime;
        if(lifeTime<=0)
        { Destroy(gameObject); }
	
	}
}
