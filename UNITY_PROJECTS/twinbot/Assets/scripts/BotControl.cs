using UnityEngine;
using System.Collections;

public class BotControl : MonoBehaviour {

    public static BotControl singleton;
    public float[] speed;
    public float[] fireDelay;
    float[] fireCounter=new float[2] { 0, 0 };
    public GameObject Blast;
    bool[] isFiring=new bool[2] { true, true };
    public Transform[] Bots;
    public Sprite[] Forms;

    private void Awake()
    {
        singleton = this;
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        fireCounter[0] -= Time.deltaTime;
        fireCounter[1] -= Time.deltaTime;
        Vector2 FireDirL = Vector2.zero;
        Vector2 FireDirR = Vector2.zero;
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Space))
        {

            isFiring[0] = !isFiring[0];
            if (isFiring[0])
                Bots[0].GetComponent<SpriteRenderer>().sprite = Forms[0];
            else
                Bots[0].GetComponent<SpriteRenderer>().sprite = Forms[1];
        }
        if (Input.GetKeyDown(KeyCode.U) || Input.GetKeyDown(KeyCode.Space))
        {
            isFiring[1] = !isFiring[1];
            if (isFiring[1])
                Bots[1].GetComponent<SpriteRenderer>().sprite = Forms[0];
            else
                Bots[1].GetComponent<SpriteRenderer>().sprite = Forms[1];
        }
        if (Bots[0].GetComponent<BotScript>().HP > 0)
        {
            if (Input.GetKey(KeyCode.W))
            {
                FireDirL += Vector2.up;
            }
            if (Input.GetKey(KeyCode.A))
                FireDirL += Vector2.left;
            if (Input.GetKey(KeyCode.D))
                FireDirL += Vector2.right;
            if (Input.GetKey(KeyCode.S))
                FireDirL += Vector2.down;
        }
        if (FireDirL.x != 0 || FireDirL.y != 0)
        {
            float a = Vector2.Angle(Vector2.up, FireDirL);
            if (FireDirL.x > 0)
                a *= -1;
            Bots[0].transform.GetChild(0).localEulerAngles = new Vector3(0, 0, a);
            if (isFiring[0])
            {
                if (fireCounter[0] <= 0)
                {
                    fireCounter[0] = fireDelay[0];
                    GameObject b = Instantiate(Blast, (Vector2)Bots[0].transform.position + (FireDirL * .75f), Bots[0].GetChild(0).rotation) as GameObject;
                    b.GetComponent<SpriteRenderer>().color = Bots[0].GetComponent<SpriteRenderer>().color;
                    b.GetComponent<BlastScript>().speed = speed[0] + 2.5f;
                    if (Bots[0].GetComponent<BotScript>().TripleShot)
                    {
                        b = Instantiate(Blast, (Vector2)Bots[0].transform.position + (FireDirL * .75f), Quaternion.Euler(0, 0, Bots[0].GetChild(0).eulerAngles.z + 30)) as GameObject;
                        b.GetComponent<SpriteRenderer>().color = Bots[0].GetComponent<SpriteRenderer>().color;
                        b.GetComponent<BlastScript>().speed = speed[0] + 2.5f;
                        b = Instantiate(Blast, (Vector2)Bots[0].transform.position + (FireDirL * .75f), Quaternion.Euler(0, 0, Bots[0].GetChild(0).eulerAngles.z - 30)) as GameObject;
                        b.GetComponent<SpriteRenderer>().color = Bots[0].GetComponent<SpriteRenderer>().color;
                        b.GetComponent<BlastScript>().speed = speed[0] + 2.5f;
                    }
                }
            }
            Bots[0].transform.Translate(FireDirL * speed[0] * Time.deltaTime);

        }
        if (Bots[1].GetComponent<BotScript>().HP >0)
        {
            if (Input.GetKey(KeyCode.J))
            {
                FireDirR += Vector2.left;
            }
            if (Input.GetKey(KeyCode.L))
            {

                FireDirR += Vector2.right;
            }
            if (Input.GetKey(KeyCode.I))
            {
                FireDirR += Vector2.up;
            }
            if (Input.GetKey(KeyCode.K))
            {
                FireDirR += Vector2.down;
            }
        }
        if (FireDirR.x != 0 || FireDirR.y != 0)
            {
                float a1 = Vector2.Angle(Vector2.up, FireDirR);
                if (FireDirR.x > 0)
                    a1 *= -1;
                Bots[1].transform.GetChild(0).localEulerAngles = new Vector3(0, 0, a1);
                if (isFiring[1])
                {
                    if (fireCounter[1] <= 0)
                    {
                        fireCounter[1] = fireDelay[1];
                       GameObject b = Instantiate(Blast, (Vector2)Bots[1].transform.position + (FireDirR * .75f), Bots[1].GetChild(0).rotation) as GameObject;
                       b.GetComponent<SpriteRenderer>().color = Bots[1].GetComponent<SpriteRenderer>().color;
                       b.GetComponent<BlastScript>().speed = speed[1] + 2.5f;
                    if (Bots[1].GetComponent<BotScript>().TripleShot)
                    {
                        b = Instantiate(Blast, (Vector2)Bots[1].transform.position + (FireDirR * .75f), Quaternion.Euler(0, 0, Bots[1].GetChild(0).eulerAngles.z + 30)) as GameObject;
                        b.GetComponent<SpriteRenderer>().color = Bots[1].GetComponent<SpriteRenderer>().color;
                        b.GetComponent<BlastScript>().speed = speed[0] + 2.5f;
                        b = Instantiate(Blast, (Vector2)Bots[1].transform.position + (FireDirR * .75f), Quaternion.Euler(0, 0, Bots[1].GetChild(0).eulerAngles.z - 30)) as GameObject;
                        b.GetComponent<SpriteRenderer>().color = Bots[1].GetComponent<SpriteRenderer>().color;
                        b.GetComponent<BlastScript>().speed = speed[1] + 2.5f;
                    }
                }
            }
            Bots[1].transform.Translate(FireDirR * speed[1] * Time.deltaTime);
        }
        if (Input.GetKeyDown(KeyCode.R) && Bots[0].GetComponent<BotScript>().HP == 0 && Bots[1].GetComponent<BotScript>().HP == 0)
            Application.LoadLevel(0);
    }


}

