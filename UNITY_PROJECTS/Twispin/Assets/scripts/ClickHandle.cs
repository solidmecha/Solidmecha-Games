using UnityEngine;
using System.Collections.Generic;

public class ClickHandle : MonoBehaviour {

    public List<int> ChildLineIndex;
    LineRenderer Lines;
    void OnMouseDown()
    {
        if (GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>().CurrentSelected == null)
        {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>().SetSelection(gameObject);
        }
        else
        {
            Transform t = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>().CurrentSelected.transform;
            if (t.parent.Equals(transform))
            {
                if(!ChildLineIndex.Contains(t.transform.GetSiblingIndex()))
                    ChildLineIndex.Add(t.GetSiblingIndex());
            }
            else if (transform.parent.Equals(t))
            {
                ClickHandle C = t.gameObject.GetComponent<ClickHandle>();
                if (!C.ChildLineIndex.Contains(transform.GetSiblingIndex())) ;
                    C.ChildLineIndex.Add(transform.GetSiblingIndex());
   
            }
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GameControl>().ClearSelected();
        }
    }

	// Use this for initialization
	void Start () {
        Lines = GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector3[] pos = new Vector3[ChildLineIndex.Count*2];
	    for(int i=0; i<ChildLineIndex.Count*2;i+=2)
        {
            pos[i]= transform.GetChild(ChildLineIndex[i/2]).position;
            pos[i + 1] = transform.position;
        }
        if (ChildLineIndex.Count > 0)
        {
            Lines.SetVertexCount(ChildLineIndex.Count * 2);
            Lines.SetPositions(pos);
        }
	}
}
