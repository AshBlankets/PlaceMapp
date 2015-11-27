using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LogoFade : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		StartCoroutine (logoFade ());
	}

	private IEnumerator logoFade() {
		yield return new WaitForSeconds (3);
		for (int i = 0; i < 10; i++) {
			this.gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1.0f - (0.1f*i));
			yield return new WaitForSeconds(0.05f);
		}
		this.gameObject.SetActive (false);
	}
}
