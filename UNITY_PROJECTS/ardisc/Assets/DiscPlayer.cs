using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscPlayer : MonoBehaviour
{

    Vector2 orig;
    public float ForceScale = 2f;

    // Use this for initialization
    void Start()
    {
    }

    private void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
            if (Input.GetMouseButtonDown(0))
                orig = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Input.GetMouseButtonUp(0))
            {
                Vector2 cur = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                cur = (cur - orig);
                if (Vector2.SqrMagnitude(cur) > 4)
                {
                    cur = cur.normalized*2;
                }
                GetComponent<Rigidbody2D>().AddForce(cur*ForceScale, ForceMode2D.Impulse);

            }
    }
}
