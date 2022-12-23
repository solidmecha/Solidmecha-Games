using UnityEngine;
using System.Collections;

public class MessageControl : MonoBehaviour {

    public float Speed;
    public Vector2 Target;
    public float TravelTime;
    float counter;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("M"))
            print("Game Over: "+transform.position.x.ToString()+" , "+ transform.position.y.ToString());
    }

    // Use this for initialization
    void Start () {
        TravelTime = (Target - (Vector2)transform.position).magnitude / Speed;
    }
	
	// Update is called once per frame
	void Update () {
        transform.Translate(Vector2.right * Time.deltaTime * Speed);
        counter += Time.deltaTime;
        if (TravelTime <= counter)
            Destroy(gameObject);

	}
}
