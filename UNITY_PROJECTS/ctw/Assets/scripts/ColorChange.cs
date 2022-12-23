using UnityEngine;
using System.Collections;

public class ColorChange : MonoBehaviour {

    public Color[] Colors;
    SpriteRenderer SR; 
    
	// Use this for initialization
	void Start () {
        SR = GetComponent<SpriteRenderer>();
        StartCoroutine(changeColor());
	}

    IEnumerator changeColor()
    {
        while (true)
        {
            SR.color = Color.Lerp(Colors[0], Colors[1], Mathf.PingPong(Time.time, 1));
            yield return null;
        }
    }
	
	// Update is called once per frame
	void Update () {
	 
	}
}
