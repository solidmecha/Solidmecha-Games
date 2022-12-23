using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ProgressScript : MonoBehaviour {
    public float Timer;
    float counter;
    Text Percent;
	// Use this for initialization
	void Start () {
        Percent = transform.GetChild(0).GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        counter += Time.deltaTime;
        float P = 100*counter / Timer;
        P=Mathf.Round(P);
        Percent.text = P.ToString()+"%";
        if (counter >= Timer)
            Destroy(gameObject);
	}
}
