using UnityEngine;
using System;

public class FighterScript : MonoBehaviour
{

    public int HP;
    public bool stunned;
    public bool ready;
    public bool crouched;
    public float Speed;
    public int Dir=1;
    public SkillControl[] Skillset;


    // Use this for initialization
    void Start()
    {
        Skillset = GetComponents<SkillControl>();
    }

    public void jump()
    { }

    public void crouch()
    {

    }

    public bool grounded()
    {
        return true;
    }

    void Move(Vector2 v)
    {
        transform.Translate(v * Speed * Time.deltaTime);
    }

    void TossOutHitBox(int index)
    {
        if (!grounded())
        {
            index += 6;
        }
        else if (crouched)
            index += 3;
        Skillset[index].Unleash(Dir);
    }

    // Update is called once per frame
    void Update()
    {
        if (ready && !stunned)
        {
            if (Input.GetKey(KeyCode.RightArrow))
            { Move(Vector2.right); Dir = 1; }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                Move(Vector2.left); Dir = -1;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            { crouch(); }
            if (Input.GetKeyDown(KeyCode.UpArrow) && grounded())
            { jump(); }

            if (Input.GetKeyDown(KeyCode.Q))
            { TossOutHitBox(0); }
            if (Input.GetKeyDown(KeyCode.W))
            { TossOutHitBox(1); }
            if (Input.GetKeyDown(KeyCode.E))
            { TossOutHitBox(2); }

        }
    }
}
