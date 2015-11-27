using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SwipeHandler : MonoBehaviour
{

	Vector2 prevPos = new Vector2 (0, 0);
	public int minDistance;
	public FoodHandler foodhandler;
	public MenuController menuController;


	//swipe variables
	private float fingerStartTime = 0.0f;
	private Vector2 fingerStartPos = Vector2.zero;
	private bool isSwipe = false;
	private float minSwipeDist = 50.0f;
	private float maxSwipeTime = 0.5f;
	private bool orderOut = false;
	private bool splashOut = true;
	public int furthest;

	private Scrollbar scroll;
	
	// Use this for initialization
	void Start ()
	{
		scroll = GameObject.Find ("Splash Scrollbar").GetComponent<Scrollbar> ();
	}



	// Checks for swipes and cycles the foodhandler
	void Update ()
	{
		if (Input.touchCount > 0) {
			foreach (Touch touch in Input.touches) {
				switch (touch.phase) {
				case TouchPhase.Began:
					/* this is a new touch */
					isSwipe = true;
					fingerStartTime = Time.time;
					fingerStartPos = touch.position;
					break;
				
				case TouchPhase.Moved:
					if (splashOut) {
						scroll.value += (touch.deltaPosition.x / (Screen.width / 12.0f));
					}
					break;

				
				case TouchPhase.Canceled:
					/* The touch is being canceled */
					isSwipe = false;
					break;
					
				case TouchPhase.Ended:
					float gestureTime = Time.time - fingerStartTime;
					float gestureDist = (touch.position - fingerStartPos).magnitude;
						
					if (isSwipe && gestureTime < maxSwipeTime && gestureDist > minSwipeDist) {
						Vector2 direction = touch.position - fingerStartPos;
						
						if (Mathf.Abs (direction.y) > Mathf.Abs (direction.x)) {
							// the swipe is vertical:
							if (fingerStartPos.y > touch.position.y) {								
								if (!orderOut)
									foodhandler.swipeDown ();
							} else {								
								if (!orderOut)
									foodhandler.swipeUp ();
							}
						}
						//Horizontal swipe
						else if (Mathf.Abs (direction.x) > Mathf.Abs (direction.y)) {

							if (!splashOut) {
								menuController.swipeOrder (fingerStartPos.x < touch.position.x);
								if (fingerStartPos.x < touch.position.x && orderOut) {
									orderOut = false;
								} else if (fingerStartPos.x > touch.position.x && !orderOut) {
									orderOut = true;
								}
							}
						}
					
					}
					break;
				}
			}
		}
	}
						
	//THIS IS THE MOUSE CODE!! CLICK SOMEWHERE AND THEN FAR AWAY TO SWIPE IN THAT DIRECTION 
	/*
		if (Input.GetMouseButton(0)) {
		// Get movement of the finger since last frame
			if(prevPos.x==0 && prevPos.y==0){ prevPos.x = Input.mousePosition.x; prevPos.y = Input.mousePosition.y;}
			else{
				float Rightdistance = (Input.mousePosition.x - prevPos.x);
				float Leftdistance = (prevPos.x - Input.mousePosition.x);
				Debug.Log ("RDist - "+Rightdistance);

				if(Rightdistance > minDistance){
					Debug.Log("Swiping Right!");
					menuController.swipeOrder(true);
				}else if(Leftdistance > minDistance){
					Debug.Log("Swiping Left!");
					menuController.swipeOrder(false);
				}

				prevPos.x = Input.mousePosition.x; prevPos.y = Input.mousePosition.y;

			}				
		}

	*/

	//Deprecated code

	/*
	public void toHere(float pos){
		StartCoroutine (moveBar (Input.mousePosition));
	}

	private IEnumerator moveBar(Vector2 target) {
	//	Vector2 t = greenBar.transform.position;
		RectTransform t = greenBar.GetComponent<RectTransform> ();
		Debug.Log ("Moving mouse from " + t.anchoredPosition.x + " to " + target.x);

		int animFrames = 20;
		float animTime = 0.25f;

		Debug.Log ("Furthest is: " + furthest);

		
		int final = (int) target.x;
		int diff = (int) Mathf.Abs(final - t.anchoredPosition.x);

		if ((t.anchoredPosition.x+diff) > furthest) {
			final = furthest;
			diff = (int) Mathf.Abs(final - t.anchoredPosition.x);
		}

		for (int i = 0; i < animFrames; i++) {
			//greenBar.transform.position = new Vector2(final + (diff/animFrames)*i, greenBar.transform.position.y);
			t.anchoredPosition = new Vector2(t.anchoredPosition.x + (diff/animFrames), t.anchoredPosition.y);
			//t.anchoredPosition = new Vector2(0 + (width/animFrames)*i, 0);
			yield return new WaitForSeconds(animTime/animFrames);
		}


		if (final == furthest) {
			splashOut = false;
			menuController.swipeSplash();
		}
	}
	*/

	//Hide splash screen if scrolled fully
	public void scrollSplash (GameObject slider)
	{
		float pos = slider.GetComponent<Scrollbar> ().value;
		if (pos == 0) {
			menuController.swipeSplash ();
			StartCoroutine (allowOrder());
		}
	}

	private IEnumerator allowOrder() {
		yield return new WaitForSeconds(1);
		splashOut = false;
	}
	/*
	private IEnumerator snapBack ()
	{
		//	Vector2 t = greenBar.transform.position;
		RectTransform t = greenBar.GetComponent<RectTransform> ();
		
		int animFrames = 20;
		float animTime = 0.25f;
		
		
		int final = (int)greenOrigin.anchoredPosition.x;
		int diff = (int)(t.anchoredPosition.x - greenOrigin.anchoredPosition.x);
		
		for (int i = 0; i < animFrames; i++) {
			//greenBar.transform.position = new Vector2(final + (diff/animFrames)*i, greenBar.transform.position.y);
			t.anchoredPosition = new Vector2 (t.anchoredPosition.x - (diff / animFrames), t.anchoredPosition.y);
			//t.anchoredPosition = new Vector2(0 + (width/animFrames)*i, 0);
			yield return new WaitForSeconds (animTime / animFrames);
		}
	}*/


}
