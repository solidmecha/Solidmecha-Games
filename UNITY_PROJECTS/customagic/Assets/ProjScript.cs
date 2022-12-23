using UnityEngine;
using System.Collections;

public class ProjScript : MonoBehaviour {

    public int ID;
    public int Dmg;
    public float speed;
    public GameObject Explo;
    public bool hasExplo;
    public bool isExplo;
    public float maxSize;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CharControl cs = collision.GetComponent<CharControl>();
        if (cs != null && collision.GetComponent<CharControl>().PlayerID == ID)
            return;
        if (collision.CompareTag("W") && collision.GetComponent<WeaponScript>().hostile)
        { Destroy(collision.transform.parent.gameObject); }
        else if (collision.CompareTag("W"))
            return;
        if (hasExplo)
        {
            GameObject go=Instantiate(Explo, transform.position, Quaternion.identity) as GameObject;
            go.GetComponent<ProjScript>().maxSize = maxSize;
        }
        if(!isExplo)
            Destroy(gameObject);
    }


    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if(!isExplo)
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        else
            transform.localScale= (Vector2)transform.localScale+(Vector2.one * speed * Time.deltaTime);
        if (isExplo && transform.localScale.x >= maxSize)
            Destroy(gameObject);

    }
}
