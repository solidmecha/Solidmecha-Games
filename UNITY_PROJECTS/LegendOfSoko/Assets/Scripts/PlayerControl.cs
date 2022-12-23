using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

    public float speed;
    float counter;
    bool isMoving;
    Vector2 Dir;
    Vector2 TargetPos;
    Vector2 TowerPos;
    public GameObject tower;
    bool readyTele;
    public Sprite[] idles;
    int idleIndex;

    // Use this for initialization
    void Start () {
        counter = 1 / speed;
	}

    bool MoveCheck(Vector2 v1, Vector2 v2)
    {
        if (Physics2D.Raycast(v1, Vector2.zero))
        {
            if (Physics2D.Raycast(v1, Vector2.zero).collider.CompareTag("Road"))
                return false;
            if (Physics2D.Raycast(v2, Vector2.zero))
            {
                if ((Physics2D.Raycast(v1, Vector2.zero) && Physics2D.Raycast(v1, Vector2.zero).collider.CompareTag("Tele")) ||
                    (Physics2D.Raycast(v2, Vector2.zero) && Physics2D.Raycast(v2, Vector2.zero).collider.CompareTag("Tele")))
                    return true;
                else if ((Physics2D.Raycast(v1, Vector2.zero).collider.CompareTag("Box") && Physics2D.Raycast(v2, Vector2.zero).collider.CompareTag("Tower")) ||
                    (Physics2D.Raycast(v1, Vector2.zero).collider.CompareTag("Tower") && Physics2D.Raycast(v2, Vector2.zero).collider.CompareTag("Box")))
                    return true;
                else
                    return false;
            }
        }
        return true;
    }

    void CheckTele()
    {
        TowerPos = TargetPos + Dir;
        if (Physics2D.Raycast(TargetPos, Vector2.zero) && Physics2D.Raycast(TargetPos, Vector2.zero).collider.CompareTag("Tele"))
            TargetPos = Physics2D.Raycast(TargetPos, Vector2.zero).collider.GetComponent<TeleportScript>().TelePos;
        else if (Physics2D.Raycast(TargetPos + Dir, Vector2.zero) && Physics2D.Raycast(TargetPos + Dir, Vector2.zero).collider.CompareTag("Tele"))
        {
            if (Physics2D.Raycast(Physics2D.Raycast(TargetPos + Dir, Vector2.zero).collider.GetComponent<TeleportScript>().TelePos + Dir, Vector2.zero))
            {
                RaycastHit2D h = Physics2D.Raycast(Physics2D.Raycast(TargetPos + Dir, Vector2.zero).collider.GetComponent<TeleportScript>().TelePos + Dir, Vector2.zero);
                if ((h.collider.CompareTag("Box") && tower != null && tower.CompareTag("Tower")) || (h.collider.CompareTag("Tower") && tower != null && tower.CompareTag("Box")) )
                    TowerPos = Physics2D.Raycast(TargetPos + Dir, Vector2.zero).collider.GetComponent<TeleportScript>().TelePos + Dir;
            }
            else
                TowerPos = Physics2D.Raycast(TargetPos + Dir, Vector2.zero).collider.GetComponent<TeleportScript>().TelePos + Dir;
        }
    }

    void HandleMove(Vector2 v)
    {
        if (MoveCheck((Vector2)transform.position + v, (Vector2)transform.position + 2 * v))
        {
            if (Physics2D.Raycast((Vector2)transform.position + v, Vector2.zero) &&
                (Physics2D.Raycast((Vector2)transform.position + v, Vector2.zero).collider.CompareTag("Tower") ||
                Physics2D.Raycast((Vector2)transform.position + v, Vector2.zero).collider.CompareTag("Box")))
                tower = Physics2D.Raycast((Vector2)transform.position + v, Vector2.zero).collider.gameObject;
            isMoving = true;
            TargetPos = (Vector2)transform.position + v;
            Dir = v;
            CheckTele();
        }
        StartCoroutine(ResetAnim());
    }

    IEnumerator ResetAnim()
    {
        yield return new WaitForEndOfFrame();
        GetComponent<Animator>().SetInteger("Anim_ID", -1);
    }

    IEnumerator SetIdle()
    {
        yield return new WaitForEndOfFrame();
        GetComponent<Animator>().SetInteger("Anim_ID", 4);
        GetComponent<SpriteRenderer>().sprite = idles[idleIndex];
        StartCoroutine(ResetAnim());
    }

    // Update is called once per frame
    void Update () {
        if (!isMoving)
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                HandleMove(Vector2.left);
                GetComponent<Animator>().SetInteger("Anim_ID", 0);
                idleIndex = 0;
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                HandleMove(Vector2.down);
                GetComponent<Animator>().SetInteger("Anim_ID", 1);
                idleIndex = 1;
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                HandleMove(Vector2.right);
                GetComponent<Animator>().SetInteger("Anim_ID", 2);
                idleIndex = 2;
            }
            else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                HandleMove(Vector2.up);
                GetComponent<Animator>().SetInteger("Anim_ID", 3);
                idleIndex = 3;
            }
        }
        else if(isMoving)
        {
            counter -= Time.deltaTime;
            transform.Translate(Dir * Time.deltaTime * speed);
            if(counter<=0)
            {
                counter = 1 / speed;
                isMoving = false;
                transform.position = TargetPos;
                if(tower != null)
                {
                    Vector2 v = TowerPos;
                    v = new Vector2(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y));
                    tower.transform.position = TowerPos;
                    tower = null;
                }
                StartCoroutine(SetIdle());
            }
        }
    }
}
