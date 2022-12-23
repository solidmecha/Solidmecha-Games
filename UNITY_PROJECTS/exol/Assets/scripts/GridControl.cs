using UnityEngine;
using System.Collections.Generic;

public class GridControl : MonoBehaviour {

    public bool WaitingForNextStep;
    public bool WaitingForInput;
    float Counter;
    public List<Vector2> CancelledPoints;
    public List<ArmControl> MovingArms;
    public List<CogControl> CogsToMove;
    public List<CogControl> CogsToActivate;
    public List<CogControl> CogsToActivateNextStep;
    float ArmSpeed;
    public GameObject[] CogParts; //cog, push, pull, Clock, CounterClock, Button, copy, destroy
    System.Random RNG=new System.Random();
    public Transform SelectedCog;
    public Transform Outline;
    public GameObject Tile;

    void MoveArms()
    {
        foreach(ArmControl a in MovingArms)
        {

            if (a != null)
            {
                a.Speed = ArmSpeed;
                a.ExtendingArm = !(ArmSpeed == 0f);
                if (ArmSpeed == 0f)
                {
                    a.transform.localPosition = Vector2.zero;
                }
            }
        }
    }

    void MoveCogs()
    {
        foreach (CogControl c in CogsToMove)
        {
            if (c != null)
            {
                if (c.Moving)
                {
                    c.transform.position = c.TargetPosition;
                    c.Moving = false;
                }
                else if (c.Rotating)
                {
                    for (int i = 0; i < c.transform.childCount; i++)
                        c.transform.GetChild(i).Rotate(new Vector3(0, 0, c.RotationDelta));
                    c.Rotating = false;
                }
                else if (c.Copied)
                {
                    c.Copied = false;
                    Instantiate(c.gameObject, c.TargetPosition, Quaternion.identity);
                }
                else
                    Destroy(c.gameObject);
            }
        }
            CogsToMove.Clear();
    }

    void ActivateCogs()
    {
        foreach(CogControl c in CogsToActivate)
        {
            if(c !=null)
                c.Activate();
        }
    }

    void Cycle()
    {
        if(CogsToActivate.Count==0)
        {
            WaitingForInput = true;
        }
        else
        {
            ActivateCogs();
            CogsToActivate.Clear();
            foreach (CogControl c in CogsToActivateNextStep)
                CogsToActivate.Add(c);
            CogsToActivateNextStep.Clear();
            ArmSpeed = 1f;
            MoveArms();
            ArmSpeed = -1f;
            Invoke("MoveArms", .5f);
            Invoke("StopArms", 1f);
            Invoke("MoveCogs", 1f);
            Invoke("Cycle", 1f);
        }
    }

    void StopArms()
    {
        ArmSpeed = 0;
        MoveArms();
        MovingArms.Clear();
    }

    public void BuildCog(Vector2 Position)
    {
        GameObject go=Instantiate(CogParts[0], Position, Quaternion.identity) as GameObject;
        int ButtonCount = RNG.Next(4);
        if (ButtonCount > 0)
        {
            List<int> Rotations = new List<int> { 0, 60,120, 180, -60, -120 };
            for(int i=0;i<ButtonCount;i++)
            {
                GameObject b = Instantiate(CogParts[5], go.transform) as GameObject;
                int r = RNG.Next(Rotations.Count);
                b.transform.localEulerAngles =new Vector3(0,0,Rotations[r]);
                b.transform.localPosition = Vector2.zero;
                b.transform.localScale = new Vector2(1f, 1f);
                Rotations.RemoveAt(r);
            }
            int ArmCount = 1 + RNG.Next(6 - ButtonCount);
            for(int i=0; i<ArmCount; i++)
            {
                int a = RNG.Next(10);
                GameObject b=null;
                if (a < 4)
                {
                    b = Instantiate(CogParts[1], go.transform) as GameObject;
                }
                else if (a < 6)
                {
                    b = Instantiate(CogParts[2], go.transform) as GameObject;
                }
                else if (a == 6)
                    b = Instantiate(CogParts[3], go.transform) as GameObject;
                else if (a == 7)
                    b = Instantiate(CogParts[4], go.transform) as GameObject;
                else if (a == 8)
                {
                    if (Rotations.Count > 1)
                    {
                        GameObject c = Instantiate(CogParts[6], go.transform) as GameObject;
                        int rot = RNG.Next(Rotations.Count);
                        c.transform.localPosition = Vector2.zero;
                        c.transform.localEulerAngles = new Vector3(0, 0, Rotations[rot]);
                        c.transform.localScale = new Vector2(.8f, .6666f);
                        Rotations.RemoveAt(rot);
                        b = Instantiate(CogParts[6], go.transform) as GameObject;
                        rot = RNG.Next(Rotations.Count);
                        b.transform.localPosition = Vector2.zero;
                        b.transform.localEulerAngles = new Vector3(0, 0, Rotations[rot]);
                        b.transform.localScale = new Vector2(.8f, .6666f);
                        Rotations.RemoveAt(rot);
                        c.GetComponent<ArmControl>().Partner = b.GetComponent<ArmControl>();
                        b.GetComponent<ArmControl>().Partner = c.GetComponent<ArmControl>();
                        i++;
                    }

                }
                else if (a == 9)
                    b = Instantiate(CogParts[7], go.transform) as GameObject;
                if (a != 8)
                {
                    int r = RNG.Next(Rotations.Count);
                    b.transform.localPosition = Vector2.zero;
                    b.transform.localEulerAngles = new Vector3(0, 0, Rotations[r]);
                    b.transform.localScale = new Vector2(.8f, .6666f);
                    Rotations.RemoveAt(r);
                }
            }
        }

    }

	// Use this for initialization
	void Start () {
        BuildGrid();
        //Cycle();
    }

    void BuildGrid()
    {
        for(int x=-7;x<8;x++)
        {
            for(int y=-5;y<6;y++)
            {
                float X = x;
                float Y = y * .866f;
                if (y % 2 != 0)
                {
                    X -= .5f;
                    if(x==7)
                        Instantiate(Tile, new Vector2(7.5f, Y), Quaternion.identity);
                }
                Instantiate(Tile, new Vector2(X, Y), Quaternion.identity);
                if(x != 0 || y !=0)
                {
                    if(RNG.Next(6)==4)
                    {
                        BuildCog(new Vector2(X, Y));
                    }
                }
            }
        }
    }

    void ChangeArm(float a, Transform T)
    {
        int F=FindandClearArmByAngle(a, T);
        if (F != 6) //prevent messing with copy arm pairs
        {
            F++;
            if (F == 6) //prevent building single copier
                F++;
            if (F < CogParts.Length)
            {
              GameObject b = Instantiate(CogParts[F], T) as GameObject;
                b.transform.localPosition = Vector2.zero;
                b.transform.localEulerAngles = new Vector3(0, 0, a);
                if(F==5)
                    b.transform.localScale = new Vector2(1f, 1f);
                else
                    b.transform.localScale = new Vector2(.8f, .6666f);
            }
        }
    }

    int FindandClearArmByAngle(float a, Transform T)
    {
        for(int i=0;i<T.childCount;i++)
        {
            float childAngle = T.GetChild(i).localEulerAngles.z;
            while (childAngle < 0)
                childAngle += 360;
            while (childAngle >= 360)
                childAngle -= 360;
            if(Mathf.Round(childAngle)==Mathf.Round(a))
            {
                if (T.GetChild(i).GetComponent<ArmControl>() == null)
                {
                    Destroy(T.GetChild(i).gameObject);
                    return 5; //button;
                }
                if (T.GetChild(i).GetComponent<ArmControl>().Mode_ID != 6)
                {
                    int Value = T.GetChild(i).GetComponent<ArmControl>().Mode_ID;
                    Destroy(T.GetChild(i).gameObject);
                    return Value;
                }
                else
                    return 6; //copy
            }
        }
        return 0;

    }


    Vector2 FindNearestHex(Vector2 input)
    {
        float Y = 0;
        float X = 0;
        if (input.y >= 0)
        {

            float NextY = .866f;
            while (!(input.y >= Y && input.y < NextY))
            {
                Y = NextY;
                NextY += .8666f;
            }
            if (Mathf.Abs(input.y - Y) > Mathf.Abs(input.y - NextY))
            {
                Y = NextY;
            }
            if (Mathf.Round(Y / .8666f) % 2 != 0)
            {
                float LeftX = Mathf.Round(input.x) - .5f;
                float RightX = Mathf.Round(input.x) + .5f;
                if (Mathf.Abs(input.x - LeftX) > Mathf.Abs(input.x - RightX))
                    X = RightX;
                else
                    X = LeftX;
            }
            else
                X = Mathf.Round(input.x);
        }
        else
        {
            float NextY = -.866f;
            while (!(input.y <= Y && input.y > NextY))
            {
                Y = NextY;
                NextY -= .8666f;
            }
            if (Mathf.Abs(input.y - Y) > Mathf.Abs(input.y - NextY))
            {
                Y = NextY;
            }
            
            if (Mathf.Round(Y / .8666f) % 2 != 0)
            {
                float LeftX = Mathf.Round(input.x) - .5f;
                float RightX = Mathf.Round(input.x) + .5f;
                if (Mathf.Abs(input.x - LeftX) > Mathf.Abs(input.x - RightX))
                    X = RightX;
                else
                    X = LeftX;
            }
            else
            {
                X = Mathf.RoundToInt(input.x);
            }
        }


            return new Vector2 (X,Y);
    }
	
	// Update is called once per frame
	void Update () {
	    if(WaitingForInput && CogsToActivate.Count !=0)
        {
            WaitingForInput = false;
            Cycle();
        }
        if(Input.GetMouseButtonDown(0))
        {
            Vector2 input = FindNearestHex(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            RaycastHit2D hit = Physics2D.Raycast(input, Vector2.zero);
            if(hit.collider !=null)
            {
                if(hit.collider.CompareTag("Cog"))
                {
                    SelectedCog = hit.transform;
                    Outline.position = hit.transform.position;
                }
                if(hit.collider.CompareTag("Omni") && WaitingForInput)
                {
                    CogsToActivate.Add(hit.collider.GetComponent<CogControl>());
                }
            }
            else
            {
                if(SelectedCog != null)
                {
                    SelectedCog.transform.position = FindNearestHex(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                    SelectedCog = null;
                    Outline.position = new Vector3(-10, -10, -10);
                }
                else
                {
                    BuildCog(input);
                }
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            Vector2 input = FindNearestHex(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            RaycastHit2D hit = Physics2D.Raycast(input, Vector2.zero);
            if (hit.collider != null)
            {
                Vector2 dir = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - input;
                float a = Vector2.Angle(Vector2.right, dir);
                a /= 60;
                a = Mathf.Round(a) * 60;
                if (dir.y < 0 && a !=0)
                    a = -1*a+360;
                print(a);
                ChangeArm(a, hit.transform);
            }
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            Application.LoadLevel(0);
        }
	}
}
