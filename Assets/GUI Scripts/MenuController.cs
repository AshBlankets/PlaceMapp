using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;

//Script responsible for the behaviour of the Menu GUI (header and left panel of food categories)
public class MenuController : MonoBehaviour {
	
	//Masks used for GUI animation
	public Mask highlightMask;
	public Mask expandMask;
	
	//Text and images changed by script events
	public Text headerText;
	public Image leftHeaderIcon;
	public Image rightHeaderIcon;
	public Sprite headerBurger;
	public Sprite headerArrow;
	public Sprite reviewIcon;
	
	//Confirmation popup sprite (to be faded out)
	public GameObject confirmCanvas;
	public CanvasGroup confirmImage;
	
	//Menu settings set to defaults
	private string menuType = "specials";
	private bool isExpanded = false;
	private bool orderOut = false;
	
	//settings for getting rid of splash
	private bool splashGone = false;
	
	public int animFrames; //Number of frames for the expand animation
	public float animTime; //Period of time to animate expand over
	
	private OrderPage orderPage; //OrderPage so we can tell it to update the info
	private GameObject leftMenu = null; //Used to set left menu GUI active/inactive when order menu is brought out
	private GameObject bottomMenu = null; //same thing but for splash
	
	// Use this for initialization
	void Start () {
		orderPage = GameObject.FindGameObjectWithTag("OrderPage").GetComponent<OrderPage>();
		bottomMenu = GameObject.FindGameObjectWithTag("BottomMenu");
		bottomMenu.SetActive (false);

		//Make sure slider is visible
		RectTransform t = GameObject.FindGameObjectWithTag ("OrderPanel").GetComponent<RectTransform> ();
		int width = Screen.currentResolution.width;
		if (width < 1080)
			width = 1080;
		t.anchoredPosition = new Vector2 (width, 0);


	}
	
	//Method called when the header burger button is pressed - expands/shrinks the left menu
	public void expand() {
		
		//Fake a closing swipe
		if (orderOut) {
			swipeOrder(true);
			return;
		}
		
		isExpanded = !isExpanded;
		
		//Go through all buttons and increase/decrease their width appropriately
		foreach (GameObject obj in GameObject.FindGameObjectsWithTag("MenuButton")) {
			RectTransform t = obj.GetComponent<RectTransform> ();
			if (isExpanded)
				t.sizeDelta = new Vector2 (540, 150);
			else
				t.sizeDelta = new Vector2 (150, 150);
		}
		if (!isExpanded)
			leftHeaderIcon.sprite = headerBurger;
		else
			leftHeaderIcon.sprite = headerArrow;
		
		//Start the animation on the mask
		StartCoroutine (expandAnim());
	}
	
	//Method called when one of the menu buttons is pressed.
	//Stores the new type and changes the position of the mask
	public void selectMenuType(string type) {
		string oldType = menuType;
		menuType = type;
		Debug.Log ("Got here with type: " + type);
		RectTransform transform = highlightMask.rectTransform;
		
		switch (type) {
		case "appetizers":
			transform.anchoredPosition = new Vector2(0, -150);
			rightHeaderIcon.sprite = GameObject.Find ("Appetizers Icon").GetComponent<Image>().sprite;
			headerText.text = "APPETIZERS";
			break;
			
		case "mains":
			transform.anchoredPosition = new Vector2(0, -300);
			rightHeaderIcon.sprite = GameObject.Find ("Mains Icon").GetComponent<Image>().sprite;
			headerText.text = "MAINS";
			break;
			
		case "specials":
			transform.anchoredPosition = new Vector2(0, 0);
			rightHeaderIcon.sprite = GameObject.Find ("Specials Icon").GetComponent<Image>().sprite;
			headerText.text = "SPECIALS";
			break;
			
		case "desserts":
			transform.anchoredPosition = new Vector2(0, -600);
			rightHeaderIcon.sprite = GameObject.Find ("Desserts Icon").GetComponent<Image>().sprite;
			headerText.text = "DESSERTS";
			break;
			
		case "drinks":
			transform.anchoredPosition = new Vector2(0, -450);
			rightHeaderIcon.sprite = GameObject.Find ("Drinks Icon").GetComponent<Image>().sprite;
			headerText.text = "DRINKS";
			break;
			
		case "help":
			transform.anchoredPosition = new Vector2(0, -750);
			rightHeaderIcon.sprite = GameObject.Find ("Help Icon").GetComponent<Image>().sprite;
			headerText.text = "HELP";
			break;
			
		default:
			Debug.Log("Unknown menu type: " + type);
			menuType = oldType;
			break;
		}
	}
	
	public void swipeOrder(bool isRight) {
		
		//Swipe order menu away
		if (isRight && orderOut) {
			orderOut = false;
			StartCoroutine (swipeAnim ());
			if (!isExpanded) {
				leftHeaderIcon.sprite = headerBurger;
				
			}
			else
				leftHeaderIcon.sprite = headerArrow;
			
		}
		
		//Swipe order menu into focus
		if (!isRight && !orderOut) {
			orderOut = true;
			orderPage.createOrders(); 
			StartCoroutine (swipeAnim ());
		}
		
	}
	
	
	//seems easiest
	public void swipeSplash() {
		if (splashGone == false) {
			splashGone = true;
			StartCoroutine (splashAnim ());			
		}
		
	}
	
	public void confirmPurchase() {
		StartCoroutine (confirmAnim ());
	}
	
	//Menu size change animation. Increases/decreases the width of the mask
	private IEnumerator expandAnim() {
		RectTransform t = expandMask.rectTransform;
		for (int i = 0; i < animFrames; i++) {
			if (isExpanded) {
				if (i == 0)
					Debug.Log (t.sizeDelta.ToString ());
				t.sizeDelta = new Vector2(150 + (390/animFrames)*i, 1770);
			}
			else {
				if (i == 0)
					Debug.Log (t.sizeDelta.ToString ());
				t.sizeDelta = new Vector2(540 - (390/animFrames)*i, 1770);
			}
			yield return new WaitForSeconds(animTime/animFrames);
		}
		
		//Make sure final sizes are correct
		if (isExpanded) {
			t.sizeDelta = new Vector2(540, 1770);
		} else {
			t.sizeDelta = new Vector2(150, 1770);
		}
	}
	
	//Order menu swipe animation. Translates order panel to the left or right
	private IEnumerator swipeAnim() {
		Debug.Log ("IN SWIPE ANIM");
		RectTransform t = GameObject.FindGameObjectWithTag ("OrderPanel").GetComponent<RectTransform> ();
		int width = Screen.currentResolution.width;
		if (width < 1080)
			width = 1080; //Kludge but it works
		for (int i = 0; i < animFrames; i++) {
			if (orderOut) {
				
				t.anchoredPosition = new Vector2(width - (width/animFrames)*i, 0);
			}
			else {
				t.anchoredPosition = new Vector2(0 + (width/animFrames)*i, 0);
			}
			yield return new WaitForSeconds((animTime/animFrames)*2);
		}
		if (orderOut) {
			t.anchoredPosition = new Vector2(0, 0);
			if (leftMenu == null)
				leftMenu = GameObject.FindGameObjectWithTag ("LeftMenu");
			leftMenu.SetActive (false);
			leftHeaderIcon.sprite = headerArrow;
			rightHeaderIcon.sprite = reviewIcon;
			headerText.text = "REVIEW ORDER";
			
		} else {
			orderPage.destroyOrders();
			t.anchoredPosition = new Vector2(width, 0);
			leftMenu.SetActive (true);
			selectMenuType (menuType); //Resets right header icon
			
		}
	}
	
	
	//piggybacking off this :V
	private IEnumerator splashAnim() {
		RectTransform t = GameObject.FindGameObjectWithTag ("SplashPanel").GetComponent<RectTransform> ();
		int width = Screen.currentResolution.width;
		
		for (int i = 0; i < animFrames; i++) {
			t.anchoredPosition = new Vector2(0 - (width/animFrames)*i, 0);
			//t.anchoredPosition = new Vector2(0 + (width/animFrames)*i, 0);
			yield return new WaitForSeconds((animTime/animFrames)*2);
		}
		
		
		GameObject.FindGameObjectWithTag ("SplashCanvas").SetActive (false); //turn it off when we're done
		bottomMenu.SetActive (true);
		
	}
	
	//Confirm panel animation - fades out, then sets itself inactive
	private IEnumerator confirmAnim() {
		yield return new WaitForSeconds (1);
		for (int i = 0; i < animFrames; i++) {
			confirmImage.alpha = (1.0f - (1.0f/animFrames)*i);
			yield return new WaitForSeconds(1/animFrames);
		}
		confirmImage.alpha = 1;
		confirmCanvas.SetActive(false);
	}
}
