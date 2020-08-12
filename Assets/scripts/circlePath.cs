using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class circlePath : MonoBehaviour
{
	public int vertexCount= 6;
	public float lineWidth= 0.2f;
	public float radius= 1f;
	public GameObject obj;
	public GameObject theLine;

	private LineRenderer lineRenderer;
	private List<GameObject> objectList= new List<GameObject>();
	private Vector3 center;

	public float touchAngle= 0f;
	public int touchIndex= 1;

	void Awake()
	{
		lineRenderer= GetComponent<LineRenderer>();
	}

	void Start()
	{
		EventSystem.current.onTouchTrigger+= addItem;
		center= transform.position;

		for(int i= 0; i < vertexCount; i ++)
		{
			GameObject circle= Instantiate(obj, new Vector3(0, 0, 0), transform.rotation); 
			circle.transform.SetParent(transform);
			circle.GetComponent<SpriteRenderer>().color= new Color( Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
			objectList.Add(circle);
		}

		updatePoints();
	}

	void Update()
	{
		if(Input.touchCount <= 0) return;

		Touch touch= Input.GetTouch(0);
		Vector3 touchPos= Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
		touchPos.z= 0;

		float delta = (2f * Mathf.PI) / (vertexCount);

		if(touch.phase == TouchPhase.Moved)
		{
			float ang= Mathf.Atan2(touchPos.y, touchPos.x);
			touchAngle= ang;
			float aproxIndex= ang / delta;

			Debug.DrawLine (transform.position, new Vector3(Mathf.Cos(ang), Mathf.Sin(ang)) * radius);

			ang= (Mathf.Floor(aproxIndex) * delta) + (delta / 2);
			
			lineRenderer.SetPosition(1, new Vector3(Mathf.Cos(ang), Mathf.Sin(ang)) * radius);
			touchAngle= (ang * Mathf.Rad2Deg);
			touchIndex= (int)Mathf.Floor(aproxIndex);
		}
	}

	public void updatePoints()
	{
		float deltaAngle = (2f * Mathf.PI) / vertexCount;
		float angle= 0f;

		for(int i= 0; i < objectList.Count; i++)
		{
			Vector3 pos= new Vector3(radius * Mathf.Cos(angle), radius * Mathf.Sin(angle));
			objectList[i].transform.position= pos;
			angle+= deltaAngle;
		}
	}

	public void addItem(Vector3 touchPos)
	{
		GameObject circle= Instantiate(obj, new Vector3(0, 0, 0), transform.rotation); 
		circle.GetComponent<SpriteRenderer>().color= new Color( Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
		circle.transform.SetParent(transform);
		objectList.Insert((touchIndex < 0 ? objectList.Count + touchIndex : touchIndex), circle);
		//transform.rotation= Quaternion.Euler(new Vector3(0, 0, touchAngle));

		vertexCount++;

		updatePoints();
	}

	public void OnDestroy()
	{
		EventSystem.current.onTouchTrigger+= addItem;
	}

	#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		float deltaAngle = (2f * Mathf.PI) / vertexCount;
		float angle= 0f;

		Vector3 oldPos= new Vector3(0, 0, 0);

		for(int i= 0; i < vertexCount + 1; i++)
		{
			Vector3 pos= new Vector3(radius * Mathf.Cos(angle), radius * Mathf.Sin(angle));
			Gizmos.DrawLine(oldPos, transform.position + pos);
			oldPos= transform.position + pos;
			angle+= deltaAngle;
		}
	}
	#endif
}
