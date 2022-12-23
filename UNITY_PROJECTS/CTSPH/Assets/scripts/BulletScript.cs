using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {
    public float speed;
    public float lifeTime;
    public Vector2 direction;
    public int id;
    public int formNumber;

	// Use this for initialization
	void Start () {
  
	}
	
	// Update is called once per frame
	void Update () {

        lifeTime -= Time.deltaTime;
        if(lifeTime<0)
        {
            Destroy(gameObject);
        }
        transform.Translate(direction * Time.deltaTime * speed);
	
	}
}
