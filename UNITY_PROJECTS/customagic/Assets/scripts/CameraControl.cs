using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

    Vector2 MinBounds=new Vector2(-8.08f, -4.06f);
    Vector2 MaxBounds=new Vector2(8.08f, 4.06f);
    Vector2 PlayerPos;
    public Transform minmapLoc;
    public bool inTransition;
    public Vector3 TargetPos;
    public float PanSpeed;
	// Use this for initialization
	void Start () {
	}

    public void Snap(Vector2 PlayerPos)
    {
        while (PlayerPos.y < MinBounds.y)
        {
            inTransition = true;
            TargetPos = (Vector2)TargetPos + Vector2.down * 9.87f;
            TargetPos += Vector3.back * 10;
            MinBounds = MinBounds + Vector2.down * 9.87f;
            MaxBounds = MaxBounds + Vector2.down * 9.87f;
            minmapLoc.localPosition = (Vector2)minmapLoc.localPosition + Vector2.down * 0.87f;
        }
        while(PlayerPos.y > MaxBounds.y)
        {
            inTransition = true;
            TargetPos = (Vector2)TargetPos + Vector2.up * 9.87f;
            TargetPos += Vector3.back * 10;
            MinBounds = MinBounds + Vector2.up * 9.87f;
            MaxBounds = MaxBounds + Vector2.up * 9.87f;
            minmapLoc.localPosition = (Vector2)minmapLoc.localPosition + Vector2.up * 0.87f;
        }
        while(PlayerPos.x < MinBounds.x)
        {
            inTransition = true;
            TargetPos = (Vector2)TargetPos + Vector2.left * 17.14f;
            TargetPos += Vector3.back * 10;
            MinBounds = MinBounds + Vector2.left * 17.14f;
            MaxBounds = MaxBounds + Vector2.left * 17.14f;
            minmapLoc.localPosition = (Vector2)minmapLoc.localPosition + Vector2.left * 1.4f;
        }
        while (PlayerPos.x > MaxBounds.x)
        {
            inTransition = true;
            TargetPos = (Vector2)TargetPos + Vector2.right * 17.14f;
            TargetPos += Vector3.back * 10;
            MinBounds = MinBounds + Vector2.right * 17.14f;
            MaxBounds = MaxBounds + Vector2.right * 17.14f;
            minmapLoc.localPosition = (Vector2)minmapLoc.localPosition + Vector2.right * 1.4f;
            if (minmapLoc.localPosition.x > 4.16f)
                minmapLoc.localPosition = new Vector2(4.16f, minmapLoc.localPosition.y);
        }
        transform.position = TargetPos;
        inTransition = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (WorldControl.singleton.ActivePlayer != null)
        {
            Vector2 PlayerPos = WorldControl.singleton.ActivePlayer.transform.position;
            if (PlayerPos.y < MinBounds.y)
            {
                inTransition = true;
                TargetPos = (Vector2)TargetPos + Vector2.down * 9.87f;
                TargetPos+= Vector3.back * 10;
                MinBounds = MinBounds + Vector2.down * 9.87f;
                MaxBounds= MaxBounds + Vector2.down * 9.87f;
                minmapLoc.localPosition = (Vector2)minmapLoc.localPosition + Vector2.down * 0.87f;
            }
            if (PlayerPos.y > MaxBounds.y)
            {
                inTransition = true;
                TargetPos = (Vector2)TargetPos + Vector2.up * 9.87f;
                TargetPos += Vector3.back * 10;
                MinBounds = MinBounds + Vector2.up * 9.87f;
                MaxBounds = MaxBounds + Vector2.up * 9.87f;
                minmapLoc.localPosition = (Vector2)minmapLoc.localPosition + Vector2.up * 0.87f;
            }
            if (PlayerPos.x < MinBounds.x)
            {
                inTransition = true;
                TargetPos = (Vector2)TargetPos + Vector2.left * 17.14f;
                TargetPos += Vector3.back * 10;
                MinBounds = MinBounds + Vector2.left * 17.14f;
                MaxBounds = MaxBounds + Vector2.left * 17.14f;
                minmapLoc.localPosition = (Vector2)minmapLoc.localPosition + Vector2.left * 1.4f;
            }
            if (PlayerPos.x > MaxBounds.x)
            {
                inTransition = true;
                TargetPos = (Vector2)TargetPos + Vector2.right * 17.14f;
                TargetPos += Vector3.back * 10;
                MinBounds = MinBounds + Vector2.right * 17.14f;
                MaxBounds = MaxBounds + Vector2.right * 17.14f;
                minmapLoc.localPosition = (Vector2)minmapLoc.localPosition + Vector2.right * 1.4f;
                if (minmapLoc.localPosition.x > 4.16f)
                    minmapLoc.localPosition = new Vector2(4.16f, minmapLoc.localPosition.y);
            }
        }
        if(inTransition)
        {
            transform.Translate((TargetPos - transform.position).normalized * Time.deltaTime * PanSpeed);
            if((TargetPos-transform.position).sqrMagnitude<.1f)
            {
                transform.position = TargetPos;
                inTransition = false;
            }
        }
	}
}
