using UnityEngine;
using System.Collections;

public class ShootingScript : MonoBehaviour {

    public GameObject bullet;
    PlayerControl PC;
    public float delay;
    float counter;
    SpriteRenderer SR;

    // Use this for initialization
    void Start () {
        SR = GetComponent<SpriteRenderer>();
        PC = (PlayerControl)GetComponent(typeof(PlayerControl));
	}

    void shoot()
    {
        GameObject go = Instantiate(bullet, transform.position, Quaternion.identity) as GameObject;
        BulletScript bs = (BulletScript)go.GetComponent(typeof(BulletScript));
        bs.direction = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)bs.transform.position;
        bs.direction = bs.direction.normalized;
        bs.id = PC.spriteIndex;
        bs.transform.SetParent(transform);
        go.GetComponent<SpriteRenderer>().sprite = SR.sprite;
    }

    // Update is called once per frame
    void Update () {

        counter += Time.deltaTime;

        if (Input.GetButton("Fire2"))
        {
            if (counter >= delay)
            {
                counter = 0;
                shoot();
            }
        }

    }
}
