using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controler : MonoBehaviour
{
	private float touchRate= 0.25f;
	private float canTouch= -1f;
	private Vector3 touchPos;

	void Start()
	{

	}

	void Update()
	{
		if(Input.touchCount > 0 && Time.time > canTouch)
		{	
			Touch touch= Input.GetTouch(0);
			touchPos= Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
			touchPos.z= 0;

			if(touch.phase == TouchPhase.Ended)
			{
				EventSystem.current.touchTrigger(touchPos);
				canTouch= Time.time + touchRate;
			}
		}
	}
}
