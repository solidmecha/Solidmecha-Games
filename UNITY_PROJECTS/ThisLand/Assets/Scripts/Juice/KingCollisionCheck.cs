using UnityEngine;
using System.Collections;

public class KingCollisionCheck : MonoBehaviour {

   void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.name.Equals("King"))
        {
            if(transform.parent.childCount==1)
            {
                CameraScript CS = (CameraScript)Camera.main.GetComponent(typeof(CameraScript));
                Destroy(CS);
                Camera.main.transform.position = new Vector3(6.97f, 9, -10);
                Camera.main.orthographicSize = 13;
                Destroy(GameObject.Find("Canvas"));
            }
            Destroy(gameObject);
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
