using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class FlipControl : MonoBehaviour {

    public static FlipControl singleton;
    public System.Random RNG;
    public List<GameObject> Coins;
    public GameObject ProbCanvas;
    bool FindingLower;
    public Text[] Messages;
    int foundCount;
    int flipCount;

    private void Awake()
    {
        singleton = this;
        RNG = new System.Random();
        List<int> Chances = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        foreach (GameObject c in Coins)
        {
            int r = RNG.Next(Chances.Count);
            c.GetComponent<Coinscript>().Chance = Chances[r];
            Chances.RemoveAt(r);
        }
    }

    // Use this for initialization
    void Start () {
        FindingLower = RNG.Next(2) == 0;
        Messages[0].text = "Find the coins with more than 50% chance of coming up Circle";
        if(FindingLower)
            Messages[0].text = "Find the coins with less than 50% chance of coming up Circle";
    }

    public void UpdateFlip()
    {
        flipCount++;
        Messages[2].text = "Flips: " + flipCount.ToString();
    }

    void FlipAll()
    {
        foreach (Coinscript c in transform.GetComponentsInChildren<Coinscript>())
        {
            c.Flip();
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FlipAll();
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            foreach (Coinscript c in transform.GetComponentsInChildren<Coinscript>())
            {
                if (!c.inTest)
                {
                    c.Msg = (Instantiate(ProbCanvas, c.transform.position, Quaternion.identity) as GameObject).transform.GetChild(0).GetComponent<Text>();
                    c.inTest = true;
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.R))
            Application.LoadLevel(0);
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if(hit.collider != null)
            {
                Coinscript C = hit.collider.GetComponent<Coinscript>();
                Messages[1].text = "That coin had a " + C.Chance.ToString() + "0% chance to land on Circle.";
                if ((FindingLower && C.Chance<5) || (!FindingLower && C.Chance>5))
                {
                    foundCount++;
                }
                else
                {
                    foundCount = -10;
                    Messages[0].text = "Game Over. Press 'R' to try again.";
                }
                if (foundCount == 4)
                {
                    string s = "";
                    if (FindingLower)
                        s = "more than";
                    else
                        s = "less than";
                    Messages[0].text = "You win this round! \nCan you find the ones with "+s+" 50% chance of coming up Circle?";
                    FindingLower = !FindingLower;
                }
                if(foundCount == 8)
                {
                    Messages[0].text = "Victory! You have found the fairest coin.";
                }
                Destroy(C.gameObject);
            }
        }
	}
}
