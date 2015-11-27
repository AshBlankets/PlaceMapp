using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Order : MonoBehaviour {

	public static Dictionary<Item, int> itemsInOrder = new Dictionary<Item, int>();

	Database database;
	FoodHandler foodhandler;
	public Text textArea;

	public Text confirmationText;

	public int currentQuantity = 1;
	public string orderString = "";
	public float totalPrice = 0.0f;

	// Use this for initialization
	void Start () {
		
		database = GameObject.FindGameObjectWithTag ("ARCamera").GetComponent<Database> ();
		foodhandler = GameObject.FindGameObjectWithTag ("DirectionalLight").GetComponent<FoodHandler> ();
	}

	//So this comes from the main screen with an Object...
	public void addToOrder(){
		int numToAdd = currentQuantity; //TODO - link this to the quantity box, somehow. 
		GameObject toAdd = foodhandler.getActiveModel ();

		if (numToAdd > 0) { //Just in case.
			string name = toAdd.name;
			Item foodItem = null;

			//FIRST since we're adding, we'll see if this is already in the order and just need to increase quantity. 
			foreach (Item i in itemsInOrder.Keys) {
				if (name.Equals (i.getName())) {
					foodItem = i;
				}
			}

			if (foodItem != null) { //There's at least one in the order already. 

				itemsInOrder [foodItem] += numToAdd; //add extra quantity. 
				//Debug.Log("Added "+numToAdd+" more "+name+" - Total num: "+itemsInOrder[foodItem]); 
				showConfirmation(foodItem, numToAdd);
		
			} else { //We don't already have it in the order - get it from the Database. 

				foreach (Item i in Database.menuDatabase) {
					if (name.Equals (i.getName())) {
						foodItem = i;
					}
				}

				if (foodItem == null) {
					confirmationText.text = "This isn't a food.";
					//Debug.Log ("Failed to add - no item in database with name " + toAdd);
				} else {
					itemsInOrder.Add (foodItem, numToAdd);  
					showConfirmation(foodItem, numToAdd);
					//Debug.Log("Added "+numToAdd+" "+name+" to order.");
				}
			}
		}
		currentQuantity = 1;
		textArea.text = currentQuantity+"";
	}

	//Increases quantity shown in bottom text area
	public void increaseQuantity(){
		currentQuantity++; //no upwards limit? 
		textArea.text = currentQuantity+"";
	}

	//Decreases quantity shown in bottom text area
	public void decreaseQuantity(){
		if (currentQuantity > 1) { //can't go below one. 
			currentQuantity--;
			textArea.text = currentQuantity+"";
		}
	}

	public void showConfirmation(Item food, int quantity){
		string orderString = "<b>"+food.getName()+"</b>";
		orderString += "\n\nQuantity: " + quantity;
		orderString += "\nTotal Price: $" + (quantity * food.getPrice()).ToString("F2");
		confirmationText.text = orderString;
	}

	public float getTotalPrice(){
		totalPrice = 0.0f;

		foreach (Item i in itemsInOrder.Keys) {
			float price = (itemsInOrder[i] * i.getPrice());
			totalPrice+=price;
		}
		return totalPrice;
	}
	
	public Dictionary<Item, int> getOrder(){
		return itemsInOrder;
	}
	// Update is called once per frame
	void Update () {
	
	}
}
