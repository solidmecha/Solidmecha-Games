using UnityEngine;
using System.Collections;

public class TransitionScript : MonoBehaviour {
    public Transform Target;
    public Vector2 Origin;
    public Vector2 Scale;
    float Counter;

	// Use this for initialization
	void Start () {
        Origin = transform.position;
        Scale = transform.localScale;
        Target = PlayerControl.singleton.Target.transform;
	}
	
	// Update is called once per frame
	void Update () {
        Counter += Time.deltaTime;
        transform.position = Vector2.Lerp(Origin, Target.position, Counter/2f);
        transform.localScale = Vector2.Lerp(Scale, Target.localScale, Counter/2f);
        if(Counter>=2f)
        {
            PlayerControl.singleton.Target.RandomMap();
            Destroy(gameObject);
        }
    }
}
