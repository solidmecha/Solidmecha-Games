using UnityEngine;
using System.Collections;

public class Coinscript : MonoBehaviour {

    public int Chance;
    bool flipping;
    float counter;
    float FinalRotation;
    int CircleCount;
    int Flips;
    public UnityEngine.UI.Text Msg;
    public bool inTest;

    private void OnMouseDown()
    {
        Flip();
    }

    public void Flip()
    {
        if (!flipping)
        {
            Flips++;
            FlipControl.singleton.UpdateFlip();
            if (FlipControl.singleton.RNG.Next(10) >= Chance)
            {
                FinalRotation = 180f;
            }
            else
            {
                FinalRotation = 0;
                counter++;
                CircleCount++;
            }
            if (Mathf.Abs(FinalRotation - Mathf.Abs(transform.localEulerAngles.x)) < 1)
                counter = 1;
            else
                counter = 1.25f;
            flipping = true;
        }
    }

    public void Trial()
    {
        Flips++;
        FlipControl.singleton.UpdateFlip();
        if (FlipControl.singleton.RNG.Next(10) >= Chance)
        {
        }
        else
        {
            CircleCount++;
        }
        Msg.text = "Circles: " + CircleCount.ToString() + "\nFlips: " + Flips.ToString() + "\nCircle%: " + (Mathf.RoundToInt((float)CircleCount / (float)Flips * 100f)).ToString();

    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (inTest)
            Trial();
	   if(flipping)
        {
            transform.Rotate(new Vector3(720 * Time.deltaTime, 0));
            counter -= Time.deltaTime;
            if(counter<=0)
            {
                counter = 0;
                flipping = false;
                transform.localEulerAngles = new Vector3(FinalRotation, 0);
            }
        }
	}
}
