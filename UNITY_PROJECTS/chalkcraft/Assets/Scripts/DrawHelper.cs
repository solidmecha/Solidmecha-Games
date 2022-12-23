using UnityEngine;
using System.Collections;

public class DrawHelper : MonoBehaviour {

    public float DrawTime;
    public GameObject Chalkling;

    void OnMouseOver()
    {
        if(Input.GetMouseButton(0))
        {
            transform.GetChild(0).localScale = new Vector3(1, transform.GetChild(0).localScale.y - (Time.deltaTime / DrawTime), 1);
        }
        if(transform.GetChild(0).localScale.y<=0f)
        {
            DrawScript ds = GameObject.FindGameObjectWithTag("GameController").GetComponent<DrawScript>();
            ds.currentlyDrawing= null;
            ds.ChalklingCount++;
            Chalkling.GetComponent<BoxCollider2D>().enabled=true;
            Chalkling.GetComponent<Animator>().enabled = true;
            Chalkling.GetComponent<SpriteRenderer>().sortingOrder = 0;
            ds.UpdateChalk(-1*Chalkling.GetComponent<ChalklingControl>().ChalkCost);
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
