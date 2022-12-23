using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

    public GameControl GC;
    public bool Breaking;
    public int BreakID;
    public int BreakCount;
    public Transform SelectedModule;
    Vector2 SelectionOffset;
    System.Random RNG = new System.Random();
	// Use this for initialization
	void Start () {

	}

    // Update is called once per frame
    void Update()
    {
        if (!Breaking)
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit && hit.collider.CompareTag("Module"))
                {
                    SelectedModule = hit.collider.transform.root;
                    SelectionOffset = SelectedModule.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                }
            }
            else if (Input.GetMouseButton(0) && SelectedModule != null)
            {
                Vector2 v = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) + SelectionOffset;
                v = new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
                SelectedModule.position = v;
            }
            else if (Input.GetMouseButtonUp(0))
                SelectedModule = null;
            if ((Input.GetKeyDown(KeyCode.R) || Input.GetMouseButtonDown(1)) && SelectedModule != null)
            {
                SelectedModule.Rotate(new Vector3(0, 0, 90));
                for (int i = 0; i < SelectedModule.childCount; i++)
                {
                    if(SelectedModule.GetChild(i).CompareTag("Module"))
                        SelectedModule.GetChild(i).Rotate(new Vector3(0, 0, -90));
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit && hit.collider.CompareTag("Module"))
                {
                    NodeID n = hit.collider.GetComponent<NodeID>();
                    if (n.Charged && n.ID == BreakID && !n.Destroyed)
                    {
                        n.Discharge();
                        BreakCount--;
                        if (BreakCount == 0)
                        {
                            if (GC.TargetSubRoutine.transform.GetSiblingIndex()<GC.IceEncountered.transform.childCount-1) //typically sword break
                            {
                                GC.Reticle.transform.position = new Vector3(10, 10, 10);
                                GC.TargetSubRoutine.Broken = true;
                                Instantiate(GC.BrokenStrikeout, GC.TargetSubRoutine.transform.position, Quaternion.identity, GC.TargetSubRoutine.transform);
                                GC.UpdateCredit(-50 * GC.TargetSubRoutine.BreakCount);
                                GC.TargetSubRoutine = null;
                            }
                            else //last subroutine
                            {
                                GC.IceEncountered.Turns--;
                                GC.IceEncountered.transform.GetChild(GC.IceEncountered.transform.childCount - 1).GetComponent<SubroutineSelect>().SetToolTip();
                                BreakCount = GC.TargetSubRoutine.BreakCount;
                            }
                            GC.UseClick();
                        }
                    }
                    else if(BreakCount>1 && n.ID==1 && n.Charged && !n.Destroyed) //virus reduce
                    {
                        n.Discharge();
                        GC.TargetSubRoutine.BreakCount--;
                        GC.TargetSubRoutine.SetToolTip();
                        BreakCount--;
                        GC.UseClick();

                    }
                    else if(n.ID==4 && n.Charged && !n.Destroyed) //power
                    {
                        n.Discharge();
                        n.Recharge(Vector2.right);
                        n.Recharge(Vector2.left);
                        n.Recharge(Vector2.up);
                        n.Recharge(Vector2.down);
                        GC.UseClick();
                    }
                    else if( n.ID==3 && !n.Destroyed) //wrench
                    {
                        n.DestroyedIcon = Instantiate(GC.IceEncountered.WreckedToken, n.transform.position, Quaternion.identity, n.transform) as GameObject;
                        n.Destroyed = true;
                        n.Repair(Vector2.right);
                        n.Repair(Vector2.left);
                        n.Repair(Vector2.up);
                        n.Repair(Vector2.down);
                        GC.UseClick();
                    }

                    else if(n.ID==0 && !n.Destroyed && n.Charged && !GC.TargetSubRoutine.isReadable)
                    {
                        GC.TargetSubRoutine.SetReadable();
                        n.Discharge();
                        GC.UseClick();
                    }
                    else if(n.ID==7 && !n.Destroyed && n.Charged)
                    {
                        n.Discharge();
                        GC.UpdateCredit(-100* RNG.Next(1, 4));
                        GC.UseClick();
                    }
                }
            }
        }
    }
}
