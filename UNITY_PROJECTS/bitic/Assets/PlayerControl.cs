using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

    public static PlayerControl singleton;
    public GameObject SelectedMap;
    public bool Selected;
    Vector2 MouseOrigin;
    public MapControl[] StartMaps;
    public MapControl Target;
    public GameObject Map;


    private void Awake()
    {
        singleton = this;
    }

    // Use this for initialization
    void Start () {
        for (int i = 0; i < StartMaps.Length; i++)
            StartMaps[i].RandomMap();
        Target.RandomMap();
    }

    void CreateMap(SpotScript Position)
    {
        GameObject g = Instantiate(Map, Position.transform.position, Quaternion.identity) as GameObject;
        MapControl m = g.GetComponent<MapControl>();
        m.Position = Position;
        Position.SetTaken(true);
        m.RandomMap();
    }
	
	// Update is called once per frame
	void Update () {
	if(Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null && hit.collider.CompareTag("Map"))
            {
                SelectedMap = hit.collider.gameObject;
                MouseOrigin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Selected = true;
            }
            else if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                hit.collider.GetComponent<GateControl>().NextGate(1);
            }
            else if(hit.collider != null && hit.collider.CompareTag("Finish"))
            {
                if(!hit.collider.GetComponent<SpotScript>().Taken)
                    CreateMap(hit.collider.GetComponent<SpotScript>());
            }
        }
    else if(Input.GetMouseButtonDown(1))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                hit.collider.GetComponent<GateControl>().ResolveOperation();
            }
            else if (hit.collider != null && hit.collider.CompareTag("Map"))
            {
                Selected = false;
                hit.collider.GetComponent<MapControl>().Position.SetTaken(false);
                Destroy(hit.collider.gameObject);
            }
        }
    else if (Input.GetMouseButtonUp(0))
        {
            if (Selected)
            {
                Selected = false;
                SelectedMap.transform.position = SelectedMap.GetComponent<MapControl>().Position.transform.position+Vector3.back;
            }
        }
    if(Selected)
        {
            Vector2 Change= (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition)-MouseOrigin;
            SelectedMap.transform.position =(Vector2)SelectedMap.transform.position+Change;
            MouseOrigin = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
	}
}
