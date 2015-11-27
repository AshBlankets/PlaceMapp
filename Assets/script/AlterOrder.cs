using UnityEngine;
using System.Collections;

/*
 * Welcome to the AlterOrder script
 * This just passes the message along to the OrderPage that the add or remove button has been hit
 */
public class AlterOrder : MonoBehaviour {
	public int action; //if placed on the delete button this is 0, if on the add button this is 1

	private OrderPage orderPage;

	// Use this for initialization
	void Start () {
		orderPage = GameObject.FindGameObjectWithTag("OrderPage").GetComponent<OrderPage>(); //Object with OrderPage tag should have OrderPage script
		//Debug.Log (GameObject.FindGameObjectWithTag("OrderPage").name);
	
		GameObject[] gOs = GameObject.FindGameObjectsWithTag("OrderPage");
		Debug.Log("Started!?");
		foreach (GameObject game in gOs) {
			Debug.Log(game.name);
		}

	}
	
	// Update is called once per frame
	void Update () {
	}

	public void alterOrder(){
		orderPage.orderQuantity (this.transform.parent.name, action);
	}

}
