using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BitObject : MonoBehaviour
{
	private float destinationAngle;
	private float maxRadius= 2f;
	private float currentRadius= 2f;
	private float timeScale= .2f;
	private float prevTime= 0;
	private float currentAngle= 0f;

	private float Currentlerp;

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
		
		float fullCurrentAngle= currentAngle < 0 ? (currentAngle + (Mathf.PI * 2)) : currentAngle;
		float fullDestinationAngle= destinationAngle < 0 ? (destinationAngle + (Mathf.PI * 2)) : destinationAngle;

		float lerpvalue= Mathf.Lerp(fullCurrentAngle, fullDestinationAngle, prevTime);
		Currentlerp= lerpvalue * Mathf.Rad2Deg;
		float lerpRad= Mathf.Lerp(currentRadius, maxRadius, prevTime);
		transform.position= new Vector3(Mathf.Cos(lerpvalue), Mathf.Sin(lerpvalue), 0f) * lerpRad;

	}

	public void lerpToPosition(float dest, float rad= 2f)
	{
		destinationAngle= dest;
		currentRadius= rad;
		prevTime= 0;

		Vector3 pos= transform.position;
		currentAngle= Mathf.Atan2(pos.y, pos.x);
	}
}
