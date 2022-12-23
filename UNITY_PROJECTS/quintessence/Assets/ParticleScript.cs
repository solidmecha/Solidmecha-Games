using UnityEngine;
using System.Collections;

public class ParticleScript : MonoBehaviour {
    public int ID;
    public Vector2 Dir;
    public Vector2 init_Vec;

	// Use this for initialization
	void Start () {
	
	}

    public void UpdateDir(Vector2 v)
    {
        Dir += v;
    }

    public void MoveParticle()
    {
        transform.position =(Vector2)transform.position+ (Dir * Time.deltaTime);
        if (transform.position.sqrMagnitude > 100000)
        {
            transform.position = Vector2.zero;
            Dir = init_Vec;
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
