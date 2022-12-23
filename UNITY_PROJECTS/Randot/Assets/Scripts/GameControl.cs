using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour {
    public List<DotControl> DClist;
    public List<int> DotOdds;
    public Vector3 LastPoint;
    public GameObject dot;
    System.Random RNG;

	// Use this for initialization
	void Start () {
        RNG = new System.Random();
        SetDotOdds();
        //StartCoroutine(GenDots());
    }

    public void SetDotOdds()
    {
        int Z = 0;
        for (int i = 0; i<DClist.Count;i++)
        {
            DotOdds.Add(DClist[i].Weight+Z);
            Z = DotOdds[i];
        }
    }

    Vector3 GetTargetDot()
    {
        int R = RNG.Next(DotOdds[DotOdds.Count - 1] + 1);
        int index=-1;
        for(int i=0;i<DotOdds.Count;i++)
        {
            if(DotOdds[i]>=R)
            {
                index = i;
                break;
            }
        }
        return DClist[index].transform.position;
    }

    public IEnumerator GenDots()
    {
        while (true)
        {
            yield return null;
        }
    }
	
	// Update is called once per frame
	void Update () {


        for (int i = 0; i < 20; i++)
        {
            Vector2 Pos = (LastPoint + GetTargetDot()) / 2f;
            Instantiate(dot, Pos, Quaternion.identity);
            LastPoint = Pos;
        }

    }
}
