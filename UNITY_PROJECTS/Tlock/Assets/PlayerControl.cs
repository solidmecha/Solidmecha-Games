using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

    public GameControl GC;
    public float speed;
    public enum MoveState {None, Left, Right, Up, Down};
    public MoveState CurrentState;
    Vector2 Target_Pos;
    float moveTimer;
    bool timeSet;
    public Vector2 Start_Pos;
    public int TimeStep;

	// Use this for initialization
	void Start () {
        TimeStep = 1;
        Start_Pos = transform.position;
        CurrentState = MoveState.None;
    }
	
	// Update is called once per frame
	void Update () {
        if (CurrentState == MoveState.None)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                TimeStep = -1;
            }
            if (Input.GetKeyDown(KeyCode.E))
                TimeStep = 1;
            if (Input.GetKeyDown(KeyCode.W))
            {
                Target_Pos = (Vector2)transform.position + Vector2.up;
                CurrentState = MoveState.Up;
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                Target_Pos = (Vector2)transform.position + Vector2.left;
                CurrentState = MoveState.Left;
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                Target_Pos = (Vector2)transform.position + Vector2.down;
                CurrentState = MoveState.Down;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                Target_Pos = (Vector2)transform.position + Vector2.right;
                CurrentState = MoveState.Right;
            }
        
        }
	
	}

    private void Move(Vector2 Dir)
    {
        if (!timeSet)
        {
            moveTimer = 1 / speed;
            timeSet = true;
            if(TimeStep<0)
                GC.MoveIndex += TimeStep;
            GC.MoveBlocks();
            if(TimeStep>0)
                GC.MoveIndex += TimeStep;
        }

        float Dist = Vector2.Distance(transform.position, Target_Pos);
        transform.Translate(speed * Dir * Time.deltaTime);
        moveTimer -= Time.deltaTime;
        transform.GetChild(0).Rotate(new Vector3(0, 0, -30*TimeStep*speed* Time.deltaTime));
        if (moveTimer<=0)
        {
            CurrentState = MoveState.None;
            timeSet = false;
            transform.GetChild(0).localEulerAngles = new Vector3(0, 0, Mathf.Round(transform.GetChild(0).localEulerAngles.z));
            transform.position = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
            GC.SnapBlocks();
        }

    }


        private void FixedUpdate()
    {

            switch (CurrentState)
            {
                case MoveState.None:
                    return;
                case MoveState.Left:
                    Move(Vector2.left);
                    break;
                case MoveState.Right:
                    Move(Vector2.right);
                    break;
                case MoveState.Up:
                    Move(Vector2.up);
                    break;
                case MoveState.Down:
                    Move(Vector2.down);
                    break;

        }

    }
}
