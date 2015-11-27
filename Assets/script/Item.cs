using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//public class Item : MonoBehaviour {
public class Item  {
	private string name;
	private string type;
	private string description;
	private float price;
	
	public Item(Dictionary<string,string> itemInfo){
		name = itemInfo["name"];
		type = itemInfo ["type"];
		description = itemInfo["description"];
		price = float.Parse (itemInfo["price"]);
	}

	public string getName(){
		return name;
	}

	public string getType(){
		return type;
	}

	public string getDescription(){
		return description;
	}

	public float getPrice(){
		return price;
	}

}
