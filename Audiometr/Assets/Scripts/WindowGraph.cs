using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;
using UnityEngine.SceneManagement;

public class WindowGraph : MonoBehaviour {

    private RectTransform graphContainer;
	[SerializeField] private Sprite circleSprite;
	[SerializeField] private Sprite crossSprite;
	public float[] resultsTable;
	public Text analysis;
	

    private void Awake() {
        graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();
        resultsTable = GameObject.Find("Results").GetComponent<resultsScript>().results;
		resultAnalysis();
        ShowGraph(resultsTable);
		Destroy(GameObject.Find("Results"));
    }

    private GameObject CreateCircle(Vector2 anchoredPosition) {
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().sprite = circleSprite;
		gameObject.transform.localScale += new Vector3(2.5f, 2.5f, 0);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(11, 11);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        return gameObject;
    }

	private GameObject CreateCross(Vector2 anchoredPosition) {
        GameObject gameObject = new GameObject("cross", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().sprite = crossSprite;
		gameObject.transform.localScale += new Vector3(2.5f, 2.5f, 0);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(11, 11);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        return gameObject;
    }

    private void ShowGraph(float[] resultsTable) {
        float graphHeight = graphContainer.sizeDelta.y;
        float yMaximum = 100f;
        float xSize = 50f;
        float j;
		float xPosition = -162;
        float yPosition = 369-(14*resultsTable[0]/5);
        GameObject lastCircleGameObject = null;
        for (int i = 0; i < 7; i++) {
            j =	resultsTable[i]/5;
            GameObject circleGameObject = CreateCircle(new Vector2(xPosition, yPosition-(19*j)));
			xPosition+=70;
            if (lastCircleGameObject != null) {
                CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition,'r');
            }
            lastCircleGameObject = circleGameObject;
        }
		xPosition = -162;
        yPosition = 369-(14*resultsTable[0]/5);
		GameObject lastCrossGameObject = null;
        for (int i = 7; i < 14; i++) {
            j =	resultsTable[i]/5;
            GameObject crossGameObject = CreateCross(new Vector2(xPosition, yPosition-(14*j)));
			xPosition+=70;
            if (lastCrossGameObject != null) {
                CreateDotConnection(lastCrossGameObject.GetComponent<RectTransform>().anchoredPosition, crossGameObject.GetComponent<RectTransform>().anchoredPosition,'b');
            }
            lastCrossGameObject = crossGameObject;
        }
    }

    private void CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB, char colour) {
        GameObject gameObject = new GameObject("dotConnection", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
		
		if (colour == 'b')
        	gameObject.GetComponent<Image>().color = new Color(0,0,255,1);
		else if ( colour == 'r')
			gameObject.GetComponent<Image>().color = new Color(255,0,0,1);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 dir = (dotPositionB - dotPositionA).normalized;
        float distance = Vector2.Distance(dotPositionA, dotPositionB);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(distance, 3f);
        rectTransform.anchoredPosition = dotPositionA + dir * distance * .5f;
        rectTransform.localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(dir));
    }

	private void resultAnalysis(){
			if(resultsTable[15]<=20)
				analysis.text="Słuch prawidłowy, dźwięki odbierane są bez problemów";
			else if (resultsTable[15]>20 && resultsTable[15]<=40)
				analysis.text="Niewielki stopień niedosłuchu, problemy ze słyszeniem szeptu, oraz cichych dźwięków";
			else if (resultsTable[15]>40 && resultsTable[15]<=60)
				analysis.text="Umiarkowany stopień niedosłuchu, problemy ze słyszalnością mowy, oraz dźwięków na  tle hałasu";
			else if (resultsTable[15]>60 && resultsTable[15]<=80)
				analysis.text="Niedosłuch znacznego stopnia, słyszenie tylko głośnych rozmów i dźwięków";
			else if (resultsTable[15]>80)
				analysis.text="Głuchota, odbieranie większości dźwięków jako wibracje";
	}

}
