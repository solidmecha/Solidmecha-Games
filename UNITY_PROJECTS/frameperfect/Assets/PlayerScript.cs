using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {


    bool jumped;
    bool win;
    float counter;
    public GameObject[] Plats;
    float frameTime;
    bool Ready;

    // Use this for initialization
    void Start () {
        frameTime = 1f / FrameControl.singleton.FPS;
	}
	
	// Update is called once per frame
	void Update () {
        if (!jumped && Input.anyKeyDown)
        {
            jumped = true;
            transform.position= (Vector2)transform.position + Vector2.down * -5;
        }
        else if(jumped && !win)
        {
            counter+= Time.deltaTime;
            bool pushFrame = false;
            if (Ready && counter >= frameTime)
            {
                Ready = false;
                Plats[0].GetComponent<SpriteRenderer>().color = Color.white;
                transform.position = new Vector2(0, -3.5f);
                jumped = false;
            }
            if (counter >= frameTime)
            {
                counter = 0;
                pushFrame = true;
            }
            bool gZ = (transform.position.y > 0);
            if (pushFrame)
            {
                transform.Translate(Vector2.down * frameTime);
            }
            if(!Ready)
                Ready = gZ && transform.position.y <= 0;
            else if(Ready)
                Plats[0].GetComponent<SpriteRenderer>().color = Color.red;
            if (Ready && Input.anyKeyDown)
            {
                Destroy(Plats[0]);
                win = true;
                Ready = false;
            }
        }
        else if(jumped)
        {
            counter += Time.deltaTime;
            bool pushFrame = false;
            if (transform.position.y <= -3.5f && counter>=frameTime)
            {
                Plats[1].GetComponent<SpriteRenderer>().color = Color.white;
                transform.position = new Vector2(0, -3.5f);
                jumped = false;
                Ready = false;
            }
            if (counter >= frameTime)
            {
                counter = 0;
                pushFrame = true;
            }
            bool gt = (transform.position.y > -3.5f);
            if(pushFrame)
                transform.Translate(Vector2.down * frameTime);
            if (!Ready)
                Ready = gt && transform.position.y <= -3.5f;
            else if (Ready)
                Plats[1].GetComponent<SpriteRenderer>().color = Color.red;

            if (Ready && Input.anyKeyDown)
            {
                FrameControl.singleton.win();
                Destroy(transform.parent.gameObject);
                jumped = false;
            }
            
        }
    }
}
