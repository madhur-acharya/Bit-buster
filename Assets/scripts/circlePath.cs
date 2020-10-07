using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LineRenderer))]
public class circlePath : MonoBehaviour
{
	private int vertexCount= 0;
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
	private int maxBits= 13;
	private GameObject centerObject;

	private float touchRate= 0.4f;
	private float canTouch= -1f;
	private Vector3 touchPos;
	private BitObject chainingBit;

	void Awake()
	{
		lineRenderer= GetComponent<LineRenderer>();
	}

	void Start()
	{
		center= transform.position;

		long numb= RNJesus.gerRandomPowO2();
		centerObject= Instantiate(obj, center, transform.rotation); 
		centerObject.GetComponent<Image>().color= new Color( Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
		centerObject.transform.SetParent(transform);
		centerObject.transform.SetParent(panel);
		centerObject.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text= numb + "";

		BitObject bitObject= centerObject.GetComponent<BitObject>();
		bitObject.objectType= "bit";
		bitObject.value= numb;

		centerObject.GetComponent<RectTransform>().localScale= new Vector3(1f, 1f, 1f);
		centerObject.GetComponent<BitObject>().enabled= false;

		for(int i= 0; i < 2; i ++)
		{
			addItem(true, "bit");
		}

		updatePoints();
	}

	void Update()
	{
		mouseInput();
		/*if(Input.touchCount <= 0) return;

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
		}*/
	}

	void mouseInput()
	{
		if(vertexCount == 0) return;

		Vector3 touchPos= Camera.main.ScreenToWorldPoint(Input.mousePosition);
		touchPos.z= 0;

		float delta = (2f * Mathf.PI) / (vertexCount);

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

		if(Time.time > canTouch && Input.GetMouseButtonDown(0))
		{
			addItem();
			canTouch= Time.time + touchRate;
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
				itm.GetComponent<BitObject>().lerpToPosition(pos, 0);
			}
			else
			{
				itm.GetComponent<BitObject>().lerpToPosition(deltaAngle * i);
			}
			angle+= deltaAngle;
		}
	}

	public void addItem(bool addDIrectly= false, string type= "random")
	{
		//if(objectList.Count >= maxBits) return;

		GameObject bit;

		if(objectList.Count >= maxBits)
		{
			bit= createBitObject("combine");
		}
		else
		{
			bit= createBitObject(type);
		}

		if(!addDIrectly)
		{
			centerObject.GetComponent<BitObject>().enabled= true;
			if(touchIndex >= objectList.Count)
				objectList.Add(centerObject);
			else
				objectList.Insert(touchIndex, centerObject);

			bit.GetComponent<BitObject>().enabled= false;
			bit.SetActive(false);
			StartCoroutine(addItemAsync(bit));
		}
		else
		{
			if(touchIndex >= objectList.Count)
				objectList.Add(bit);
			else
				objectList.Insert(touchIndex, bit);
		}

		vertexCount++;
		updatePoints(touchIndex);
	}

	private GameObject createBitObject(string explicitType= "random")
	{
		List<float> space= new List<float>();
		space.Add(0.8f);
		space.Add(0.2f);
		int type= RNJesus.biasedRandom(space);
		long numb= RNJesus.gerRandomPowO2();

		GameObject bit= Instantiate(obj, new Vector3(0, 0, 0), transform.rotation); 
		bit.GetComponent<Image>().color= new Color( Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
		bit.transform.SetParent(transform);
		bit.transform.SetParent(panel);
		bit.GetComponent<RectTransform>().localScale= new Vector3(1f, 1f, 1f);
		BitObject bitObject= bit.GetComponent<BitObject>();

		switch(explicitType)
		{
			case "random" : {
				if(type == 0)
				{
					bit.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text= numb + "";
					bitObject.objectType= "bit";
					bitObject.value= numb;
				}
				else
				{
					bitObject.objectType= "combine";
					bit.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text= "+";
				}
				break;
			};
			case "bit" : {
				bit.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text= numb + "";
				bitObject.objectType= "bit";
				bitObject.value= numb;
				break;
			};
			case "combine" : {
				bitObject.objectType= "combine";
				bit.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text= "+";
				break;
			};
			default : {
				bitObject.objectType= "bit";
				bit.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text= "+";
				bitObject.value= numb;
				break;
			};
		}

		return bit;
	}

	private bool checkForCombinations(bool chain= false)
	{
		for(int i= 0; i < objectList.Count; i++)
		{
			GameObject itm= objectList[i];

			if(itm.GetComponent<BitObject>().objectType != "bit")
			{
				int prev= (i - 1 > -1) ? i-1 : objectList.Count - 1;
				int next= (i + 1 < objectList.Count) ? i+1 : 0;
				int mid= i;

				Debug.Log(prev + ", " + i + ", " + next + " => " +  objectList[prev].GetComponent<BitObject>().value + ", " +  objectList[next].GetComponent<BitObject>().value);

				if(objectList[prev].GetComponent<BitObject>().value == objectList[next].GetComponent<BitObject>().value)
				{
					GameObject ref1= objectList[prev];
					GameObject ref2= objectList[next];
					GameObject ref3= objectList[mid];

					long val1= ref1.GetComponent<BitObject>().value;
					long val2= ref2.GetComponent<BitObject>().value;

					BitObject scriptRef= ref3.GetComponent<BitObject>();
					scriptRef.objectType= "combine";

					if(chain)
					{
						scriptRef.value= val1 * val2;
						ref3.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text= scriptRef.value + "";
						chainingBit= scriptRef;
					}
					else
					{
						scriptRef.value= val1 + val2;
						ref3.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text= scriptRef.value + "";
						chainingBit= null;
					}

					if(next > prev)
					{
						objectList.RemoveAt(next);
						objectList.RemoveAt(prev);
					}
					else
					{
						objectList.RemoveAt(prev);
						objectList.RemoveAt(next);
					}

					float angle= ((2f * Mathf.PI) / vertexCount) * mid;
					ref1.GetComponent<BitObject>().lerpToPosition(angle);
					ref2.GetComponent<BitObject>().lerpToPosition(angle);
					Destroy(ref1, 0.2f);
					Destroy(ref2, 0.2f);

					vertexCount-= 2;
					return true;
				}
			}
		}
		return false;
	}

	private IEnumerator addItemAsync(GameObject item, float delay= 0.2f)
	{
		bool chain= false;

		for(int i= 0; i < maxBits; i++)
		{
			yield return new WaitForSeconds(delay);
			
			if(!checkForCombinations(chain))
			{
				break;
			}
			else
			{
				chain= true;
			}
		}

		centerObject= item;
		centerObject.SetActive(true);
		if(chainingBit != null)
		{
			chainingBit.objectType= "bit";
			chainingBit= null;
		}
		
		yield return new WaitForSeconds(delay);
		updatePoints();
		Debug.Log("Chain complete");
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
