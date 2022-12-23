using UnityEngine;
using System.Collections;

public class TileControl : MonoBehaviour {

    public int[]  ColorIDs;
    public bool matched;
    public bool fall;
    public int fallDistance;
    
    public void UpdateColors()
    {
        GameControl gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>();
        for (int i = 0; i < 4; i++)
            transform.GetChild(i).GetComponent<SpriteRenderer>().color = gc.colors[ColorIDs[i]];
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (matched)
        {
            if(fallDistance==0)
            {
                if(GetComponent<RotateIt>())
                {
                    Destroy(GetComponent<RotateIt>());
                    transform.localEulerAngles = Vector3.zero;
                    GetComponent<TileControl>().UpdateColors();
                }
                Destroy(GetComponent<BoxCollider2D>());
                fallDistance = 1;
                for(int y=(int)transform.position.y; y<9; y++)
                {
                    RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, y), Vector2.zero);
                    if(hit.collider != null && hit.collider.CompareTag("T"))
                    {
                        TileControl tc = hit.collider.GetComponent<TileControl>();
                        if(!tc.matched)
                        {
                            tc.fallDistance++;
                            if(!GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>().DropList.Contains(tc))
                            {
                                GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>().DropList.Add(tc);
                            }
                        }
                    }
                }
            }
            transform.localScale = (Vector2)transform.localScale + Vector2.one * fallDistance * Time.deltaTime*2.74f;
            if (transform.localScale.x >= 1.15f)
                fallDistance = -1;
            if(transform.localScale.x<=0)
                Destroy(gameObject);
        }
        else if(fall)
        {
            transform.Translate(Vector2.down * Time.deltaTime*fallDistance*2.381f);
        }
	}
}
