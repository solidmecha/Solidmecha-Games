using UnityEngine;
using System.Collections;

public class MoveScript : MonoBehaviour {

    private void OnMouseOver()
    {
        GameObject Go=GameObject.FindGameObjectWithTag("Player");
        if (Vector2.Distance(Go.transform.position, transform.position) <= 1.01f)
        {
            Vector2 v = transform.position-Go.transform.position;
            if (v.x == 0 & v.y == 1)
                Go.GetComponent<PlayerControl>().moveUp = true;
            else if (v.x == 0 & v.y == -1)
                Go.GetComponent<PlayerControl>().moveDown = true;
            else if (v.x == 1 & v.y == 0)
                Go.GetComponent<PlayerControl>().moveRight = true;
            else if (v.x == -1 & v.y == 0)
                Go.GetComponent<PlayerControl>().moveLeft = true;
        }
    }
    

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
