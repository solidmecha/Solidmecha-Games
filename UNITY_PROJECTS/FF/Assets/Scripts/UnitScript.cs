using UnityEngine;
using System.Collections;

public class UnitScript : MonoBehaviour {

    public GameObject Target;
    public GameObject shot;
    public float FireDelay;
    float counter;
    public bool Attacking;
    public float speed;
    Vector2 TargetPos;
    public bool isMoving;
    public bool Intercept;
    public GameManager GM;

    void OnMouseDown()
    {
        if (GM.CurrentSelection.Count > 0)
        {
            int l= GM.CurrentSelection.Count;
            for (int i = 0; i < l; i++)
                GM.Deselect(GM.CurrentSelection[0]);
        }
        GM.Select(gameObject);
    }

	// Use this for initialization
	void Start () {
        GM = (GameManager)GameObject.Find("GameManager").GetComponent(typeof(GameManager));
	}
    //TODO: fix current selection after state switch bugs
    public void move(Vector2 V)
    {
        RotateTowards(V);
        Attacking = false;
        isMoving = true;
        TargetPos = V + (Vector2)transform.position;
    }

    public void RotateTowards(Vector2 V)
    {
        if (V.x >= 0)
            transform.eulerAngles = new Vector3(0, 0, -1 * Vector2.Angle(Vector2.up, V));
        else
            transform.eulerAngles = new Vector3(0, 0, Vector2.Angle(Vector2.up, V));
    }
    void Fire()
    {
        GameObject go=Instantiate(shot, transform.position, transform.rotation) as GameObject;
        if (!Intercept)
        {
            ProjectileScript ps = (ProjectileScript)go.GetComponent(typeof(ProjectileScript));
            ps.Target = Target;
        }
        else
        {
            InterceptorScript IS = (InterceptorScript)go.GetComponent(typeof(InterceptorScript));
            IS.Home = gameObject;
            IS.Target = Target;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if(isMoving)
        {
            float d1 = Vector2.Distance(transform.position, TargetPos);
            transform.Translate(Vector2.up * speed * Time.deltaTime);
            float d2 = Vector2.Distance(transform.position, TargetPos);
            if (d1 <= d2)
            {
                isMoving = false;
                transform.position = TargetPos;
                Attacking = true;
            }
        }
        if (Attacking && Target!=null)
        {
            RotateTowards(Target.transform.position - transform.position);
            counter += Time.deltaTime;
            if(counter>=FireDelay)
            {
                counter = 0;
                Fire();
            }
            if (Target == null)
                Attacking = false;
        }

	}
}
