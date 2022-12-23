using UnityEngine;
using System.Collections;

public class RotateIt : MonoBehaviour {
    public Vector3 Rotation;
    float counter = 0;
    public bool vertical;
    public bool horizontal;

	// Use this for initialization
	void Start () {
    }

	// Update is called once per frame
	void Update () {
        counter += Time.deltaTime;
        transform.Rotate(Rotation * Time.deltaTime);
        if (counter+Time.deltaTime >= 1f)
        {
            transform.localEulerAngles = Vector3.zero;
            GetComponent<TileControl>().UpdateColors();
            Destroy(this);
        }
	
	}
}
