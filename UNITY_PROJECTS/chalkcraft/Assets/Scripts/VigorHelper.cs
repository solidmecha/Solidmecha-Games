using UnityEngine;
using System.Collections.Generic;

public class VigorHelper : MonoBehaviour {

    public float Damage;
    bool running;
    float lifeTime=30f;
    List<GameObject> Vigors=new List<GameObject> { };
    public int ChalkCost;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("F"))
        {
            running = false;
            Vector2 v = Vector2.Reflect(-1 * transform.right, collision.transform.up);
            Damage *= 2;
            drawVigor(v, transform.position - transform.right * transform.localScale.x / 50f);
        }
        else if(collision.CompareTag("C"))
        {
            collision.GetComponent<ChalklingControl>().flashRed = true;
            collision.GetComponent<ChalklingControl>().flashNumber = 1;
            collision.GetComponent<ChalklingControl>().HP -= Damage;
            foreach (GameObject g in Vigors)
                Destroy(g);
            Destroy(gameObject);
        }
    }

    void drawVigor(Vector2 result, Vector2 startPos)
    {
        float a = Mathf.Atan2(result.y, result.x) * Mathf.Rad2Deg;
        GameObject Go =
        Instantiate(GameObject.FindGameObjectWithTag("GameController").GetComponent<DrawScript>().plane, startPos, Quaternion.Euler(0, 180, -1 * a))
        as GameObject;
        VigorHelper vh = Go.GetComponent<VigorHelper>();
        GameObject.FindGameObjectWithTag("GameController").GetComponent<DrawScript>().VigorRunner = Go;
        // vh.Vigors = new List<GameObject> { };
        vh.lifeTime = lifeTime;
        vh.Damage = Damage;
        foreach (GameObject g in Vigors)
            vh.Vigors.Add(g);
        Vigors.Clear();
    }

        // Use this for initialization
        void Start () {
        running = true;
        Vigors.Add(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
        if (running)
        {
            Vector2 v = (Vector2)transform.localScale + Vector2.right * Time.deltaTime * 33;
            transform.localScale = v;
            GetComponent<Renderer>().material.mainTextureScale = new Vector3(v.x * 2 / 33, .99f);
            lifeTime -= Time.deltaTime;
            Damage += Time.deltaTime*7;
            if(lifeTime<=0)
            {
                foreach (GameObject g in Vigors)
                    Destroy(g);
                Destroy(gameObject);
            }

        }

    }
}
