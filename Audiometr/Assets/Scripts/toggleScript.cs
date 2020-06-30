using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class toggleScript : MonoBehaviour, IPointerDownHandler {

	[SerializeField]
	private bool _isOn = false;
	public bool isOn {
		get {
			return _isOn;
		}
	}

	[SerializeField]
	private RectTransform toggleIndicator;
	[SerializeField]
	private Image backgroundImage;

	[SerializeField]
	private Color onColor;
	[SerializeField]
	private Color offColor;

	private float offX;
	private float onX;

	[SerializeField]
	private float tweenTime = 0.25f;

	public delegate void ValueChanged(bool value);
	public event ValueChanged valueChanged;

	// Use this for initialization
	void Start () {
		offX = toggleIndicator.anchoredPosition.x;
		onX = backgroundImage.rectTransform.rect.width - toggleIndicator.rect.width;
	}


	private void OnEnable() {
		Toggle(isOn);
	}


	private void Toggle(bool value) {
		if (value != isOn) {
			_isOn = value;
			ToggleColor(isOn);
			MoveIndicator(isOn);

			if(valueChanged != null) {
				valueChanged(isOn);
			}
		}
	}


	private void ToggleColor(bool value) {
		if(value)
			backgroundImage.DOColor(onColor, tweenTime);
		else
			backgroundImage.DOColor(offColor, tweenTime);
	}

	private void MoveIndicator(bool value) {
		if(value)
			toggleIndicator.DOAnchorPosX(onX, tweenTime);
		else 
			toggleIndicator.DOAnchorPosX(offX, tweenTime);
	}

	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnPointerDown (PointerEventData eventData) {
		Toggle(!isOn);
	}
}
