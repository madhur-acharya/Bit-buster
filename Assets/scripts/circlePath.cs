using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class circlePath : MonoBehaviour
{
	public int vertexCount= 6;
	public float lineWidth= 0.2f;
	public float radius= 1f;
	public bool circleFillScreen= false;
	public GameObject obj;

	private LineRenderer lineRenderer;
	private List<GameObject> objectList= new List<GameObject>();

	void Awake()
	{
		lineRenderer= GetComponent<LineRenderer>();
	}

	void Start()
	{
		EventSystem.current.onTouchTrigger+= addItem;

		for(int i= 0; i <= vertexCount; i ++)
		{
			objectList.Add(Instantiate(obj, new Vector3(0, 0, 0), transform.rotation));
		}
	}

	void Update()
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
		float delta = (2f * Mathf.PI) / vertexCount;

		float ang= Mathf.Atan2(touchPos.y, touchPos.x);
		Debug.Log(ang * Mathf.Rad2Deg);



		//objectList.Insert(index, Instantiate(obj, new Vector3(0, 0, 0), transform.rotation));
		objectList.Add(Instantiate(obj, new Vector3(0, 0, 0), transform.rotation));
		vertexCount++;
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
