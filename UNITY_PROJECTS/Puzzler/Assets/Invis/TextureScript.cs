using UnityEngine;
using System.Collections;

public class TextureScript : MonoBehaviour {


	// Use this for initialization
	void Start () {

        SpriteRenderer T=gameObject.GetComponent<SpriteRenderer>();
                for(int a=0;a<360*5;a++)
                {
                    float r = a*Mathf.Deg2Rad;
                    float x = 5*r * Mathf.Cos(r) + T.sprite.texture.width/2;
                    float y = 5*r * Mathf.Sin(r) + T.sprite.texture.height / 2;
                    T.sprite.texture.SetPixel((int)x, (int)y, Color.black);
                }
        T.sprite.texture.Apply();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
