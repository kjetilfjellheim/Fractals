using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddPoints : MonoBehaviour {

	public GameObject[] startPoints;

	GameObject prevPoint;

	void Start () {
		SetInitalStartPoint ();
		InvokeRepeating("AddNewPoint", 0f, 0.001f);
	}

	void AddNewPoint() {
		GameObject moveToward = startPoints [Random.Range (0, startPoints.Length)];
		Vector3 moveTowardPosition = moveToward.transform.position;
		Vector3 prevPointPosition = prevPoint.transform.position;
		float newXPointPosition = (moveTowardPosition.x + prevPointPosition.x) / 2;
		float newYPointPosition = (moveTowardPosition.y + prevPointPosition.y) / 2;

		GameObject newPoint = Instantiate (prevPoint);
		Vector3 newPointPosition = newPoint.transform.position;
		newPointPosition.x = newXPointPosition;
		newPointPosition.y = newYPointPosition;
		newPoint.transform.position = newPointPosition;
		prevPoint = newPoint;
	}

	void SetInitalStartPoint() {
		prevPoint = Instantiate (startPoints [Random.Range (0, startPoints.Length)]);
	}
}
