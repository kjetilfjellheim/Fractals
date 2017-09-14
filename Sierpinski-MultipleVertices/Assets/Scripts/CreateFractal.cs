using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateFractal : MonoBehaviour {

	private const int MIN_VERTICES = 3;
	private const int MAX_VERTICES = 8;
	private const int TEXTURE_HEIGHT = 400;
	private const int TEXTURE_WIDTH = 400;

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
		ShowFractal (newFractalTexture);
	}

	public void DrawStartPoints(Point[] startPoints, Texture2D fractalTexture) {
		foreach (Point startPoint in startPoints) {
			fractalTexture.SetPixel (startPoint.x, startPoint.y, Color.black);
		}
	}

	public void SetWhiteBackground(Texture2D fractalTexture) {
		for (int x = 0;x<fractalTexture.width;x++) {
			for (int y = 0;y<fractalTexture.height;y++) {
				fractalTexture.SetPixel (x, y, Color.white);
			}
		}		
	}

	public void ShowFractal(Texture2D fractalTexture) {
		canvasRenderer.SetTexture (fractalTexture);
		fractalTexture.Apply ();		
	}

	public Point[] GetStartPoints(Texture2D fractalTexture) {
		int numVertices = int.Parse(numVerticesDropdown.GetComponentInChildren<Text>().text);
		int deltaV = int.Parse (deltaVInputField.text);
		Point[] vertices = new Point[numVertices];
		float radius = (TEXTURE_HEIGHT / 2) - deltaV;
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
