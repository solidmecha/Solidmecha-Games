using UnityEngine;
using System.Collections;

public class HighlightScript : MonoBehaviour {

    public SpriteRenderer SR;
    public bool Highlight;
    public Color EndC;
    Color StartC;
    float counter;

	// Use this for initialization
	void Start () {
	
	}

    public void BeginHilight(SpriteRenderer S)
    {
        Highlight = true;
        SR = S;
        StartC = S.color;
    }

    public void StopHiLight()
    {
        SR.color = StartC;
        Highlight = false;
    }


	
	// Update is called once per frame
	void Update () {
        counter += Time.deltaTime;
        if (counter > 1)
            counter = 0;
        if(Highlight)
        {
            SR.color = Color.Lerp(StartC, EndC, counter);
        }
	
	}
}
