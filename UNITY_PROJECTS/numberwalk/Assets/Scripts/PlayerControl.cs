using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerControl : MonoBehaviour {

    public GameObject Tile;
    public int counter;
	// Use this for initialization
	void Start () {
	
	}

    void Move(Vector2 v)
    {
        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position + v, Vector2.zero);
        if (hit.collider == null)
        {
            transform.position = (Vector2)transform.position + v;
            counter++;
            GameObject go = Instantiate(Tile, transform.position, Quaternion.identity) as GameObject;
            go.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = counter.ToString();
        }
        
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.W))
        {
            Move(Vector2.up);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            Move(Vector2.left);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            Move(Vector2.down);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            Move(Vector2.right);
        }
        else if (Input.GetKey(KeyCode.R))
            Application.LoadLevel(0);

    }
}
