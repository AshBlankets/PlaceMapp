using UnityEngine;
using UnityEngine.UI;

using System.Collections;

public class BottomPanelText : MonoBehaviour {

	public Text titleText;
	public Text priceText;
	public Text description;
	public GameObject downArrow;

	public FoodHandler foodhandler;

	public bool expanded = false;

	// Use this for initialization
	void Start () {
	
	}

	public void setText(){
		titleText.text = foodhandler.getActiveName ();
		priceText.text = foodhandler.getPrice ();
		
		if (expanded) {
			description.text = foodhandler.getDescription ();
		}
	//	else {
		//	description.text = "";
		//}
	}

	//public void setExpanded(bool isExpanded){
	//	expanded = isExpanded;
	//	setText();
	//}

	// Update is called once per frame
	void Update () {

	}
}
