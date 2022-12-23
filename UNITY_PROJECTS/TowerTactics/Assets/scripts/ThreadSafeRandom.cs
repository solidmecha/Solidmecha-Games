using UnityEngine;
using System.Collections;

public class ThreadSafeRandom : MonoBehaviour {
			
		private static System.Random random = new System.Random();
		
		
		
		public static int Next()
			
		{
			
			lock (random)
				
			{
				
				return random.Next();
				
			}
			
		}
		
	}

