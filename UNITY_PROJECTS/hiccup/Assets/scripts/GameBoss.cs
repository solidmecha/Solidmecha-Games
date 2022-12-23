using UnityEngine;
using System.Collections;

public class GameBoss : MonoBehaviour {

    public System.Random RNG;
    public GameObject Block;
    public GameObject pip;
    public Transform player;
    float countdown;
    public Color[] colors;
    public bool GameOver;


    // Use this for initialization
    void Start () {
    	RNG=new System.Random();
    }

    void createChallenge()
    {
        GameObject go = Instantiate(Block) as GameObject;
        BarBoss BB = go.GetComponent<BarBoss>();
        //BB.transform.GetChild(0).GetComponent<SpriteRenderer>().color = colors[RNG.Next(3)];
        //BB.transform.GetChild(1).GetComponent<SpriteRenderer>().color = colors[RNG.Next(3)];
        if (RNG.Next(4) != 1)
        {
            switch (RNG.Next(4))
            {
                case 0:
                    BB.transform.position = new Vector2(RNG.Next(-6, 7), -6);
                    BB.speed = -1 * RNG.Next(1, 6);
                    BB.lifeTime = 12;
                    break;
                case 1:
                    BB.transform.localEulerAngles = new Vector3(0, 0, 90);
                    BB.transform.position = new Vector2(-9, RNG.Next(-3, 4));
                    BB.speed = RNG.Next(1, 6);
                    BB.lifeTime = 18;
                    break;
                case 2:
                    BB.transform.localEulerAngles = new Vector3(0, 0, 90);
                    BB.transform.position = new Vector2(9, RNG.Next(-3, 4));
                    BB.speed = -1 * RNG.Next(1, 6);
                    BB.lifeTime = 18;
                    break;
                default:
                    BB.transform.position = new Vector2(RNG.Next(-6, 7), 6);
                    BB.speed = RNG.Next(1, 6);
                    BB.lifeTime = 12;
                    break;
            }
        }
        else
        {
            switch (RNG.Next(4))
            {
                case 0:
                    BB.transform.position = new Vector2(-8,5);
                    BB.transform.localEulerAngles = new Vector3(0, 0, 45);
                    BB.speed = RNG.Next(1, 6);
                    BB.lifeTime = 15;
                    break;
                case 1:
                    BB.transform.position = new Vector2(8, -5);
                    BB.transform.localEulerAngles = new Vector3(0, 0, 45);
                    BB.speed = -1*RNG.Next(1, 6);
                    BB.lifeTime = 15;
                    break;
                case 2:
                    BB.transform.position = new Vector2(-8, -5);
                    BB.transform.localEulerAngles = new Vector3(0, 0, -45);
                    BB.speed = -1*RNG.Next(1, 6);
                    BB.lifeTime = 15;
                    break;
                default:
                    BB.transform.position = new Vector2(8, 5);
                    BB.transform.localEulerAngles = new Vector3(0, 0, -45);
                    BB.speed = RNG.Next(1, 6);
                    BB.lifeTime = 15;
                    break;
            }
        }
        BB.rotSpeed = 30;
        if (RNG.Next(2) == 0)
            BB.rotSpeed = -30;
        if (RNG.Next(4) == 3)
        {
            if (RNG.Next(2) == 0)
                BB.scaleSpeed = RNG.Next(1,3);
            BB.rotStart = BB.lifeTime - RNG.Next(3, 7);
        }
        


    }
	
	// Update is called once per frame
	void Update () {
        countdown -= Time.deltaTime;
	    if(countdown<=0)
        {
            if (!GameOver)
            {
                Vector2 v = new Vector2(RNG.Next(-7, 8), RNG.Next(-4, 5));
                while (Vector2.SqrMagnitude((v - (Vector2)player.position)) <= 1)
                    v = new Vector2(RNG.Next(-7, 8), RNG.Next(-4, 5));
                Instantiate(pip, v, Quaternion.identity);
            }
            int c = RNG.Next(1, 4);
            for(int i=0;i<c;i++)
                createChallenge();
            countdown = RNG.Next(5,7);
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            Application.LoadLevel(0);
        }
	}
}
