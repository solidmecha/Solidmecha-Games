using UnityEngine;
using System.Collections;

public class MillScript : MonoBehaviour {

    GameControl GC;
    public float delay;
    float counter;
    public GameObject Resource;
    // Use this for initialization
    void Start()
    {
        GC = (GameControl)GameObject.Find("GameControlOBJ").GetComponent(typeof(GameControl));
    }

    // Update is called once per frame
    void Update()
    {
        if (GC.GrainTime > 0)
        {
            counter += Time.deltaTime;
            GC.GrainTime -= Time.deltaTime;
            RotateScript rs = (RotateScript)transform.GetChild(0).GetComponent(typeof(RotateScript));
            rs.isRotate = true;
        }
        else
        {
            RotateScript rs = (RotateScript)transform.GetChild(0).GetComponent(typeof(RotateScript));
            rs.isRotate = false;
        }
        if (counter >= delay)
        {
            Instantiate(Resource, transform.position, Quaternion.identity);
            counter = 0;
        }
    }
}
