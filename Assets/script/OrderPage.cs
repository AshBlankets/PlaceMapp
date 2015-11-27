using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/*
*Welcome to the Order Page. This handles displaying everything on the rhs order page.
*createOrders() is called by MenuController when the order page is swiped to and destroyOrders() is called on swiping away
*OrderElements created will be made children of whatever this script is on.
*
*(whatever this script is on should be tagged w OrderPage)
*/

public class OrderPage : MonoBehaviour {
	public Order order;

	public int y; //y pos of the first entry
	public int itemHt; //height of the item entry slot. Bigger = spaced further apart

	public Text orderPrefab; //the OrderElement prefab
	public RectTransform contentPane; //transform for the panel containing orders (part of scrollview)

	private Dictionary<Item, int> orders;
	private Text orderEntry; 
	private Text priceField;
	private float totalPrice;
	private int maxHeight = 1400;

	// Use this for initialization
	void Start () {
		orders = order.getOrder ();
	}

	//look at the current order list and make a Text for each
	public void createOrders(){
		int iCount = 0;
		foreach(Item i in orders.Keys){

			//making the order
			orderEntry = (Text)Instantiate(orderPrefab);
			orderEntry.name = i.getName(); //name the object
			int qty = orders[i];
			string temp = "x"+qty;
			temp+= " "+i.getName();
			orderEntry.text = temp; //assign the text

			priceField = orderEntry.transform.Find("Price").gameObject.GetComponent<Text>(); //find the Price-text child 
			priceField.text = (qty*i.getPrice()).ToString("C");

			//positioning the order
			orderEntry.transform.SetParent(this.gameObject.transform); //make position relative to the orderview
			orderEntry.transform.localScale = Vector2.one;//this line stops the text from being huge
			orderEntry.GetComponent<RectTransform>().localPosition = new Vector2(50, y-(iCount*itemHt));
			iCount++;
		}

		if ((y - itemHt*iCount) < -maxHeight)
			contentPane.sizeDelta = new Vector2 (980, (-y + itemHt*iCount));
		else
			contentPane.sizeDelta = new Vector2 (980, maxHeight);
		updatePrice ();
	}

	public void updatePrice (){
		GameObject.FindGameObjectWithTag ("TotalPrice").GetComponent<Text>().text= order.getTotalPrice ().ToString("C");
	}

	public void destroyOrders(){
			GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("OrderElem");
			foreach (GameObject target in gameObjects) {
			GameObject.Destroy(target);
		}
	}

	//this gets called by the AlterOrder script. Action 0-remove 1-add
	public void orderQuantity(string name, int action){
		if(action==0){
		decreaseOrder (name);

		destroyOrders ();
		createOrders ();
		}
		else{
		increaseOrder (name);
			
		destroyOrders ();
		createOrders ();
		}
	}

	// Update is called once per frame
	void Update () {

	}

	//Removes one of a given item from the order page. If quantity becomes 0 remove completely.
	public void decreaseOrder(string name){
		Item found = null;
		foreach (Item i in orders.Keys) {
			if(i.getName().Equals(name)){
				found = i;
			}
		}
		
		if (found == null) {
			Debug.Log ("Tried to remove non-existent item - you shouldn't be able to get here.");
		} 
		else {
			orders[found] --;
			if(orders[found] == 0){
				orders.Remove(found);
			}
		}
		//displayOrder ();
	}
	
	//Adds one of a given item to the order page.
	public void increaseOrder(string name){
		Item found = null;
		foreach (Item i in orders.Keys) {
			if(i.getName().Equals(name)){
				found = i;
			}
		}
		
		if (found == null) {
			Debug.Log ("Tried to increase quantity of non-existent item - you shouldn't be able to get here.");
		} else {
			orders[found] ++;
		}
		//displayOrder ();
	}
}
