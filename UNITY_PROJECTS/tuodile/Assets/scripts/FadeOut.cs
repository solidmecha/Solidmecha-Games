using UnityEngine;
using System.Collections;

public class FadeOut : MonoBehaviour {

    float counter;
    SpriteRenderer SR;
	// Use this for initialization
	void Start () {
        SR = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        counter += Time.deltaTime;
        SR.color = Color.Lerp(Color.white, Color.clear, counter * 2);
        if (counter >= .5f)
            Destroy(gameObject);
	}
}
