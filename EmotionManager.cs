using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionManager : MonoBehaviour
{
    public static EmotionManager emotionManager;
    public Sprite thinking;
    public Sprite exclamation;
    public Sprite happy;
    public Sprite sad;
    private float currentEmotionTime;
    public float maxEmotionTime;
    private float counter;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        counter = 0;
        emotionManager = this;
        GameManager.manager.GameOverEvent += setSad;
        currentEmotionTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<SpriteRenderer>().sprite != thinking)
        {
            currentEmotionTime += Time.deltaTime;
        }
        else
        {
            currentEmotionTime = 0;
        }
        if (currentEmotionTime > maxEmotionTime)
        {
            GetComponent<SpriteRenderer>().sprite = thinking;
            currentEmotionTime = 0;
        }
    }

    public void setExclamation()
    {
        GetComponent<SpriteRenderer>().sprite = exclamation;
    }
    public void setHappy()
    {
        GetComponent<SpriteRenderer>().sprite = happy;
    }
    public void setSad()
    {
        GetComponent<SpriteRenderer>().sprite = sad;
    }
}
