using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {
	public Camera viewport;
	public LineRenderer lineKeeperPrefab;
	public Material changingMaterial;
	public Material changingMaterial2;

	List<LineRenderer> symbols = new List<LineRenderer> ();
	LineRenderer currentLine;

	Collider drawingTarget;

	bool drawing = false;

	// Use this for initialization
	void Start () {
		//lr = GetComponent<LineRenderer> ();
	}



	// Update is called once per frame
	void Update () {

		Vector3 pointer = Input.mousePosition;

		bool leftMouseButton = Input.GetMouseButton (0);
		bool rKey = Input.GetKeyDown (KeyCode.R);

		if (rKey) {
			
			//Vector3 centre = new Vector3 (viewport.pixelWidth * 0.5f, viewport.pixelHeight * 0.5f);
			Ray rae = viewport.ScreenPointToRay (pointer);
			RaycastHit rayHit = new RaycastHit ();
			Physics.Raycast (rae, out rayHit);
			Debug.DrawLine (rae.origin, rae.origin + rae.direction * 5.0f, Color.green, 100.0f);
				//rae.direction, Color.green, 100.0f);
		}

		if (Input.GetMouseButtonDown (0) && !drawing) {
			drawing = true;

			Ray rae = viewport.ScreenPointToRay (pointer);
			RaycastHit rayHit = new RaycastHit ();
			Physics.Raycast (rae, out rayHit);

			drawingTarget = rayHit.collider;

			currentLine = Instantiate<LineRenderer> (lineKeeperPrefab);//, rayHit.point, Quaternion.identity);
		

			currentLine.transform.rotation = Quaternion.LookRotation(-rayHit.normal);
			currentLine.transform.Translate (-(rayHit.normal.normalized)*0.1f);
			currentLine.positionCount = 1;
			currentLine.SetPosition (0, rayHit.point);
		}

		else if (!(Input.GetMouseButton (0)) && drawing) {
			drawing = false;


			bool straight = checkStraightness (currentLine);
			Debug.Log ("straight: " + straight.ToString());
			bool vertical = checkVerticalness (currentLine);
			Debug.Log ("verticalness: " + vertical.ToString());
			bool circular = checkCircleness (currentLine);
			Debug.Log ("circleness: " + circular.ToString ());

			if (straight) {
				//previously && vertical
				drawingTarget.GetComponent<Renderer> ().material = changingMaterial;
			}

			if (circular) {
				drawingTarget.GetComponent<Renderer> ().material = changingMaterial2;
			}

			symbols.Add (currentLine);


		}

		else if (leftMouseButton) {
			//TODO: Need to make it only collect points every x distance, otherwise mean gets weird.
			//Debug.Log ("R");
			//Vector3 centre = new Vector3 (viewport.pixelWidth * 0.5f, viewport.pixelHeight * 0.5f);
			Ray rae = viewport.ScreenPointToRay (pointer);
			RaycastHit rayHit = new RaycastHit ();
			Physics.Raycast (rae, out rayHit);


			Vector3 lastPoint = currentLine.GetPosition (currentLine.positionCount - 1);
			//float dist = Vector3.Distance (rayHit.point, lastPoint);

			if (Vector3.Distance (rayHit.point, lastPoint) > 0.1f) {
				Debug.Log ("A");
				int i = 0;
				while (Vector3.Distance (rayHit.point, lastPoint) > 0.1f) {
					//Debug.Log ("pc: " + (currentLine.positionCount - 1).ToString());
					if (currentLine.positionCount % 2 == 0) {
						Debug.DrawLine (lastPoint, rayHit.point, Color.blue, 100.0f, false);
					} else {
						Debug.DrawLine (lastPoint, rayHit.point, Color.cyan, 100.0f, false);
					}


					lastPoint = lastPoint + Vector3.Normalize(rayHit.point - lastPoint) * 0.1f;

					currentLine.positionCount += 1;
					currentLine.SetPosition (currentLine.positionCount - 1, lastPoint);


					if (i > 0)
						Debug.Log ("whiling: " + i.ToString ());
					i++;
					
				}
			}
		}
	}

	float distancePointToLine3D(Vector3 point, Vector3 lineStart, Vector3 lineEnd){
		return ((Vector3.Cross (lineEnd - lineStart, lineStart - point).magnitude) / (lineEnd - lineStart).magnitude);
	}

	bool checkCircleness(LineRenderer lineRendererInQuestion) {
		Vector3[] lineInQuestion = new Vector3[10000];
		lineRendererInQuestion.GetPositions (lineInQuestion);

		Vector3 first = lineInQuestion [0];
		Vector3 last = lineInQuestion [Mathf.Max(0,lineRendererInQuestion.positionCount-2)];

		Vector3 dif = last - first;

		if (Mathf.Abs (dif.x) < 0.8f && Mathf.Abs (dif.y) < 0.8f && Mathf.Abs (dif.z) < 0.8f) {
			Vector3 meanPoint = Vector3.zero;
			for (int i = 0; i < Mathf.Max (0, lineRendererInQuestion.positionCount - 2); i++) {
				meanPoint += lineInQuestion [i];
			}
			meanPoint /= lineRendererInQuestion.positionCount - 1;

			float maxDist = float.NegativeInfinity;
			float minDist = float.PositiveInfinity;
			Vector3 furthest = Vector3.zero;
			Vector3 closest = Vector3.zero;
			float meanDist = 0.0f;
			for (int i = 0; i < Mathf.Max (0, lineRendererInQuestion.positionCount - 2); i++) {
				float dist = Mathf.Abs (Vector3.Distance (meanPoint, lineInQuestion [i])); 
				meanDist += dist;
				maxDist = Mathf.Max (maxDist, dist);
				minDist = Mathf.Min (minDist, dist);

				if (maxDist == dist)
					furthest = lineInQuestion [i];

				if (minDist == dist)
					closest = lineInQuestion [i];

				Debug.DrawLine (meanPoint, lineInQuestion [i], Color.black, 100.0f, false);
			}
			meanDist /= lineRendererInQuestion.positionCount - 1;

			Debug.Log ("meanDist: " + meanDist.ToString ());
			Debug.Log ("maxDist: "  + maxDist.ToString ());
			Debug.Log ("minDist: "  + minDist.ToString ());

			Debug.Log ("meanPoint: " + meanPoint.ToString ());
			Debug.Log ("furthest: "  + furthest.ToString ());
			Debug.Log ("closest: "   + closest.ToString ());

			Debug.DrawLine (meanPoint, furthest, Color.red,100.0f,false);
			Debug.DrawLine (meanPoint, closest, Color.green,100.0f,false);

			Debug.DrawLine (meanPoint + new Vector3 (0.0f, meanDist * 0.8f, 0.0f), meanPoint + new Vector3 (0.0f, meanDist * 1.2f, 0.0f), Color.blue, 100.0f);
			if (meanDist * 1.2f > maxDist && meanDist * 0.8f < minDist) {
				return true;
			} else {
				return false;
			}

		
			/*
			float maxDist = float.NegativeInfinity;
			bool circular = true;
			float c = Vector3.Distance (mean, first);
			for (int i = 1; i < Mathf.Max (0, lineRendererInQuestion.positionCount - 2); i++) {
				float dist = Mathf.Abs (Vector3.Distance (meanPoint, lineInQuestion [i]));
				if (dist < 0.5f) {
					continue;
				} else {
					circular = false;
					maxDist = Mathf.Max (maxDist, dist);
				}
			}
			Debug.Log ("maxDist = " + maxDist.ToString ());
			//Debug.Log ("Vertical Line!");
			return circular;
			*/
		} else {
			Debug.Log ("failed first test");
			//Debug.Log ("Non-vertical Line!");
			return false;
		}

	}

	bool checkStraightness(LineRenderer lineRendererInQuestion) {
		Vector3[] lineInQuestion = new Vector3[1000];
		lineRendererInQuestion.GetPositions (lineInQuestion);

		Vector3 first = lineInQuestion [0];
		Vector3 last = lineInQuestion [Mathf.Max(0,lineRendererInQuestion.positionCount-2)];

		bool acceptable = true;
	
		for (int i = 1; i < lineRendererInQuestion.positionCount - 2; i++) {
			if (distancePointToLine3D (lineInQuestion [i], first, last) < 1.0f) {
				continue;
			} else {
				acceptable = false;
			}
		}

		lineRendererInQuestion.GetComponent<Transform> ().SetParent (drawingTarget.GetComponent<Transform>());

		return acceptable;
	}

	bool checkVerticalness(LineRenderer lineRendererInQuestion) {
		Debug.Log ("Checking");
		Vector3[] lineInQuestion = new Vector3[1000];
		lineRendererInQuestion.GetPositions (lineInQuestion);

		Vector3 first = lineInQuestion [0];
		Vector3 last = lineInQuestion [lineRendererInQuestion.positionCount-2];

		Vector3 dif = last - first;

		Debug.Log (first);
		Debug.Log (last);
		Debug.Log (dif);

		if (Mathf.Abs (dif.x) < 0.2f && Mathf.Abs (dif.y) > 0.2f && Mathf.Abs (dif.z) < 0.2f) {
			Debug.Log ("Vertical Line!");
			return true;
		} else {
			Debug.Log ("Non-vertical Line!");
			return false;
		}
	}
}
