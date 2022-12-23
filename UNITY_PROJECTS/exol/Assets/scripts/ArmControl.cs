using UnityEngine;
using System.Collections;

public class ArmControl : MonoBehaviour {

    public int Mode_ID;
    public bool ExtendingArm;
    public float Speed;
    public ArmControl Partner;
    bool CheckForButtonAhead()
    {
        RaycastHit2D hit=Physics2D.Raycast(transform.root.position + transform.right*.3f, transform.right, 1f);
        //print(hit.collider.name);
        return hit.collider != null && hit.collider.CompareTag("Red");
    }

    public void UseArm()
    {
        switch(Mode_ID)
        {
            case 1: //push
                {
                    RaycastHit2D hitCheck = Physics2D.Raycast(transform.root.position + transform.right, Vector2.zero);
                    RaycastHit2D emptyCheck = Physics2D.Raycast(transform.root.position + transform.right * 2, Vector2.zero);
                    //print((transform.root.position + transform.right).x);
                    //print((transform.root.position + transform.right).y);
                    if (CheckForButtonAhead())
                    {
                        GameObject.FindGameObjectWithTag("GameController").GetComponent<GridControl>().CogsToActivateNextStep.Add(hitCheck.collider.GetComponent<CogControl>());
                        GameObject.FindGameObjectWithTag("GameController").GetComponent<GridControl>().MovingArms.Add(this);
                    }
                    else if (hitCheck.collider != null && !hitCheck.collider.CompareTag("Empty"))
                    {

                        if (emptyCheck.collider == null || emptyCheck.collider.CompareTag("Empty"))
                        {
                            // print("Moving!");
                            GridControl GC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GridControl>();
                            CogControl CC = hitCheck.collider.GetComponent<CogControl>();
                            GC.MovingArms.Add(this);
                            CC.Moving = true;
                            CC.TargetPosition = CC.transform.position + transform.right;
                            GC.CogsToMove.Add(CC);
                        }
                    }
                }
                break;
            case 2: //pull
                {
                    RaycastHit2D emptyCheck = Physics2D.Raycast(transform.root.position + transform.right, Vector2.zero);
                    RaycastHit2D  hitCheck = Physics2D.Raycast(transform.root.position + transform.right * 2, Vector2.zero);
                    //print((transform.root.position + transform.right).x);
                    //print((transform.root.position + transform.right).y);
                     if (hitCheck.collider != null && !hitCheck.collider.CompareTag("Empty"))
                    {

                        if (emptyCheck.collider == null || emptyCheck.collider.CompareTag("Empty"))
                        {
                            // print("Moving!");
                            GridControl GC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GridControl>();
                            CogControl CC = hitCheck.collider.GetComponent<CogControl>();
                            GC.MovingArms.Add(this);
                            CC.Moving = true;
                            CC.TargetPosition = transform.position + transform.right;
                            GC.CogsToMove.Add(CC);
                        }
                    }
                }
                break;
            case 3: //counterclockwise turn
                {
                    RaycastHit2D hitCheck = Physics2D.Raycast(transform.root.position + transform.right, Vector2.zero);
                    if (hitCheck.collider != null && !hitCheck.collider.CompareTag("Empty"))
                    {
                        GridControl GC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GridControl>();
                        CogControl CC = hitCheck.collider.GetComponent<CogControl>();
                        GC.MovingArms.Add(this);
                        CC.Rotating = true;
                        CC.RotationDelta = 60f;
                        GC.CogsToMove.Add(CC);
                    }
                }
                break;
            case 4: //clockwise turn
                {
                    RaycastHit2D hitCheck = Physics2D.Raycast(transform.root.position + transform.right, Vector2.zero);
                    if (hitCheck.collider != null && !hitCheck.collider.CompareTag("Empty"))
                    {
                        GridControl GC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GridControl>();
                        CogControl CC = hitCheck.collider.GetComponent<CogControl>();
                        GC.MovingArms.Add(this);
                        CC.Rotating = true;
                        CC.RotationDelta = -60f;
                        GC.CogsToMove.Add(CC);
                    }
                }
                break;
            case 6: //copy
                {
                    RaycastHit2D emptyCheck = Physics2D.Raycast(transform.position + Partner.transform.right, Vector2.zero);
                    RaycastHit2D hitCheck = Physics2D.Raycast(transform.root.position + transform.right, Vector2.zero);
                    if (hitCheck.collider != null && !hitCheck.collider.CompareTag("Empty"))
                    {

                        if (emptyCheck.collider == null || emptyCheck.collider.CompareTag("Empty"))
                        {
                            GridControl GC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GridControl>();
                            CogControl CC = hitCheck.collider.GetComponent<CogControl>();
                            GC.MovingArms.Add(this);
                            CC.Copied = true;
                            CC.TargetPosition = transform.position + Partner.transform.right;
                            GC.CogsToMove.Add(CC);
                        }
                    }
                }
                break;
            case 7: //destroy
                {
                    RaycastHit2D hitCheck = Physics2D.Raycast(transform.root.position + transform.right, Vector2.zero);
                    if (hitCheck.collider != null && !hitCheck.collider.CompareTag("Empty"))
                    {
                            GridControl GC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GridControl>();
                            CogControl CC = hitCheck.collider.GetComponent<CogControl>();
                            GC.MovingArms.Add(this);
                            GC.CogsToMove.Add(CC);
                    }
                }
                break;
        }
    }



	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	    if(ExtendingArm)
        {
            transform.localPosition+=transform.right* Speed * Time.deltaTime;
        }
	}
}
