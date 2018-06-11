using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class animAppear : MonoBehaviour {

    public float animationTime = 0.01f;
    public Texture2D texture;

    private Image image;
    private Color panelColor = Color.black;

	// Use this for initialization
	void Start () {
        image = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
        if(Time.timeSinceLevelLoad < animationTime){
            //perform animation

            float timeChange = Time.deltaTime / animationTime;
            panelColor.a -= timeChange;
            image.color = panelColor;
        }
        else{
            gameObject.SetActive(false);
        }
		
	}
}
