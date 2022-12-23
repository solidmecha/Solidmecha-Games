using UnityEngine;
using System.Collections.Generic;

public class BlockScript : MonoBehaviour {

    public float delay;
    public int ID;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("B"))
            checkForClear();
    }

    void checkForClear()
    {
        List<GameObject> Blocks = new List<GameObject> { };
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position+new Vector2(i,j), Vector2.zero);
                if (hit.collider!=null && hit.collider.CompareTag("B") && hit.collider.GetComponent<BlockScript>().ID==ID)
                    Blocks.Add(hit.collider.gameObject);
            }
        }

        
        if (Blocks.Count > 3)
        {
            for (int b = 0; b < Blocks.Count; b++)
            {
                if (Blocks[b] != null)
                {
                    for (int i = -1; i < 2; i++)
                    {
                        for (int j = -1; j < 2; j++)
                        {
                            RaycastHit2D hit = Physics2D.Raycast((Vector2)Blocks[b].transform.position + new Vector2(i, j), Vector2.zero);
                            if (hit.collider != null && hit.collider.CompareTag("B") && hit.collider.GetComponent<BlockScript>().ID == ID && !Blocks.Contains(hit.collider.gameObject))
                                Blocks.Add(hit.collider.gameObject);
                        }
                    }
                }
            }

            for (int i = 0; i < Blocks.Count; i++)
            {
                if(Blocks[i] != null)
                {
                    Destroy(Blocks[i]);
                    GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>().UpdateScore(2*i);
                }
            }

        }
        Blocks.Clear();
    }
    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        delay -= Time.deltaTime;
        if (delay <= 0)
            GetComponent<Rigidbody2D>().gravityScale = 1;
	}
}
