using UnityEngine;
using System.Collections;

public class PathScript : MonoBehaviour {

    public float speed;
    public Material ChildMat;

	// Use this for initialization
	void Start () {
        ChildMat = transform.GetChild(0).GetComponent<MeshRenderer>().material;
	}
	
	// Update is called once per frame
	void Update () {
        speed = transform.parent.GetComponent<UnitScript>().speed;
        transform.GetChild(0).localPosition = new Vector3(transform.GetChild(0).localPosition.x - speed * Time.deltaTime/2, 0,-.05f);
        transform.GetChild(0).localScale = new Vector2(transform.GetChild(0).localScale.x - speed * Time.deltaTime, 0.35f);
        if (transform.GetChild(0).localScale.x <= 0)
            Destroy(gameObject);
        ChildMat.mainTextureOffset = new Vector2(ChildMat.mainTextureOffset.x + speed * Time.deltaTime*-3,0);
        ChildMat.mainTextureScale = new Vector2(transform.GetChild(0).localScale.x * 4, 1);
	}
}
