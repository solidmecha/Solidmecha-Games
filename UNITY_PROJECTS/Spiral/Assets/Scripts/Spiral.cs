using UnityEngine;
using System.Collections;

public class Spiral : MonoBehaviour {

    public GameObject Circle;

	// Use this for initialization
	void Start () {

        for(int i=1; i<540;i++)
        {
    
            GameObject go=Instantiate(Circle) as GameObject;
            go.transform.SetParent(transform);
            go.transform.localPosition = new Vector2(0, .016f * i);
            transform.eulerAngles=new Vector3(0, 0, i*2);
            go.transform.SetParent(null);
        }
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
