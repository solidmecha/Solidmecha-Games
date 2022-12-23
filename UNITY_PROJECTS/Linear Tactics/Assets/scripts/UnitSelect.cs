using UnityEngine;
using System.Collections;

public class UnitSelect : MonoBehaviour {

LineRenderer lRender;
Vector3 startPos;
	// Use this for initialization
	void Start () {
		lRender=GetComponent<LineRenderer>();
	
	}
	
	// Update is called once per frame
	void Update () {
		 if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
		 {
		 	startPos=Input.GetTouch(0).position;
		 }
		 if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved) {
            Vector2 DPos = Input.GetTouch(0).deltaPosition;
            Vector2 av=new Vector2(startPos.x, startPos.y-DPos.y);
            Vector2 bv=new Vector2(startPos.x-DPos.x, startPos.y);
          //  lRender.Positions.Size=4;
          lRender.SetPosition(0, startPos);
          lRender.SetPosition(1, av);
          lRender.SetPosition(2, DPos);
          lRender.SetPosition(3, bv);
             
        }
        	if(Input.GetButtonDown("Fire1"))
        	{
        		startPos=new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z);
        	}
           if (Input.GetButton("Fire1"))
           {
           	
           	 Vector3 DPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z);
            Vector3 bv=new Vector3(startPos.x, startPos.y-(startPos.y-DPos.y), -Camera.main.transform.position.z);
            Vector3 av=new Vector3(startPos.x+(DPos.x-startPos.x), startPos.y, -Camera.main.transform.position.z);
            lRender.SetVertexCount(5);
          lRender.SetPosition(0, Camera.main.ScreenToWorldPoint(startPos));
          lRender.SetPosition(1, Camera.main.ScreenToWorldPoint(av));
          lRender.SetPosition(2, Camera.main.ScreenToWorldPoint(DPos));
          lRender.SetPosition(3,Camera.main.ScreenToWorldPoint( bv));
          lRender.SetPosition(4, Camera.main.ScreenToWorldPoint(startPos));
           		
           }
	}
}
