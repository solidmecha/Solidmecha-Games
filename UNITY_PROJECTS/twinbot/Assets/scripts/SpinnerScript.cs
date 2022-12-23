using UnityEngine;
using System.Collections;

public class SpinnerScript : MonoBehaviour {

    public Transform track;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (transform.childCount == 0 || track == null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
                transform.position = track.transform.position;
                transform.Rotate(new Vector3(0, 0, 90) * Time.deltaTime);
        }

	}
}
