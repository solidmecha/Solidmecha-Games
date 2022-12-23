using UnityEngine;
using System.Collections.Generic;

public class BoostHelper {

    public Vector2[] Diamond = new Vector2[4] { new Vector2(1, 1), new Vector2(-1, 1), new Vector2(1, -1), new Vector2(-1, -1) };
    public Vector2[] Cross = new Vector2[4] { new Vector2(1, 0), new Vector2(-1, 0), new Vector2(0, 1), new Vector2(0, -1) };
    public List<List<int[]>> Cone = new List<List<int[]>> { };

    public BoostHelper()
    {
    }

    public void ConeUp()
        {
        Cone.Add(
    new List<int[]> { new int[2] { 1, 1 }, new int[2] { 0, 1 }, new int[2] { -1, 1 } }
    );
        Cone.Add(
            new List<int[]> { new int[2] { 1, 2 }, new int[2] { 0, 2 }, new int[2] { -1, 2 }, new int[2] { 2, 2 }, new int[2] { -2, 2 } }
            );
        Cone.Add(
            new List<int[]> { new int[2] { 1, 3 }, new int[2] { 0, 3 }, new int[2] { -1, 3 }, new int[2] { 2, 3 }, new int[2] { -2, 3 }, new int[2] { 3, 3 }, new int[2] { -3, 3 } }
            );
    }

    public void ConeDown()
    {
        Cone.Add(
    new List<int[]> { new int[2] { 1, -1 }, new int[2] { 0, -1 }, new int[2] { -1, -1 } }
    );
        Cone.Add(
            new List<int[]> { new int[2] { 1, -2 }, new int[2] { 0, -2 }, new int[2] { -1, -2 }, new int[2] { 2, -2 }, new int[2] { -2, -2 } }
            );
        Cone.Add(
            new List<int[]> { new int[2] { 1, -3 }, new int[2] { 0, -3 }, new int[2] { -1, -3 }, new int[2] { 2, -3 }, new int[2] { -2, -3 }, new int[2] { 3, -3 }, new int[2] { -3, -3 } }
            );
    }

    public void ConeForward()
    {
        Cone.Add(
    new List<int[]> { new int[2] { 1, 1 }, new int[2] { 1, 0 }, new int[2] { 1, -1 } }
    );
        Cone.Add(
            new List<int[]> { new int[2] { 2, 1 }, new int[2] { 2, 0 }, new int[2] { 2, -1 }, new int[2] { 2, 2 }, new int[2] { 2, -2 } }
            );
        Cone.Add(
            new List<int[]> { new int[2] { 3, 1 }, new int[2] { 3, 0 }, new int[2] { 3, -1 }, new int[2] { 3, 2 }, new int[2] { 3, -2 }, new int[2] { 3, 3 }, new int[2] { 3, -3 } }
            );
    }

    public void ConeBack()
    {
        Cone.Add(
    new List<int[]> { new int[2] { -1, 1 }, new int[2] { -1, 0 }, new int[2] { -1, -1 } }
    );
        Cone.Add(
            new List<int[]> { new int[2] { -2, 1 }, new int[2] { -2, 0 }, new int[2] { -2, -1 }, new int[2] { -2, 2 }, new int[2] { -2, -2 } }
            );
        Cone.Add(
            new List<int[]> { new int[2] { -3, 1 }, new int[2] { -3, 0 }, new int[2] { -3, -1 }, new int[2] { -3, 2 }, new int[2] { -3, -2 }, new int[2] { -3, 3 }, new int[2] { -3, -3 } }
            );
    }

    public void ConeUpDiag()
    {
        Cone.Add(
    new List<int[]> { new int[2] { 1, 0 }, new int[2] { 0, 1 } }
    );
        Cone.Add(
            new List<int[]> { new int[2] { 2, 0 }, new int[2] { 1, 1 }, new int[2] { 0, 2 } }
            );
        Cone.Add(
            new List<int[]> { new int[2] { 3, 0 }, new int[2] { 1, 2 }, new int[2] { 2, 1 }, new int[2] { 0, 3 }}
            );
    }

    public void ConeDownDiag()
    {
        Cone.Add(
    new List<int[]> { new int[2] { 1, 0 }, new int[2] { 0, -1 } }
    );
        Cone.Add(
            new List<int[]> { new int[2] { 2, 0 }, new int[2] { 1, -1 }, new int[2] { 0, -2 } }
            );
        Cone.Add(
            new List<int[]> { new int[2] { 3, 0 }, new int[2] { 1, -2 }, new int[2] { 2, -1 }, new int[2] { 0, -3 } }
            );
    }

    public void ConeBackUPDiag()
    {
        Cone.Add(
    new List<int[]> { new int[2] { 0, 1}, new int[2] { -1,0 } }
    );
        Cone.Add(
            new List<int[]> { new int[2] { 0, 2}, new int[2] { -1,1 }, new int[2] { -2,0 } }
            );
        Cone.Add(
            new List<int[]> { new int[2] { 0,3 }, new int[2] { -2, 1 }, new int[2] { -1,2 }, new int[2] {-3,0 } }
            );
    }

    public void ConeBackDownDiag()
    {
        Cone.Add(
    new List<int[]> { new int[2] { 0, -1 }, new int[2] { -1, 0 } }
    );
        Cone.Add(
            new List<int[]> { new int[2] { 0, -2 }, new int[2] { -1, -1 }, new int[2] { -2, 0 } }
            );
        Cone.Add(
            new List<int[]> { new int[2] { 0, -3 }, new int[2] { -2, -1 }, new int[2] { -1, -2 }, new int[2] { -3, 0 } }
            );
    }


    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
