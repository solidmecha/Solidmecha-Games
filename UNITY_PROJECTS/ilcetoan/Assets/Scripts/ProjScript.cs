using UnityEngine;
using System.Collections;

public class ProjScript : MonoBehaviour {

    public float speed;
    public Vector2 dir;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("B"))
        {
            Destroy(other.gameObject);
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>().UpdateScore(-5);
        }
        if(!other.CompareTag("Player"))
            Destroy(gameObject);
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        transform.Translate(speed * dir * Time.deltaTime);
	}
}
