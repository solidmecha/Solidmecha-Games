using UnityEngine;
using System.Collections;

public class AutoClick : MonoBehaviour {

    public float delay;

	// Use this for initialization
	void Start () {
        StartCoroutine(Click());
	}

    IEnumerator Click()
    {
        while(true)
        {
            yield return new WaitForSeconds(delay);
            if (transform.parent.GetComponent<GameControl>().won)
                break;
            foreach (CardControl cc in transform.parent.GetComponentsInChildren<CardControl>())
                cc.MouseClick();         
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
