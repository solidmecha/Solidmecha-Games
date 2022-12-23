using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour {

    public GameObject Player;
    public float speed;
   public bool moveLeft;
    public bool moveRight;
   public bool moveUp;
    public bool moveDown;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Player.transform.position.x - transform.position.x > 4)
        {
            moveRight = true;
        }
        else if(Player.transform.position.x - transform.position.x <2)
            moveRight = false;
        if (Player.transform.position.x - transform.position.x  < -4)
            moveLeft = true;
        else if (Player.transform.position.x- transform.position.x > -2)
            moveLeft = false;
        if (Player.transform.position.y - transform.position.y > 4)
            moveUp = true;
        else if (Player.transform.position.y - transform.position.y < 2)
            moveUp = false;
        if (Player.transform.position.y -transform.position.y  < -4)
            moveDown = true;
        else if (Player.transform.position.y - transform.position.y  > -2)
            moveDown = false;

        if (moveRight)
            transform.Translate(Vector2.right * Time.deltaTime*speed);
        if(moveLeft)
            transform.Translate(Vector2.left * Time.deltaTime * speed);
        if(moveUp)
            transform.Translate(Vector2.up * Time.deltaTime * speed);
        if(moveDown)
            transform.Translate(Vector2.down * Time.deltaTime * speed);
    }
}
