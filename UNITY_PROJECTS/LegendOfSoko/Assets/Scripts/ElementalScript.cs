using UnityEngine;
using System.Collections;

public class ElementalScript : MonoBehaviour {

    public GameObject Projectile;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Tower") && collision.collider.transform.childCount>0)
        {
            collision.collider.transform.GetChild(0).GetComponent<TowerScript>().Projectile = Projectile;
            Vector2 v = collision.collider.transform.position;
            v = new Vector2(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y));
            collision.collider.transform.position = v;
            collision.collider.GetComponent<SpriteRenderer>().color = GetComponent<SpriteRenderer>().color;
            collision.collider.transform.GetChild(1).GetComponent<SpriteRenderer>().color = GetComponent<SpriteRenderer>().color;
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
