using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

    public GameControl GC;
    public float speed;
    public enum MoveState {None, Left, Right, Forward, Backward, Up, Down, Clocktwist, CounterClocktwist};
    bool isEdgeSpin;
    bool EdgeSet;
    MoveState CurrentState;
    Vector3 Target_Pos;
    public GameObject Cross;
    public GameObject Edge;

    float moveTimer;
    bool timeSet;

	// Use this for initialization
	void Start () {
        CurrentState = MoveState.None;
    }
	
	// Update is called once per frame
	void Update () {
        if (CurrentState == MoveState.None)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                Target_Pos = transform.position + Vector3.forward;
                CurrentState = MoveState.Forward;
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                Target_Pos = transform.position + Vector3.left;
                CurrentState = MoveState.Left;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                Target_Pos = transform.position + Vector3.back;
                CurrentState = MoveState.Backward;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                Target_Pos = transform.position + Vector3.right;
                CurrentState = MoveState.Right;
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                CurrentState = MoveState.CounterClocktwist;
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                CurrentState = MoveState.Clocktwist;
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                isEdgeSpin = true;
                CurrentState = MoveState.Forward;                
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                isEdgeSpin = true;
                CurrentState = MoveState.Backward;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                isEdgeSpin = true;
                CurrentState = MoveState.Left;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                isEdgeSpin = true;
                CurrentState = MoveState.Right;
            }
        
        }
	
	}

    private void Move(Vector3 Dir)
    {
        if (!timeSet)
        {
            moveTimer = 1 / speed;
            timeSet = true;
        }
        transform.DetachChildren();
        float Dist = Vector3.Distance(transform.position, Target_Pos);
        transform.Translate(speed * Dir * Time.deltaTime);
        moveTimer -= Time.deltaTime;
        if (moveTimer<=0)
        {
            CurrentState = MoveState.None;
            CenterCross();
            timeSet = false;
            
        }
        CheckHold();
    }

    private void RotateEdge(Vector3 Pos)
    {
        if (!EdgeSet)
        {
            Edge.transform.position = new Vector3(transform.position.x + (Pos.x / 2f), transform.position.y - .5f, transform.position.z + (Pos.z / 2f));
            transform.SetParent(Edge.transform);
            EdgeSet = true;
        }
        
        if (Pos.x == 1)
            Edge.transform.Rotate(new Vector3(0, 0, -speed * 50 * Time.deltaTime));
        else if(Pos.x==-1)
            Edge.transform.Rotate(new Vector3(0, 0, speed * 50 * Time.deltaTime));
        else if (Pos.z == -1)
            Edge.transform.Rotate(new Vector3( -speed *50 * Time.deltaTime,0, 0));
        else if(Pos.z==1)
            Edge.transform.Rotate(new Vector3(speed * 50 * Time.deltaTime,0, 0));
        if ((Edge.transform.eulerAngles.x >= 85 && Edge.transform.eulerAngles.x <= 95) || (Edge.transform.eulerAngles.x <= 275 && Edge.transform.eulerAngles.x>265) ||
            (transform.root.localEulerAngles.z >=85 && transform.root.localEulerAngles.z <= 95) || (transform.root.localEulerAngles.z <= 275 && transform.root.localEulerAngles.z > 265))
        {
            Edge.transform.DetachChildren();
            Edge.transform.localEulerAngles = Vector3.zero;
            CurrentState = MoveState.None;
            isEdgeSpin = false;
            EdgeSet = false;
            transform.DetachChildren();
            transform.localEulerAngles = new Vector3(0, 0, 0);
            CenterCross();
            CheckHold();
        }
    }

    private void CenterCross()
    {
        GC.SnapToGrid();
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        Cross.transform.position = new Vector3(Mathf.Round(Cross.transform.position.x), Mathf.Round(Cross.transform.position.y), Mathf.Round(Cross.transform.position.z));
        Cross.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Cross.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        Cross.transform.eulerAngles = FixAngles(Cross.transform.eulerAngles);
        transform.position = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), Mathf.Round(transform.position.z));
    }

    private Vector3 FixAngles(Vector3 V)
    {
        float x=0, y=0, z=0;
        if (V.x < 5 && V.x > 365)
            x = 0;
        else if (V.x > 85 && V.x < 95)
            x = 90;
        else if (V.x > 175 && V.x < 185)
            x = 180;
        else if (V.x > 265 && V.x < 275)
            x = 270;

        if (V.y < 5 && V.y > 365)
            y = 0;
        else if (V.y > 85 && V.y < 95)
            y = 90;
        else if (V.y > 175 && V.y < 185)
            y = 180;
        else if (V.y > 265 && V.y < 275)
            y = 270;

        if (V.z < 5 && V.z > 365)
            z = 0;
        else if (V.z > 85 && V.z < 95)
            z = 90;
        else if (V.z > 175 && V.z < 185)
            z = 180;
        else if (V.z > 265 && V.z < 275)
            z = 270;

        return new Vector3(x, y, z);
    }

    private void RotatePlayer(Vector3 Dir)
    {
        transform.Rotate(Dir*speed*50*Time.deltaTime);
        if((transform.localEulerAngles.y>=90 && transform.localEulerAngles.y <= 95) || (transform.localEulerAngles.y <= 270 && transform.localEulerAngles.y>265))
            {

            transform.DetachChildren();
            transform.localEulerAngles = new Vector3(0, 0, 0);
            CenterCross();
            CheckHold();
            CurrentState = MoveState.None;
            }
    }

    void CheckHold()
    {
        Vector3[] CheckLocations = new Vector3[6] { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };
        foreach (Vector3 v in CheckLocations)
        {
            Ray ray=new Ray();
            RaycastHit hit;
            ray.origin = transform.position;
            ray.direction = v;
            if(Physics.Raycast(ray, out hit, 1.5f) && hit.collider.CompareTag("Hold"))
            {
                hit.transform.root.SetParent(transform);
            }
        }
    }

        private void FixedUpdate()
    {
        if (!isEdgeSpin)
        {
            switch (CurrentState)
            {
                case MoveState.None:
                    return;
                case MoveState.Left:
                    Move(Vector3.left);
                    break;
                case MoveState.Right:
                    Move(Vector3.right);
                    break;
                case MoveState.Forward:
                    Move(Vector3.forward);
                    break;
                case MoveState.Backward:
                    Move(Vector3.back);
                    break;
                case MoveState.Up:
                    Move(Vector3.up);
                    break;
                case MoveState.Down:
                    Move(Vector3.down);
                    break;
                case MoveState.CounterClocktwist:
                    RotatePlayer(Vector3.down);
                    break;
                case MoveState.Clocktwist:
                    RotatePlayer(Vector3.up);
                    break;
            }
        }
        else
        {
            //edge spin
            switch (CurrentState)
            {
                case MoveState.None:
                    return;
                case MoveState.Left:
                    RotateEdge(Vector3.left);
                    break;
                case MoveState.Right:
                    RotateEdge(Vector3.right);
                    break;
                case MoveState.Forward:
                    RotateEdge(Vector3.forward);
                    break;
                case MoveState.Backward:
                    RotateEdge(Vector3.back);
                    break;
                case MoveState.Up:
                    RotateEdge(Vector3.up);
                    break;
                case MoveState.Down:
                    RotateEdge(Vector3.down);
                    break;
            }
        }
    }
}
