using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ChalklingControl : MonoBehaviour {

    public int[] HPr;
    public int[] DPSr;
    public int[] Speedr;
    public float HP;
    public float DPS;
    public float Speed;
    public float currentSpeed;
    public float DrawTime;
    public int ChalkCost;
    public float CoverOffset;
    ChalklingControl Attacker;
    public GameObject Panel;
    GameObject currentPanel;
    public bool flashRed;
    float flashCounter;
    public float flashNumber;
    public bool Wild;
    public bool Warding;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.collider.CompareTag("C") || collision.collider.CompareTag("F")) && !Warding)
        {
            if (collision.collider.GetComponent<ChalklingControl>().Warding && !Wild)
                return;
            Attacker = collision.collider.GetComponent<ChalklingControl>();
            currentSpeed = 0;
            if(GetComponent<Animator>())
                GetComponent<Animator>().SetBool("Walking", false);
            Vector2 diff = Attacker.transform.position - transform.position;
            float x = Mathf.Abs(diff.x);
            float y = Mathf.Abs(diff.y);
            if (x >= y)
            {
                if (diff.x > 0)
                {
                    transform.localEulerAngles = new Vector3(0, 0, 0);
                    GetComponent<SpriteRenderer>().flipY = false;
                }
                else
                {
                    transform.localEulerAngles = new Vector3(0, 0, 180);
                    GetComponent<SpriteRenderer>().flipY = true;
                }

            }
            else
            {
                if (diff.y > 0)
                {
                    transform.localEulerAngles = new Vector3(0, 0, 90);
                }
                else
                {
                    transform.localEulerAngles = new Vector3(0, 0, -90);
                }
            }
        }
    }

    private void OnMouseDown()
    {
        if (!Wild && Attacker == null)
        {
            if (currentPanel != null)
                Destroy(currentPanel);
            GameObject go = Instantiate(Panel, (Vector2)transform.position + .5f * Vector2.down, Quaternion.identity) as GameObject;
            currentPanel = go;
            foreach (CommandScript c in go.transform.GetComponentsInChildren<CommandScript>())
                c.chalkling = transform;
        }
    }

	// Use this for initialization
	void Start () {
        System.Random r = new System.Random();
        HP = r.Next(HPr[0], HPr[1]);
        HP *= 2.5f;
        DPS = r.Next(DPSr[0], DPSr[1]);
        Speed = r.Next(Speedr[0], Speedr[1]);
        Speed /= 10f;

    }

    public bool IsAttacking()
    {
        return Attacker != null;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        transform.Translate(currentSpeed * Vector2.right * Time.deltaTime);
        if (Attacker != null)
        {
            Attacker.HP -= DPS * Time.deltaTime;
            Attacker.flashRed=true;
            Attacker.flashNumber=1;
            if (Attacker.Attacker == null)
                Attacker.Attacker = this;
        }
        if(flashRed)
        {
            flashCounter -= Time.deltaTime;
            if (flashCounter <= 0)
            {
                if (GetComponent<SpriteRenderer>().color.g == 0f)
                {
                    GetComponent<SpriteRenderer>().color = Color.white;
                    flashNumber--;
                }
                else
                    GetComponent<SpriteRenderer>().color = Color.red;
                flashCounter = .3f;
            }

        }
        if (flashNumber<=0 && flashRed)
        {
            GetComponent<SpriteRenderer>().color = Color.white;
            flashRed = false;
        }
        if (HP <= 0)
        {
           DrawScript ds= GameObject.FindGameObjectWithTag("GameController").GetComponent<DrawScript>();
            if (!Wild)
                ds.ChalklingCount--;
            ds.UpdateChalk(ChalkCost / 2);
            if(Warding)
            {
                ds.EndGame();
            }
            if (currentPanel != null)
                Destroy(currentPanel);
            Destroy(gameObject);
        }
	}
}
