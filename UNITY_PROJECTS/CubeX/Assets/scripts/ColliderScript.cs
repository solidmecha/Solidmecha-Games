using UnityEngine;
using System.Collections.Generic;

public class ColliderScript : MonoBehaviour {

    public int index;
    public CubeControl CC;
    bool inU;
    bool inE;
    bool inD;
    bool inL;
    bool inM;
    bool inR;
    bool inF;
    bool inS;
    bool inB;
    bool inX1;
    bool inX2;
    bool inX3;
    bool inX4;
    bool inX5;
    bool inX6;
    bool inY1;
    bool inY2;
    bool inY3;
    bool inY4;
    bool inY5;
    bool inY6;
    public List<bool> boolList = new List<bool> { };
    void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("L"))
            CC.cubePieces[index] = other.gameObject;
     /*   if (CC.cubePieces.Count > index)
            CC.cubePieces.RemoveAt(index);
        CC.cubePieces.Insert(index,other.gameObject); */
    }

	// Use this for initialization
	void Start () {
        CC = (CubeControl)GameObject.Find("Core").GetComponent(typeof(CubeControl));
        boolList.Add(inU);
        boolList.Add(inE);
        boolList.Add(inD);
        boolList.Add(inL);
        boolList.Add(inM);
        boolList.Add(inR);
        boolList.Add(inF);
        boolList.Add(inS);
        boolList.Add(inB);
        boolList.Add(inX1);
        boolList.Add(inX2);
        boolList.Add(inX3);
        boolList.Add(inX4);
        boolList.Add(inX5);
        boolList.Add(inX6);
        boolList.Add(inY1);
        boolList.Add(inY2);
        boolList.Add(inY3);
        boolList.Add(inY4);
        boolList.Add(inY5);
        boolList.Add(inY6);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
