using UnityEngine;
using System.Collections;

public class ClickingScript : MonoBehaviour {
	bool isRotating;
	public float speed;
	void OnMouseDown()
	{
		isRotating=true;
		}
	// Use this for initialization
	void Start () {
		isRotating = false;
	}

void checkBlock()
{
	PuzzleManager.RotatedObjs.Add(gameObject);
	StartCoroutine(Delay());
	if(transform.GetChild(0).gameObject.name.Equals(PuzzleManager.solutionList[PuzzleManager.puzzlePointer]))
	{
		PuzzleManager.puzzlePointer++;
	}
	else
	{
		PuzzleManager.puzzlePointer=0;
		PuzzleManager.rotateBack=true;
		switch(transform.GetChild(1).gameObject.name)
		{
			case "circle(Clone)":
			PuzzleManager.shuffle(0,gameObject);
			break;
			case "triangle(Clone)":
			PuzzleManager.shuffle(1,gameObject);
			break;
			case "square(Clone)":
			PuzzleManager.shuffle(2,gameObject);
			break;
		}
	}
}	

IEnumerator Delay()
{
	yield return new WaitForSeconds(2);
}

	// Update is called once per frame
	void Update () {

	if (isRotating) {

			transform.Rotate(Vector3.up * Time.deltaTime*speed);
			if(transform.localEulerAngles.y<=9 || transform.localEulerAngles.y>=351)
			{
				transform.localEulerAngles=Vector3.zero;
				checkBlock();
				isRotating=false;
			}
				}
	}
}
