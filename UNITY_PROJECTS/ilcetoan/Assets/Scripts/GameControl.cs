using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class GameControl : MonoBehaviour {

    public GameObject block;
    public System.Random RNG;
    public float delay;
    public List<Color> Colors = new List<Color> { };
    public Text ScoreText;
    public int CurrentScore;

	// Use this for initialization
	void Start () {
        RNG = new System.Random(ThreadSafeRandom.Next());

        for(int x=-5;x<7;x++)
        {
            for(int y = -5; y < 7; y++)
            {
                Vector2 pos = new Vector2(-.5f + x, -.5f + y);
                GameObject go = Instantiate(block, pos, Quaternion.identity) as GameObject;
                int r = RNG.Next(Colors.Count);
                go.GetComponent<SpriteRenderer>().color = Colors[r];
                go.GetComponent<BlockScript>().ID = r;
            }
        }

      //  StartCoroutine(PrintBlock());
	}

    public void UpdateScore(int ScoreChange)
    {
        CurrentScore += ScoreChange;
        ScoreText.text=CurrentScore.ToString();
    }

    public IEnumerator PrintBlock()
    {
        yield return new WaitForSeconds(delay);
        float x=-.5f+RNG.Next(2);
        float y= -.5f + RNG.Next(2);
        Vector2 pos = new Vector2(x + RNG.Next(-5, 6), y + RNG.Next(-5, 6));
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
        if (hit.collider == null)
        {
            GameObject go = Instantiate(block, pos, Quaternion.identity) as GameObject;
            int r = RNG.Next(Colors.Count);
            go.GetComponent<SpriteRenderer>().color = Colors[r];
            go.GetComponent<BlockScript>().ID = r;
            
        }
        StartCoroutine(PrintBlock());
    }
	
	// Update is called once per frame
	void Update () {
	}
}
