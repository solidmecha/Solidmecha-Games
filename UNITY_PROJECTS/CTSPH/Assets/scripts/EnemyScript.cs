using UnityEngine;
using System.Collections.Generic;
using System;

public class EnemyScript : MonoBehaviour {

    public float speed;
    public List<Sprite> SpriteList;
    public Vector2 direction;
    public GameObject bullet;
    public int id;
    public Action shootingStye;
    public float delay;
    float counter;
    public SpriteRenderer SR;
    System.Random RNG;

    void OnCollisionEnter2D(Collision2D coll)
    {
        print("enemy hit");
        if(coll.gameObject.name.Equals("bullet(Clone)"))
        {
            BulletScript bs=(BulletScript)coll.gameObject.GetComponent(typeof(BulletScript));
            if(bs.id==id)
            {
                Destroy(coll.gameObject);
                    Destroy(gameObject);
            }
        }
    }

	// Use this for initialization
	void Start () {
        RNG = new System.Random(ThreadSafeRandom.Next());
        SR = GetComponent<SpriteRenderer>();
        id = RNG.Next(SpriteList.Count);
        SR.sprite = SpriteList[id];
        direction = new Vector2(RNG.Next(-10,11), RNG.Next(-10,11));
        direction = direction.normalized;
        //speed = 2f+(float)RNG.NextDouble()*3f;
        int r = RNG.Next(3);
        switch(r)
        {
            case 0:
                shootingStye = vertShot;
                break;
            case 1:
                shootingStye = horizontalShot;
                break;
            case 2:
                shootingStye = QuadShot;
                break;
        }
	}

    public void rightShot()
    {

        GameObject go = Instantiate(bullet, transform.position, Quaternion.identity) as GameObject;
        BulletScript bs = (BulletScript)go.GetComponent(typeof(BulletScript));
        bs.direction = Vector2.right;
        bs.id = id;
        go.GetComponent<SpriteRenderer>().sprite = SR.sprite;
    }

    public void leftShot()
    {

        GameObject go = Instantiate(bullet, transform.position, Quaternion.identity) as GameObject;
        BulletScript bs = (BulletScript)go.GetComponent(typeof(BulletScript));
        bs.direction = Vector2.left;
        bs.id = id;
        go.GetComponent<SpriteRenderer>().sprite = SR.sprite;
    }

    public void upShot()
    {

        GameObject go = Instantiate(bullet, transform.position, Quaternion.identity) as GameObject;
        BulletScript bs = (BulletScript)go.GetComponent(typeof(BulletScript));
        bs.direction = Vector2.up;
        bs.id = id;
        go.GetComponent<SpriteRenderer>().sprite = SR.sprite;
    }

    public void downShot()
    {

        GameObject go = Instantiate(bullet, transform.position, Quaternion.identity) as GameObject;
        BulletScript bs = (BulletScript)go.GetComponent(typeof(BulletScript));
        bs.direction = Vector2.down;
        bs.id = id;
        go.GetComponent<SpriteRenderer>().sprite = SR.sprite;
    }

    public void horizontalShot()
    {
        leftShot();
        rightShot();
    }
    public void vertShot()
    {
        upShot();
        downShot();
    }
    public void QuadShot()
    {
        vertShot();
        downShot();
    }

    // Update is called once per frame
    void Update () {
        counter += Time.deltaTime;
        if(counter>=delay)
        {
            shootingStye();
            counter = 0;
        }
        transform.Translate(direction * Time.deltaTime * speed);
	
	}
}
