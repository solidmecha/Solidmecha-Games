using UnityEngine;
using System.Collections;

public class MoveScript : MonoBehaviour {

    public bool isMoving;
    public Vector3 target;
    public Vector3 StartPos;
    public float counter;
    public float Movetime;
	// Use this for initialization
	void Start () {
	
	}

    public void StopMove()
    {
        counter = 0;
        isMoving = false;
        transform.position = target;
    }

    public void StartMove(Vector3 Dest, float t)
    {
        isMoving = true;
        Movetime = t;
        target = Dest;
        StartPos = transform.position;
    }
	
	// Update is called once per frame
	void Update () {
	if(isMoving)
        {
            counter += Time.deltaTime;
            if (counter >= Movetime)
                StopMove();
            else
                transform.position = Vector3.Lerp(StartPos, target, counter / Movetime);
        }
	}
}
