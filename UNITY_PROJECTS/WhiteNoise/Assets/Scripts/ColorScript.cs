using UnityEngine;
using System.Collections;

public class ColorScript : MonoBehaviour {

    bool clicked;
    float delay;
    System.Random RNG = new System.Random();
    public int Move_ID;

    public float speed;
    public Vector2 dir;

    private void OnMouseDown()
    {
        Click();
    }

    public void Click()
    {
        ColorChange();
        Destroy(GetComponent<Collider2D>());
        clicked = true;
        delay = RNG.Next(300, 3000) / 1000f;
    }

    // Use this for initialization
    void Start() {

    }

    void ColorChange()
    {
        GetComponent<SpriteRenderer>().color = new Color(RNG.Next(256) / 255f, RNG.Next(256) / 255f, RNG.Next(256) / 255f);
    }

    void move()
    {
        

    }

    private void OnBecameInvisible()
    {
        transform.position = new Vector2(RNG.Next(-7000, 7001) / 1000f, RNG.Next(-4500, 4501) / 1000f);
    }

    // Update is called once per frame
    void Update () {
	if(clicked)
        {
            InvokeRepeating("ColorChange", delay, delay);
            clicked = false;           
        }
    if(Move_ID==1)
        transform.Translate(new Vector2(RNG.Next(-400, 401) / 100f, RNG.Next(-400, 401) / 100f) * Time.deltaTime);
    else if(Move_ID==2)
        {
            if (dir.x * transform.position.x >= 7)
                dir.x *= -1;
            if (dir.y * transform.position.y >= 4.5)
                dir.y *= -1;
            transform.Translate(dir * speed * Time.deltaTime);
        }
    }
}
