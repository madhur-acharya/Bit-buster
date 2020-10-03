using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RNJesus
{
	public static long gerRandomPowO2(long lowerLimit= 0, long upperLimit= 3)
	{
		long pow= (int)Random.Range(lowerLimit,  upperLimit * 3);
		pow= pow % upperLimit;
		return (long)Mathf.Pow(2, pow);
	}

	public static int biasedRandom(List<float> sampleSpace= default(List<float>))
	{
		int count= sampleSpace.Count;
		float total= 0;
		for(int i= 0; i < count; i++)
		{
			total+= sampleSpace[i];
		}

		float random= Random.value * total;

		for(int i= 0; i < count; i++)
		{
			if(random < sampleSpace[i])
			{
				return i;
			}
			random-= sampleSpace[i];
		}
		return 0;
	}
}
