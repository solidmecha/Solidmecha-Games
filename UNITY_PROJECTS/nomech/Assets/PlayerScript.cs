using UnityEngine;
using System.Collections.Generic;

public class PlayerScript : MonoBehaviour {
    float[] Speeds;
	// Use this for initialization
	void Start () {
        Speeds = new float[6];
        for (int i = 0; i < Speeds.Length; i++)
        {
            Speeds[i] = GameControl.singleton.RNG.Next(75, 251) / 200f;
        }
        if(GameControl.singleton.RNG.Next(2)==1)
        {
            for (int i = 0; i < Speeds.Length; i++)
            {
                Speeds[i] *= -1;
            }
        }
        int count = GameControl.singleton.RNG.Next(3, 6);
        List<int> nums=new List<int> { 0,1,2,3,4,5,6,7};
        int angle = 45;
        if (GameControl.singleton.RNG.Next(2) == 1)
            angle = 30;
        for (int i=0;i<count;i++)
        {
            int a = GameControl.singleton.RNG.Next(nums.Count);
            GameObject go=Instantiate(GameControl.singleton.Lines, transform.position, Quaternion.Euler(0, 0, nums[a] * angle)) as GameObject;
            go.transform.localScale = new Vector2(GameControl.singleton.RNG.Next(2, 10), .2f);
            go.transform.SetParent(transform);
            nums.RemoveAt(a);
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("hi");
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKey(KeyCode.A))
            transform.Translate(transform.right * -1 * Speeds[0] * Time.deltaTime);
        if (Input.GetKey(KeyCode.D))
            transform.Translate(transform.right * Speeds[1] * Time.deltaTime);
        if (Input.GetKey(KeyCode.W))
            transform.Translate(transform.up * Speeds[2] * Time.deltaTime);
        if (Input.GetKey(KeyCode.S))
            transform.Translate(transform.up * -1 * Speeds[3] * Time.deltaTime);
        if (Input.GetKey(KeyCode.Q))
            transform.Rotate(new Vector3(0,0,180)* -1 * Speeds[4] * Time.deltaTime);
        if (Input.GetKey(KeyCode.E))
            transform.Rotate(new Vector3(0, 0, 180) * Speeds[5] * Time.deltaTime);

    }
}
