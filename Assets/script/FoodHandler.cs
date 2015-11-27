using UnityEngine;
using System.Collections;

public class FoodHandler : MonoBehaviour {

	Database database;
	MenuController menucontroller;
	public BottomPanelText bpt;
	public BottomPanelText bpt2; //this is a shitty way to do this and I'm sorry. 

	//this is the order the appear on the menu

	GameObject[][] dishes; 

	GameObject activeFood;
	int activeFoodIndex = 0;
	int activeCategory = 4; // 0 Appetizer | 1 Main | 2 Drinks | 3 Desserts | 4 Specials

	// Use this for initialization
	void Start () {

		database = GameObject.FindGameObjectWithTag ("ARCamera").GetComponent<Database> ();
		menucontroller = GameObject.FindGameObjectWithTag ("GUIController").GetComponent<MenuController> ();
		//database.printInfo ();

		dishes = new GameObject[5][]; //Set number of categories, flex number of items ;o what sexy code

		dishes[0] = GameObject.FindGameObjectsWithTag("FoodAppetizer"); 
		dishes[1] = GameObject.FindGameObjectsWithTag("FoodMain"); 
		dishes[2] = GameObject.FindGameObjectsWithTag("FoodDrink"); 
		dishes[3] = GameObject.FindGameObjectsWithTag("FoodDessert"); 
		dishes[4] = new GameObject[]{dishes [1][0], dishes [1][1]};

		for (int i = 0; i<4; i++) {
			for(int j = 0; j<dishes[i].Length; j++){
				dishes[i][j].SetActive(false);
			}
		}

		setActive ();
	}

	public void swipeUp(){
		activeFood.SetActive (false);
		activeFoodIndex --; 
		if (activeFoodIndex == -1) {
			//activeFoodIndex = dishes [activeCategory].Length - 1;
			scrollCategory (activeCategory-1, true);
		} else {
			setActive ();
		}
	}

	public void swipeDown(){
		activeFood.SetActive (false);
		activeFoodIndex ++; 
		if (activeFoodIndex == dishes [activeCategory].Length) {
			//activeFoodIndex = 0;
			scrollCategory (activeCategory + 1, false);
		} else {
			setActive ();
		}
	}

	//Called from buttonpresses - linked by buttons in scene
	public void changeCategory(int cat){
		activeFood.SetActive (false);
		activeFoodIndex = 0; 
		activeCategory = cat;
	
		setActive ();
	}

	//called by SwipeUp/Down and by the arrow buttons on the GUI. Scrolls through categories. 
	public void scrollCategory(int newcat, bool up){
		// swipe Up and Down just call with +/- 1 so we do this here
		if (newcat == -1) {
			newcat = 4;
		}
		if (newcat == 5) {
			newcat = 0;
		}

		//change the button to match new category, since we didn't get here from pressing
		switch (newcat) {
		case 0:
			menucontroller.selectMenuType("appetizers");
			break;
		case 1:
			menucontroller.selectMenuType("mains");
			break;
		case 2:
			menucontroller.selectMenuType("drinks");
			break;
		case 3:
			menucontroller.selectMenuType("desserts");
			break;
		case 4:
			menucontroller.selectMenuType("specials");
			break;
		}

		activeFood.SetActive (false);
		activeCategory = newcat;
		if (up) {
			activeFoodIndex = dishes [activeCategory].Length - 1;
		} else {
			activeFoodIndex = 0; 
		}

		setActive ();
	}

	public void setActive(){
		
		activeFood = dishes [activeCategory] [activeFoodIndex];
		activeFood.SetActive (true);
		bpt.setText ();		bpt2.setText ();
	}

	public GameObject getActiveModel(){
		return activeFood;
	}

	public string getActiveName(){
		return activeFood.name;
	}

	public string getPrice(){
		Item item = database.getItem (activeFood.name);
		if (item != null) {
			return "$"+item.getPrice().ToString("F2");
		} else {
			return "No Item In Database";
		}
	}

	public string getDescription(){
		Item item = database.getItem (activeFood.name);
		if (item != null) {
			return item.getDescription () + "";
		} else {
			return "No Item In Database";
		}
	}

	// Update is called once per frame
	void Update () {

	}
}
