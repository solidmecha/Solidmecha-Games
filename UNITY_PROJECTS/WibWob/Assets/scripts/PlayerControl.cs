using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

    public GameControl GC;
    public float speed;
    public bool move;
    public bool rotate;
    public Vector2 pos;
    public Vector2 TempPos;
    public float RotDir;
    public float RotationTarget;
	// Use this for initialization
	void Start () {
	
	}
	
    public Vector2 FindPos(Vector2 V)
    {
        return new Vector2(Mathf.Round(V.x), Mathf.Round(V.y)) + (Vector2)GC.transform.position;
    }
	// Update is called once per frame
	void Update () {
        if(!move && !rotate && Input.GetMouseButtonDown(0))
        {
          Vector2 V = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            V -= (Vector2)GC.transform.position;
            pos = FindPos(V);
            move = (Mathf.Round(pos.x-transform.position.x)==0 || Mathf.Round(pos.y-transform.position.y)==0);
        }
        if(move)
        {
            float d1 = Vector2.Distance(transform.position, pos);
            Vector2 v=pos - (Vector2)transform.position;
            transform.Translate(v.normalized * speed * Time.deltaTime);
            float d2= Vector2.Distance(transform.position, pos);
            if(d2>=d1)
            {
                move = false;
                transform.position = pos;
            }
        }
        if(rotate && !move)
        {
            transform.GetChild(0).localEulerAngles = new Vector3(0, 0, transform.GetChild(0).localEulerAngles.z + 30*speed*RotDir * Time.deltaTime);
            if (Mathf.Round(transform.GetChild(0).localEulerAngles.z) >= RotationTarget-5 && Mathf.Round(transform.GetChild(0).localEulerAngles.z)<=RotationTarget+5)
            {
                transform.GetChild(0).localEulerAngles = new Vector3(0, 0, RotationTarget);
                pos = TempPos;
                move = pos.x != transform.position.x || pos.y != transform.position.y;
                rotate = false;
            }
         }
	}
}
