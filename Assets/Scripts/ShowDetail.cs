using UnityEngine;

public class ShowDetail : MonoBehaviour {

	// Update is called once per frame
	void OnSelect () {
        transform.GetChild(0).gameObject.SetActive(true);
	}
}
