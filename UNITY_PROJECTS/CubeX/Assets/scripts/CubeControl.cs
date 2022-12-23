using UnityEngine;
using System.Collections.Generic;

public class CubeControl : MonoBehaviour {

    public bool inTurnMode=false;
    public bool rotateCore;
    Vector2 OrigMpos;
    public float rotationInc;
    public GameObject cube;
    public GameObject colliderChecks;
    public GameObject Layers;
    public GameObject rotationObject;
    public List<GameObject> cubePieces = new List<GameObject> { };

	// Use this for initialization
	void Start () {
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < colliderChecks.transform.GetChild(i).childCount; j++)
            {
                cubePieces.Add(cube.transform.GetChild(0).GetChild(j).gameObject);
                ColliderScript cs = (ColliderScript)colliderChecks.transform.GetChild(i).GetChild(j).gameObject.GetComponent(typeof(ColliderScript));
                cs.index = cubePieces.Count - 1;
            }
        }
	}

   public void CubeRotation(int C, int neg1forCounterClock)
    {
        print(C);
        List<Transform> RotationList = new List<Transform> { };
        List<Transform> ParentList = new List<Transform> { };
        Vector3 rotVec = Vector3.zero;
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 16; j++)
            {
                ColliderScript cs = (ColliderScript)colliderChecks.transform.GetChild(i).GetChild(j).gameObject.GetComponent(typeof(ColliderScript));
                if (cs.boolList[C])
                {
                    RotationList.Add(cubePieces[cs.index].transform);
                    ParentList.Add(cubePieces[cs.index].transform.parent);
                }
            }
        }
       // print(RotationList.Count);
        if (C < 3)
            rotVec = new Vector3(0, 90 * neg1forCounterClock, 0);
        else if (C < 6)
            rotVec = new Vector3(90 * neg1forCounterClock, 0, 0);
        else if (C < 9)
            rotVec = new Vector3(0, 0, 90 * neg1forCounterClock);
        else if (C < 11)
        {
            rotationObject.transform.parent.localEulerAngles = new Vector3(0, 0, 45);
            rotVec = new Vector3(0, 90, 0);
         }
        else if(C<13)
        {
            rotationObject.transform.parent.localEulerAngles = new Vector3(45, 0, 0);
            rotVec = new Vector3(0, 180, 0);
        }
        else if (C < 15)
        {
            rotationObject.transform.parent.localEulerAngles = new Vector3(0, 45, 0);
            rotVec = new Vector3(180, 0, 0);
        }
        else if(C<17)
        {
            rotationObject.transform.parent.localEulerAngles = new Vector3(0, 0, 135);
            rotVec = new Vector3(0, 180, 0);
        }
        else if (C < 19)
        {
            rotationObject.transform.parent.localEulerAngles = new Vector3(135, 0, 0);
            rotVec = new Vector3(0, 180, 0);
        }
        else if (C < 21)
        {
            rotationObject.transform.parent.localEulerAngles = new Vector3(0, 135, 0);
            rotVec = new Vector3(180, 0, 0);
        }
        foreach (Transform t in RotationList)
        {
            t.SetParent(rotationObject.transform);
        }
        rotationObject.transform.localEulerAngles = rotVec;
       // rotationObject.transform.position = new Vector3(5, 5, 0);
        for(int i=0;i<ParentList.Count;i++)
        {
            RotationList[i].SetParent(ParentList[i]);
        }
        rotationObject.transform.localEulerAngles = Vector3.zero;
        rotationObject.transform.parent.localEulerAngles = Vector3.zero;

    }
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetMouseButtonDown(0) && !inTurnMode && !rotateCore)
        {
            rotateCore = true;
            OrigMpos = Input.mousePosition;
        }

        if(Input.GetMouseButton(0) && rotateCore)
        {
            Vector2 V = (Vector2)Input.mousePosition - OrigMpos;
            transform.eulerAngles+=new Vector3 (V.y*rotationInc, -V.x*rotationInc, 0);
            OrigMpos = Input.mousePosition;
            transform.DetachChildren();
            transform.eulerAngles = Vector3.zero;
            cube.transform.SetParent(transform);
            colliderChecks.transform.SetParent(transform);
            Layers.transform.SetParent(transform);
        }
        else
        {
            rotateCore = false;
        }

        if(Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Physics.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward, out hit);
            
        }

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            CubeRotation(0, 1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            CubeRotation(1, 1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            CubeRotation(2, 1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            CubeRotation(3, 1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            CubeRotation(4, 1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            CubeRotation(5, 1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            CubeRotation(6, 1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            CubeRotation(7, 1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            CubeRotation(8, 1);
        }
        else if(Input.GetKeyDown(KeyCode.E))
        {
            CubeRotation(9, 1);
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            CubeRotation(10, 1);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            CubeRotation(11, 1);
        }
        else if (Input.GetKeyDown(KeyCode.F))
        {
            CubeRotation(12, 1);
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            CubeRotation(13, 1);
        }
        else if (Input.GetKeyDown(KeyCode.V))
        {
            CubeRotation(14, 1);
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            CubeRotation(15, 1);
        }
        else if (Input.GetKeyDown(KeyCode.U))
        {
            CubeRotation(16, 1);
        }
        else if (Input.GetKeyDown(KeyCode.H))
        {
            CubeRotation(17, 1);
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            CubeRotation(18, 1);
        }
        else if (Input.GetKeyDown(KeyCode.N))
        {
            CubeRotation(19, 1);
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            CubeRotation(20, 1);
        }
    }
}
