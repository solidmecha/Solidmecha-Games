using UnityEngine;
using System.Collections.Generic;

public class PlayerControl : MonoBehaviour {

    public float speed;
    public GameControl GC;
    int[] dirCheck = new int[2] {1,0};
    bool superNom;
    float counter;
    public GameObject victory;
    List<Color> AIColors = new List<Color> { };

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("pip"))
        {
            //nom pips
            Destroy(other.gameObject);
            GC.pipCount--;
            if (GC.pipCount == 0)
                Instantiate(victory);
        }
        if(other.CompareTag("pow"))
        {
            counter = 0;
            Destroy(other.gameObject);
            AIColors.Clear();
            foreach (GameObject g in GC.Chasers)
            {
                AIColors.Add(g.transform.GetChild(0).GetComponent<SpriteRenderer>().color);
                AIscript ais = (AIscript)g.GetComponent(typeof(AIscript));
                ais.speed = 3.1f;
                g.transform.GetChild(0).GetComponent<SpriteRenderer>().color = GC.Pip.GetComponent<SpriteRenderer>().color;
            }
            superNom = true;
        }
        else if (other.CompareTag("cha"))
        {
            if (!superNom)
                Application.LoadLevel(0);
            else
            {
                Destroy(other.gameObject);
                for (int i = 0; i < GC.Chasers.Count; i++)
                {
                    if(GC.Chasers[i].Equals(other.gameObject))
                    {
                        GC.Chasers.RemoveAt(i);
                        AIColors.RemoveAt(i);
                    }
                }
            }
        }
    }

	// Use this for initialization
	void Start () {
	
	}

    Vector2 PlayerWorldVec()
    {
        Vector2 V = transform.position - GC.transform.position;
        V = new Vector2(Mathf.Round(V.x), Mathf.Round(V.y));
        return V;
    }
	
	// Update is called once per frame
	void Update () {
        if(superNom)
        {
            counter += Time.deltaTime;
            if(counter>=7.2)
            {
                counter = 0;
                superNom = false;
                for (int i = 0; i < GC.Chasers.Count; i++)
                {
                    GC.Chasers[i].transform.GetChild(0).GetComponent<SpriteRenderer>().color = AIColors[i];
                    AIscript ais = (AIscript)GC.Chasers[i].GetComponent(typeof(AIscript));
                    ais.speed = 3.1f;
                }
            }
            else if(counter>=7)
            {
                foreach (GameObject g in GC.Chasers)
                {
                    g.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.red;
                }
            }
            else if (counter >= 6.6)
            {
                foreach (GameObject g in GC.Chasers)
                {
                    g.transform.GetChild(0).GetComponent<SpriteRenderer>().color = GC.Pip.GetComponent<SpriteRenderer>().color;
                }
            }
            else if (counter >= 6.2)
            {
                foreach (GameObject g in GC.Chasers)
                {
                    g.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.red;
                }

            }
            else if (counter >= 5.8)
            {
                foreach (GameObject g in GC.Chasers)
                {
                    g.transform.GetChild(0).GetComponent<SpriteRenderer>().color = GC.Pip.GetComponent<SpriteRenderer>().color;
                }

            }
            else if (counter >= 5.4)
            {
                foreach (GameObject g in GC.Chasers)
                {
                    g.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.red;
                }
            }
            else if (counter >= 5)
            {
                foreach (GameObject g in GC.Chasers)
                {
                    g.transform.GetChild(0).GetComponent<SpriteRenderer>().color = GC.Pip.GetComponent<SpriteRenderer>().color;
                }

            }
            else if (counter >= 4.6)
            {
                foreach (GameObject g in GC.Chasers)
                {
                    g.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.red;
                }
            }
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            if (GC.World[(int)PlayerWorldVec().x - 1][(int)PlayerWorldVec().y].GetComponent<SpriteRenderer>().color.Equals(Color.white))
            {
                dirCheck = new int[2] { -1,0};
                transform.position = PlayerWorldVec() + (Vector2)GC.transform.position;
                transform.eulerAngles = new Vector3(0, 0, 180);
            }
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            if (GC.World[(int)PlayerWorldVec().x + 1][(int)PlayerWorldVec().y].GetComponent<SpriteRenderer>().color.Equals(Color.white))
            {
                dirCheck = new int[2] { 1, 0 };
                transform.position = PlayerWorldVec() + (Vector2)GC.transform.position;
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            if (GC.World[(int)PlayerWorldVec().x][(int)PlayerWorldVec().y-1].GetComponent<SpriteRenderer>().color.Equals(Color.white))
            {
                dirCheck = new int[2] { 0, -1 };
                transform.position = PlayerWorldVec() + (Vector2)GC.transform.position;
                transform.eulerAngles = new Vector3(0, 0, -90);
            }
        }
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            if (GC.World[(int)PlayerWorldVec().x][(int)PlayerWorldVec().y+1].GetComponent<SpriteRenderer>().color.Equals(Color.white))
            {
                dirCheck = new int[2] { 0, 1 };
                transform.position = PlayerWorldVec()+(Vector2)GC.transform.position;
                transform.eulerAngles = new Vector3(0, 0, 90);
            }
        }

        if (GC.World[(int)PlayerWorldVec().x + dirCheck[0]][(int)PlayerWorldVec().y + dirCheck[1]].GetComponent<SpriteRenderer>().color.Equals(Color.white))
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        else if(Vector2.Distance(transform.position, PlayerWorldVec()+(Vector2)GC.transform.position) >.09f)
            transform.Translate(Vector2.right * speed * Time.deltaTime);

    }
}
