using UnityEngine;
using System.Collections;

public class FogScript : MonoBehaviour {

    bool isFading;
    float targetAlpha;
    int UnitCount;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<UnitScript>().PlayerControlled)
        {
            targetAlpha = -1;
            isFading = true;
            UnitCount++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<UnitScript>().PlayerControlled)
        {
            UnitCount--;
            if (UnitCount == 0)
            {
                targetAlpha = 1;
                isFading = true;
            }
           
        }

    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	if(isFading)
        {
            GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, GetComponent<SpriteRenderer>().color.a + targetAlpha * Time.deltaTime);
            if(GetComponent<SpriteRenderer>().color.a<=0)
            {
                isFading = false;
                GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
            }
            else if (GetComponent<SpriteRenderer>().color.a >= 1)
            {
                isFading = false;
                GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 1);
            }
        }
	}
}
