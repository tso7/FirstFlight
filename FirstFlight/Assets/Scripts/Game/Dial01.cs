using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Dial01 : MonoBehaviour {

	
	void Start () {
        
        float pos_x = Mathf.Abs((float)Screen.width / (float)Screen.height * -4.5f);

        transform.localPosition = new Vector3(pos_x, 0f, 0f);
	}
}
