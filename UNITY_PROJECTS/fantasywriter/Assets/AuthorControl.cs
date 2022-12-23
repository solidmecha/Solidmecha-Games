using UnityEngine;
using System.Collections;

public class AuthorControl : MonoBehaviour {

    public TextControl[] WordControls;
    int CurrentIndex;
    int CurrentWordIndex;
    float counter;
    public float Speed;
    public bool Writing;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Writing)
        {
            counter += Time.deltaTime;
            if (counter >= Speed)
            {
                counter = 0;
                WordControls[CurrentIndex].Text.text = WordControls[CurrentIndex].Words[CurrentWordIndex];
                CurrentWordIndex++;
                if (CurrentWordIndex == WordControls[CurrentIndex].Words.Length)
                    CurrentWordIndex = 0;
        }
            if(Input.anyKeyDown)
            {
                CurrentIndex++;
                if (CurrentIndex == WordControls.Length)
                    Writing = false;
            }
        }
	}
}
