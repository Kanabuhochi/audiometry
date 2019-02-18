using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowGraph : MonoBehaviour {

	//public GameObject resultsT;
	private RectTransform graphContainer;
	[SerializeField] private Sprite circleSprite;
	[SerializeField] private Sprite crossSprite;
	private float[] resultsTable;

	void Awake(){
		graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();
		
		resultsTable = GameObject.Find("Results").GetComponent<resultsScript>().results;;
		int posX = -109;
		int posY = 152;
		float j;
		for(int i = 0; i < 7; i++){
			j =	resultsTable[i]/5;
			CreateCircle(new Vector2(posX,posY-(14*j)));
			posX+=53;
		}

	}

	void CreateCircle(Vector2 anchoredPosition){
		GameObject gameobject = new GameObject("circle",typeof(Image));
		gameobject.transform.SetParent(graphContainer,false);
		gameobject.GetComponent<Image>().sprite = circleSprite;
		RectTransform rectTransform = gameobject.GetComponent<RectTransform>();
		rectTransform.anchoredPosition = anchoredPosition;
		rectTransform.sizeDelta = new Vector2(11,11);
		rectTransform.anchorMin = new Vector2(0,0);
		rectTransform.anchorMax = new Vector2(0,0);
	}


}
