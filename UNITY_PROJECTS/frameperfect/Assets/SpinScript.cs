using UnityEngine;
using System.Collections;

public class SpinScript : MonoBehaviour {
    float counter;
    float FrameTime;
    bool Ready;
    // Use this for initialization
    void Start () {
        FrameTime = 1f / FrameControl.singleton.FPS;
    }
	
	// Update is called once per frame
	void Update () {

        bool pushFrame = false;
        counter += Time.deltaTime;
        if (counter >= FrameTime)
        {
            pushFrame = true;
            counter = 0;
        }
        if (pushFrame)
        {
            transform.Rotate(new Vector3(0, 0, 90 * FrameTime));
            int z = Mathf.RoundToInt(transform.eulerAngles.z);
            Ready = z == 180 || z == -180 || z == 0;
        }
        if (Ready && Input.anyKeyDown)
        {
            FrameControl.singleton.win();
            Destroy(transform.parent.gameObject);
        }
    }
}
