using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;

public class Database : MonoBehaviour {

	public TextAsset menuDataText; //the XML file with all the menu info
	public static List<Item> menuDatabase = new List<Item>(); //Public list of Items
	
	private Dictionary<string, string> menuDict; //name-entry for each item
	private List<Dictionary<string, string>> menuItemInfo = new List<Dictionary<string,string>>(); //name-entry for every item

	void Awake(){
		loadItems ();

		//populate the list with Item objects
		for (int i =0; i< menuItemInfo.Count; i++) {
			menuDatabase.Add(new Item(menuItemInfo[i]));
		}

		//printInfo (); //just to test loading. Comment out later
	}

	//prints the details for all of the items
	public void printInfo(){
		foreach(Item i in menuDatabase){
		Debug.Log ("NAME : "+i.getName()+" TYPE: "+i.getType()+" DESCRIPTION: "+i.getDescription()+" PRICE: "+i.getPrice());
	}
	}

	public void loadItems(){
		XmlDocument menuData = new XmlDocument();
		menuData.LoadXml (menuDataText.text);

		//grab the things tagged <Item> in the XML file
		XmlNodeList itemList = menuData.GetElementsByTagName ("Item");

		//for each item in the XML file grab all the child nodes
		foreach (XmlNode itemInfo in itemList) {
			XmlNodeList itemContent = itemInfo.ChildNodes;
			menuDict = new Dictionary<string, string>();

			//create a dictionary entry with the name and entry e.g. name-pizza price-8.0
			foreach(XmlNode content in itemContent){
				menuDict.Add(content.Name, content.InnerText);
			}

			menuItemInfo.Add(menuDict);
		}
	}

	public Item getItem(string name){
		foreach (Item i in menuDatabase) {
			if(i.getName().Equals(name)){
				return i;
			}
		}
		return null;
	}
}
