using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BitObject : MonoBehaviour
{
	private float destinationAngle;
	private Vector3 destinationPosition;
	private float maxRadius= 2f;
	private float currentRadius= 2f;
	private float timeScale= .2f;
	private float prevTime= 0;
	private float currentAngle= 0f;
	private bool lerpLinearly= false;

	void Start()
	{
		if(destinationAngle == null)
			destinationAngle= transform.rotation.z;

		Vector3 pos= transform.position;
		currentAngle= Mathf.Atan2(pos.y, pos.x);
	}

	void Update()
	{
		if(prevTime > 1f)
		{
			return;
		};
		prevTime= prevTime + (Time.deltaTime / timeScale);

		if(lerpLinearly == true)
		{
			transform.position= Vector3.Lerp(transform.position, destinationPosition, prevTime);
		}
		else
		{
			float fullCurrentAngle= currentAngle < 0 ? (currentAngle + (Mathf.PI * 2)) : currentAngle;
			float fullDestinationAngle= destinationAngle < 0 ? (destinationAngle + (Mathf.PI * 2)) : destinationAngle;

			float lerpvalue= Mathf.Lerp(fullCurrentAngle, fullDestinationAngle, prevTime);
			transform.position= new Vector3(Mathf.Cos(lerpvalue), Mathf.Sin(lerpvalue), 0f) * maxRadius;
		}
	}

	public void lerpToPosition(float dest)
	{
		destinationAngle= dest;
		prevTime= 0;
		lerpLinearly= false;

		Vector3 pos= transform.position;
		currentAngle= Mathf.Atan2(pos.y, pos.x);
	}

	public void lerpToPosition(Vector3 dest, float rad= 0f)
	{
		destinationPosition= dest;
		prevTime= 0;
		lerpLinearly= true;
	}
}
