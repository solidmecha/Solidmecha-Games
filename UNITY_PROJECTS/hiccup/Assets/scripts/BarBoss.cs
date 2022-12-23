using UnityEngine;
using System.Collections;

public class BarBoss : MonoBehaviour {

    public float speed;
    public float rotSpeed;
    public float rotStart;
    bool rotating;
    public float lifeTime;
    public float scaleSpeed;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(Vector2.down * speed * Time.deltaTime);
        if (rotStart!=0 && lifeTime <= rotStart)
        {
            if (scaleSpeed == 0)
            {
                transform.Rotate(Vector3.forward * rotSpeed * Time.deltaTime);
            }
            else
            {
                transform.GetChild(0).localScale = (Vector2)transform.GetChild(0).localScale + Vector2.one * scaleSpeed * Time.deltaTime;
                transform.GetChild(1).localScale = (Vector2)transform.GetChild(1).localScale + Vector2.one * scaleSpeed * Time.deltaTime;
            }
            if(!rotating)
            {
                speed = 0;
                rotating = true;
                lifeTime = 3;
            }
        }
        lifeTime -=Time.deltaTime;
        if(lifeTime<0)
        {
            Destroy(gameObject);
        }
	}
}
