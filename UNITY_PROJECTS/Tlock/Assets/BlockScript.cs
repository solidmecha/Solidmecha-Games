using UnityEngine;
using System.Collections.Generic;

public class BlockScript : MonoBehaviour {

    float moveTimer;
    public bool timeSet;
    public float speed;
    public enum MoveState { None, Left, Right, Up, Down };
    Vector2 Target_Pos;
    public MoveState CurrentState;
    public List<MoveState> MoveList=new List<MoveState> { };
    public Vector2 Start_Pos;
    public GameControl GC;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CurrentState = MoveState.None;
        timeSet = false;
    }

    // Use this for initialization
    void Start () {
        Start_Pos = transform.position;
        System.Random RNG = new System.Random(ThreadSafeRandom.Next());
        for(int i=0;i<12;i++)
            MoveList.Add((MoveState)RNG.Next(5));
	}

    private void Move(Vector2 Dir)
    {
        /*
        if (!timeSet)
        {
            moveTimer = 1 / speed;
            timeSet = true;
        } */

      //  float Dist = Vector2.Distance(transform.position, Target_Pos);
        transform.Translate(speed *GC.PC.TimeStep* Dir * Time.deltaTime);

        /*
        moveTimer -= Time.deltaTime;
        if (moveTimer <= 0)
        {
            CurrentState = MoveState.None;
            timeSet = false;           
        } */
    }

        // Update is called once per frame
        void FixedUpdate () {
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
