using UnityEngine;
using System.Collections;

public class PipScript : MonoBehaviour {

    public PuzzleManager PM;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (PM.GoalCount == 1)
        { PM.Wins++;
            PM.GUIHandler();
            Application.LoadLevel(0);
        }
        else
        {
            PM.GoalCount--;
            Destroy(gameObject);
        }
    }

	// Use this for initialization
	void Start () {
        PM = (PuzzleManager)GameObject.Find("Manager").GetComponent(typeof(PuzzleManager));
        PM.GoalCount++;
	}
	
	// Update is called once per frame
	void Update () {

        transform.Rotate(0, 0, 36 * Time.deltaTime);
	
	}
}
