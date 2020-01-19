using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaBar : MonoBehaviour {
    
    [SerializeField]
    private Transform barTransform;
    // Start is called before the first frame update
    void Start() {
    }

    public void setSize(float size) {
        barTransform.localScale = new Vector3(size, 1f);
    }

    public void setColour(Color colour) {
        barTransform.Find("BarSprite").GetComponent<SpriteRenderer>().color = colour;
    }
    // Update is called once per frame
    void Update() {

    }
}
