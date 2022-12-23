using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

    public float speed;
    public Transform LockedOnBaby;
    bool isHolding;
   public GameObject BabyOutline;
    GameObject currentOutline;

	// Use this for initialization
	void Start () {
	
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isHolding)
        {
            if (collision.CompareTag("Baby"))
            {
                //add outline and lock on
                if(currentOutline!=null)
                {
                    Destroy(currentOutline);
                }
                LockedOnBaby = collision.transform;
                currentOutline = Instantiate(BabyOutline, LockedOnBaby.position, Quaternion.identity) as GameObject;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!isHolding)
        {
            if (collision.CompareTag("Baby") && LockedOnBaby != null && LockedOnBaby.Equals(collision.transform))
            {
                //add outline and lock on
                if (currentOutline != null)
                {
                    Destroy(currentOutline);
                }
                LockedOnBaby = null;
            }
        }
    }

    void Interact()
    {
        if (!isHolding && LockedOnBaby != null)
        {
            if (LockedOnBaby.GetComponent<BabyScript>().RoomIndex != -1)
                LockedOnBaby.GetComponent<BabyScript>().Room.GetComponent<ObjectScript>().PickupBaby(LockedOnBaby.GetComponent<BabyScript>());
        LockedOnBaby.SetParent(transform);
        LockedOnBaby.localPosition = new Vector2(0, -.3f);
        LockedOnBaby.rotation = Quaternion.identity;
        isHolding = true;
            Destroy(currentOutline);
        }
        else if(isHolding)
        {
            if (LockedOnBaby.gameObject.GetComponent<BabyScript>().Room != null
                && LockedOnBaby.gameObject.GetComponent<BabyScript>().Room.GetComponent<ObjectScript>().index != -1)
            {
                LockedOnBaby.gameObject.GetComponent<BabyScript>().Room.GetComponent<ObjectScript>().SetBaby(LockedOnBaby.gameObject.GetComponent<BabyScript>());
                isHolding = false;
            }
            else if (LockedOnBaby.gameObject.GetComponent<BabyScript>().Room != null && LockedOnBaby.GetComponent<BabyScript>().wants[LockedOnBaby.GetComponent<BabyScript>().wantIndex] == 11)
            {
                Destroy(LockedOnBaby.gameObject);
                DaycareControl.singleton.MaxBabies--;
                isHolding = false;
                LockedOnBaby = null;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(speed * Vector2.up * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        { transform.Translate(speed * Vector2.left * Time.deltaTime); }
        if (Input.GetKey(KeyCode.S))
        { transform.Translate(speed * Vector2.down * Time.deltaTime); }
        if (Input.GetKey(KeyCode.D))
        { transform.Translate(speed * Vector2.right * Time.deltaTime); }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Interact();
        }
    }
}
