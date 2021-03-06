using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateFractal : MonoBehaviour {

	private const int MIN_VERTICES = 3;
	private const int MAX_VERTICES = 20;
	private const int TEXTURE_HEIGHT = 1600;
	private const int TEXTURE_WIDTH = 1600;

	public Image fractalImage;
	public InputField numPoints;
	public InputField deltaVInputField;
	public Dropdown numVerticesDropdown;
	public Button createFractalButton;

	private CanvasRenderer canvasRenderer;

	void Start () {
		SetVerticesDropdownOptions (numVerticesDropdown);
		canvasRenderer = GetComponent<CanvasRenderer> ();
		createFractalButton.onClick.AddListener (CreateFractalAction);
	}

	public void SetVerticesDropdownOptions(Dropdown numVerticesDropdown) {
		List<Dropdown.OptionData> options = new List<Dropdown.OptionData> ();
		for (int vertices = MIN_VERTICES; vertices <= MAX_VERTICES; vertices++) {
			Dropdown.OptionData optionData = new Dropdown.OptionData ();
			optionData.text = vertices.ToString();
			options.Add (optionData);
		}
		numVerticesDropdown.AddOptions(options);
	}

	public void CreateFractalAction() {
		Texture2D newFractalTexture = new Texture2D (TEXTURE_WIDTH, TEXTURE_HEIGHT);	
		SetWhiteBackground (newFractalTexture);
		Point[] startPoints = GetStartPoints (newFractalTexture);
		DrawStartPoints (startPoints, newFractalTexture);
		float distanceMultiplier = float.Parse(deltaVInputField.GetComponentInChildren<Text>().text);
		DrawFractal (startPoints, newFractalTexture, distanceMultiplier);
		ShowFractal (newFractalTexture, distanceMultiplier);
	}

	public void DrawFractal(Point[] startPoints, Texture2D fractalTexture, float distanceMultiplier) {
		long points = long.Parse (numPoints.text);
		Point startPoint = GetArbitraryPoint (startPoints);
		int currentX = startPoint.x;
		int currentY = startPoint.y;
		for (long p = 0;p < points; p++) {
			Point moveTowardPoint = GetArbitraryPoint (startPoints);
			float distX = (moveTowardPoint.x + currentX) * distanceMultiplier;
			float distY = (moveTowardPoint.y + currentY) * distanceMultiplier;
			currentX = (int)((distX * distanceMultiplier) * 0.5f);
			currentY = (int)((distY * distanceMultiplier) * 0.5f);
			fractalTexture.SetPixel (currentX, currentY, Color.black);
		}
	}

	public Point GetArbitraryPoint(Point[] points) {
		return points[Random.Range (0, points.Length)];
	}

	public void DrawStartPoints(Point[] startPoints, Texture2D fractalTexture) {
		foreach (Point startPoint in startPoints) {
			fractalTexture.SetPixel (startPoint.x, startPoint.y, Color.blue);
		}
	}

	public void SetWhiteBackground(Texture2D fractalTexture) {
		for (int x = 0;x<fractalTexture.width;x++) {
			for (int y = 0;y<fractalTexture.height;y++) {
				fractalTexture.SetPixel (x, y, Color.white);
			}
		}		
	}

	public void ShowFractal(Texture2D fractalTexture, float distanceMultiplier) {	
		int newWidth = (int)(((float)fractalTexture.width) * distanceMultiplier);
		int newHeight = (int)(((float)fractalTexture.height) * distanceMultiplier);
		Color[] copyColor = fractalTexture.GetPixels (0, 0, newWidth, newHeight);
		Texture2D newTexture = new Texture2D (newWidth, newHeight);
		fractalTexture.Apply ();
		newTexture.SetPixels (copyColor);
		canvasRenderer.SetTexture (fractalTexture);
		newTexture.Apply ();		
	}

	public Point[] GetStartPoints(Texture2D fractalTexture) {
		int numVertices = int.Parse(numVerticesDropdown.GetComponentInChildren<Text>().text);
		Point[] vertices = new Point[numVertices];
		float radius = (TEXTURE_HEIGHT / 2) - 50;
		for (int verticeIndex = 1; verticeIndex <= numVertices; verticeIndex++) {
			int x = (int)(radius * Mathf.Cos(((2f * Mathf.PI * (float)verticeIndex) / ((float)numVertices)) + (Mathf.PI / 6f)) + (TEXTURE_WIDTH / 2) );
			int y = (int)(radius * Mathf.Sin(((2f * Mathf.PI * (float)verticeIndex) / ((float)numVertices)) + (Mathf.PI / 6f)) +  (TEXTURE_HEIGHT / 2) );
			vertices [verticeIndex - 1].x = x; 
			vertices [verticeIndex - 1].y = y; 
		}
		return vertices;
	}
		
	public struct Point {
		public int x, y;
	}


}
