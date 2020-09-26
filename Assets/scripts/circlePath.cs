using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LineRenderer))]
public class circlePath : MonoBehaviour
{
	public int vertexCount= 6;
	public float lineWidth= 0.2f;
	public float radius= 1f;
	public GameObject obj;
	public GameObject theLine;
	public Transform panel;

	private LineRenderer lineRenderer;
	private List<GameObject> objectList= new List<GameObject>();
	private Vector3 center;

	private float touchAngle= 0f;
	private float actualAngle= 0f;
	private int touchIndex= 0;
	private float currentIndex= 0;

	private float touchRate= 0.25f;
	private float canTouch= -1f;
	private Vector3 touchPos;

	void Awake()
	{
		lineRenderer= GetComponent<LineRenderer>();
	}

	void Start()
	{
		center= transform.position;

		for(int i= 0; i < vertexCount; i ++)
		{
			GameObject circle= Instantiate(obj, new Vector3(0, 0, 0), transform.rotation); 
			circle.transform.SetParent(transform);
			circle.transform.SetParent(panel);
			circle.GetComponent<RectTransform>().localScale= new Vector3(1f, 1f, 1f);
			circle.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text= i + "";
			circle.GetComponent<Image>().color= new Color( Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
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

		if(touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Began)
		{
			lineRenderer.enabled= true;
			touchAngle= Mathf.Atan2(touchPos.y, touchPos.x);
			actualAngle= (touchAngle >= 0 ? touchAngle : ((Mathf.PI * 2) + touchAngle)) * Mathf.Rad2Deg;

			float aproxIndex= touchAngle / delta;
			Debug.DrawLine (transform.position, new Vector3(Mathf.Cos(touchAngle), Mathf.Sin(touchAngle)) * radius);

			touchAngle= (Mathf.Floor(aproxIndex) * delta) + (delta / 2);
			lineRenderer.SetPosition(1, new Vector3(Mathf.Cos(touchAngle), Mathf.Sin(touchAngle)) * radius);
			
			for(int i= 0; i <= vertexCount; i++)
			{
				currentIndex= i * delta * Mathf.Rad2Deg;
				if(actualAngle <= (i * delta * Mathf.Rad2Deg))
				{
					touchIndex= i;
					break;
				}
			}
		}

		if(Input.touchCount > 0 && Time.time > canTouch)
		{	
			if(touch.phase == TouchPhase.Ended)
			{
				addItem();
				canTouch= Time.time + touchRate;
			}
		}
	}

	public void updatePoints(int exclusion= 0)
	{
		float deltaAngle = (2f * Mathf.PI) / vertexCount;
		float angle= 0f;

		for(int i= 0; i < objectList.Count; i++)
		{
			GameObject itm= objectList[i];
			Vector3 pos= new Vector3(radius * Mathf.Cos(angle), radius * Mathf.Sin(angle));
			if(i == exclusion)
			{
				itm.GetComponent<BitObject>().lerpToPosition(deltaAngle * i, 0);
			}
			else
			{
				itm.GetComponent<BitObject>().lerpToPosition(deltaAngle * i);
			}
			angle+= deltaAngle;
		}
	}

	public void addItem()
	{
		lineRenderer.enabled= false;

		GameObject bit= Instantiate(obj, new Vector3(0, 0, 0), transform.rotation); 
		bit.GetComponent<Image>().color= new Color( Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
		bit.transform.SetParent(transform);
		bit.transform.SetParent(panel);
		bit.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text= objectList.Count + "";
		bit.GetComponent<RectTransform>().localScale= new Vector3(1f, 1f, 1f);

		if(touchIndex >= objectList.Count)
			objectList.Add(bit);
		else
			objectList.Insert(touchIndex, bit);

		vertexCount++;
		updatePoints(touchIndex);

		float deltaAngle = (2f * Mathf.PI) / vertexCount;
		Vector3 pos= new Vector3(radius * Mathf.Cos(deltaAngle * touchIndex), radius * Mathf.Sin(deltaAngle * touchIndex));
		bit.transform.position= pos;
	}

	public void OnDestroy()
	{

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
