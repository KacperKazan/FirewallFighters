using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FF : MonoBehaviour
{
    public static FF fF;
    public float moveSpeed;
    public Vector3 initial;
    public Vector3 target;
    public Sprite faceRight;
    public Sprite faceForward;
    public GameObject emotion;
    private float facingTime;
    public float distractedTime;
    public bool facing;
    private float counter;
    // Start is called before the first frame update
    void Start()
    {
        emotion.SetActive(false);
        transform.localPosition = initial;
        counter = 0;
        fF = this;
        facing = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.localPosition == target)
        {
            emotion.SetActive(true);
        }
        if (!GameManager.manager.GameOver)
        {
            counter += Time.deltaTime;
        }
        if(counter > 2)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, moveSpeed * Time.deltaTime);
        }

        if(GetComponent<SpriteRenderer>().sprite == faceRight)
        {
            facingTime += Time.deltaTime;
        }
        else
        {
            facingTime = 0;
        }
        if(facingTime > distractedTime)
        {
            GetComponent<SpriteRenderer>().sprite = faceForward;
            facingTime = 0;
            facing = true;
            Display.display.EndDistraction();
        }

    }

    public void CheckMonitor()
    {
        GetComponent<SpriteRenderer>().sprite = faceRight;
        EmotionManager.emotionManager.setExclamation();
        facing = false;
    }
}
