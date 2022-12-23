using UnityEngine;
using System.Collections;

public class FrameCheck : MonoBehaviour {

    int index;
    bool Playing;
    public GameObject circ;
	// Use this for initialization
	void Start () {
	  for(int i=0;i<12;i++)
        {
            for(int j=0;j<5;j++)
            {
                Instantiate(circ, (Vector2)transform.position + new Vector2(i, j), Quaternion.identity, transform);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (Playing)
        {
            int last = index;
            index++;
            if (index == transform.childCount)
                index = 0;
            transform.GetChild(index).GetComponent<SpriteRenderer>().color = Color.blue;
            transform.GetChild(last).GetComponent<SpriteRenderer>().color = Color.white;
        }
        
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Playing = !Playing;
        }

	}
}
