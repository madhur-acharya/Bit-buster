using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSystem : MonoBehaviour
{
	public static EventSystem current;	

	void Awake()
	{
		current= this;
	}
	
	public event Action<Vector3> onTouchTrigger;
	public void touchTrigger(Vector3 pos)
	{
		if(onTouchTrigger != null) onTouchTrigger(pos);
	}

}
