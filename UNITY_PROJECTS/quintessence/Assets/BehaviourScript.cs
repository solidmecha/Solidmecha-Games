using UnityEngine;
using System.Collections.Generic;

public class BehaviourScript : MonoBehaviour {
    System.Random RNG;
    public float[][] Weights;
    public List<ParticleScript> Particles;
    public int count;
    public GameObject Part;
    public Sprite[] sprites;
    public GameObject pip;
    bool placePip;

    private void Awake()
    {
        RNG = new System.Random();
        Weights = new float[5][];
        Particles = new List<ParticleScript>() { };
    }

    // Use this for initialization
    void Start () {

        for(int i=0;i<count; i++)
        {
            int ID = RNG.Next(5);
            GameObject go = Instantiate(Part, new Vector2(RNG.Next(-7, 8), RNG.Next(-4, 5)), Quaternion.identity) as GameObject;
            go.GetComponent<SpriteRenderer>().sprite = sprites[ID];
            go.GetComponent<ParticleScript>().ID = ID;
            Particles.Add(go.GetComponent<ParticleScript>());
        }

        for (int i = 0; i < 5; i++)
        {
            Weights[i] = new float[5];
            for (int j = 0; j < 5; j++)
            {
                Weights[i][j] = (float)RNG.Next(-100, 101) / 110f;
            }
        }
	}

    float CheckInfluence(int particleID1, int ParticleID2)
    {
        if (particleID1 > ParticleID2)
            return Weights[particleID1][ParticleID2];
        else
            return Weights[ParticleID2][particleID1];
    }

    void UpdateParticles()
    {
        for(int i=0;i<Particles.Count;i++)
        {
            for(int j = 0; j <Particles.Count;j++)
            {
                if(i!=j)
                {
                    Vector2 dir = Particles[j].transform.position - Particles[i].transform.position;
                    Particles[i].UpdateDir(dir.normalized * Weights[Particles[i].ID][Particles[j].ID]*(2f/(dir.magnitude+2)));
                    //Particles[i].UpdateDir(dir.normalized * CheckInfluence(Particles[i].ID,Particles[j].ID) * (1f / (dir.magnitude + 1)));
                }
            }
            if(i<5)
                Instantiate(pip, Particles[i].transform.position, Quaternion.identity);
        }

        foreach (ParticleScript p in Particles)
        {
            p.MoveParticle();          
        }
    }
	
	// Update is called once per frame
	void Update () {
        UpdateParticles();
	}
}
