using UnityEngine;
using System.Collections;

public class PictureSelect : MonoBehaviour {

    public int index;
    public bool isCorrect;
    public PicturePuzzle PicPuz;

    void OnMouseDown()
    {
        PicPuz.SelectedObject[index] = gameObject;
        PicPuz.Outlines[index].transform.position = (Vector2)transform.position+GetComponent<BoxCollider2D>().offset;
    }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
