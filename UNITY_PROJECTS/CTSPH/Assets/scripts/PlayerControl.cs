using UnityEngine;
using System.Collections.Generic;

public class PlayerControl : MonoBehaviour {

    public float speed;
    SpriteRenderer SR;
    public List<Sprite> SpriteList;
    public int spriteIndex;
    public PulseScript PS;

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.name.Equals("bullet(Clone)"))
        {
            BulletScript bs = (BulletScript)coll.gameObject.GetComponent(typeof(BulletScript));
            if (bs.id != spriteIndex)
            {
                PS.number_Of_Forms = bs.formNumber;
                Destroy(coll.gameObject);
                Destroy(gameObject);
                PS.GameOver();
            }
        }
    }
	// Use this for initialization
	void Start () {
        SR = GetComponent<SpriteRenderer>();
	}

    void setSprite()
    {
        if(spriteIndex>=SpriteList.Count)
        { spriteIndex = 0; }
        else if(spriteIndex<0)
        { spriteIndex = SpriteList.Count-1; }

        SR.sprite = SpriteList[spriteIndex];
    }
	

	// Update is called once per frame
	void Update () {

        /*
        if (Input.GetKey(KeyCode.W))
        { transform.Translate(Vector2.up * Time.deltaTime * speed);
        }

        if (Input.GetKey(KeyCode.A))
        { transform.Translate(Vector2.left * Time.deltaTime * speed);
        }

        if (Input.GetKey(KeyCode.S))
        { transform.Translate(Vector2.down * Time.deltaTime * speed);
        }

        if (Input.GetKey(KeyCode.D))
        { transform.Translate(Vector2.right * Time.deltaTime * speed);
        }
        */

        if (Input.GetKeyDown(KeyCode.Q) || Input.GetMouseButtonDown(0))
        {
            spriteIndex--;
            setSprite();
        }

        if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(1))
        {
            spriteIndex++;
            setSprite();
        }
    }
}
