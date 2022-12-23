using UnityEngine;
using System.Collections.Generic;

public class LevelHashGenerator {



    public string GenerateHash(int[][] InputWorld, int w, int h)
    {
        string hash="";

        for(int i=0;i<w;i++)
        {
            for(int j=0; j<h; j++)
            {
                hash = hash + ConvertToLetter(InputWorld[i][j]);

            }
        }

        return hash;
    }
    
    public string ConvertToLetter(int a)
    {
        string conversion =""; 

        if(a>9)
        {
            switch(a)
            {
                case 10: conversion = "A";
                    break;
                case 11:
                    conversion = "B";
                    break;
                case 12:
                    conversion = "C";
                    break;
                case 13:
                    conversion = "D";
                    break;
                case 14:
                    conversion = "E";
                    break;
                case 15:
                    conversion = "F";
                    break;
                case 16:
                    conversion = "G";
                    break;
                case 17:
                    conversion = "H";
                    break;

            }
        }
        else
        {
            conversion = a.ToString();
        }

        return conversion;
    }


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
