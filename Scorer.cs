using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scorer : MonoBehaviour
{
    private Text scoreText;
    public long scoreValue;
    public static Scorer scorer;

    // Start is called before the first frame update
    void Start()
    {
        scorer = this;
        scoreValue = 0;
        scoreText = GetComponent<Text>();
        GameManager.manager.ResetEvent += Reset;
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + scoreValue;
    }

    private void Reset() {
        scoreValue = 0;
    }

    public void IncreaseScore() {
        scoreValue += GameManager.manager.CalcScore();
        GameManager.manager.sounds.pitch = Mathf.Pow(Math.Min(Math.Max(((float)scoreValue - 1_000f)/80_000f, 0f), 0.8f), 2) + 1f;
    }
}
