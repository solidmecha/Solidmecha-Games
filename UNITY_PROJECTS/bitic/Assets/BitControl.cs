using UnityEngine;
using System.Collections;

public class BitControl : MonoBehaviour {
    public static BitControl singleton;
    public enum Operation { Or, And, Xor, Nand, Nor, Xnor, Not, ShiftLeft, ShiftRight, ShiftUp, ShiftDown, RotClock, RotCounterClock, HorizontalReflect, VertReflect}
    public System.Random RNG = new System.Random();

    public bool Combine(Operation O, bool a, bool b)
    {
        switch(O)
        {
            case Operation.Or: return a | b;
            case Operation.And: return a & b;
            case Operation.Xor: return a ^ b;
            case Operation.Nand: return !(a & b);
            case Operation.Nor: return !(a | b);
            case Operation.Xnor: return !(a ^ b);
            case Operation.Not: return !a;
            default: return false;
        }
    }

    public int[] NewIndicies(Operation O)
    {
        switch (O)
        {
            case Operation.ShiftLeft:
        return new int[16]
       {
                1,2,3,0,
                5,6,7,4,
                9,10,11,8,
                13,14,15,12
       };
            case Operation.ShiftRight:
                return new int[16]
       {
                3,0,1,2,
                7,4,5,6,
                11,8,9,10,
                15,12,13,14
       };
            case Operation.ShiftUp:
                return new int[16]
      {
                4,5,6,7,
                8,9,10,11,
                12,13,14,15,
                0,1,2,3
      };
            case Operation.ShiftDown:
                return new int[16]
      {
               12,13,14,15,
                0,1,2,3,
                4,5,6,7,
                8,9,10,11
      };
            case Operation.RotClock:
                return new int[16]
      {
               12,8,4,0,
               13,9,5,1,
               14,10,6,2,
               15,11,7,3
      };
            case Operation.RotCounterClock:
                return new int[16]
      {
               3,7,11,15,
               2,6,10,14,
               1,5,9,13,
               0,4,8,12
      };
            case Operation.HorizontalReflect:
                return new int[16]
       {
               12,13,14,15,
               8,9,10,11,
               4,5,6,7,
               0,1,2,3
       };
            case Operation.VertReflect:
                return new int[16]
       {
               3,2,1,0,
               7,6,5,4,
               11,10,9,8,
               15,14,13,12
       };
        default: return new int[16];
    }



    }

    public int[] StartMaps(int i)
    {
        switch(i)
        {
            case 0:
                return new int[16]
       {
                0,1,0,1,
                1,0,1,0,
                0,1,0,1,
                1,0,1,0
       };
            case 1:
                return new int[16]
       {
                0,1,0,1,
                0,1,0,1,
                0,1,0,1,
                0,1,0,1
       };
            case 2:
                return new int[16]
        {
                0,0,0,0,
                0,1,1,0,
                0,1,1,0,
                0,0,0,0
        };
            case 3:
                return new int[16]
        {
                0,1,1,0,
                0,1,1,0,
                0,1,1,0,
                0,1,1,0
        };
            case 4:
                return new int[16]
         {
                1,0,0,1,
                0,0,0,0,
                0,0,0,0,
                1,0,0,1
         };
            case 5:
                return new int[16]
         {
                0,0,0,0,
                1,1,1,1,
                0,0,0,0,
                0,0,0,0
         };
            case 6:
                return new int[16]
         {
                0,1,0,0,
                0,1,0,0,
                0,1,0,0,
                0,1,0,0
         };
            case 7:
                return new int[16]
         {
                0,0,0,0,
                1,1,1,1,
                1,1,1,1,
                0,0,0,0
         };
            default: return new int[16]
            {
                0,0,0,0,
                0,0,0,0,
                0,0,0,0,
                0,0,0,0
            };
        }
    }

    private void Awake()
    {
        singleton = this;
    }
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
