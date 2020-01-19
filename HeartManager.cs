using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartManager : MonoBehaviour
{
    public static HeartManager manager;
    public List<GameObject> hearts;
    // Start is called before the first frame update
    void Start()
    {
        manager = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoseHeart()
    {
        hearts[GameManager.manager.health - 1].SetActive(false);
    }

    public void LoseAll()
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            hearts[i].SetActive(false);
        }
    }

    public void GainAll()
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            hearts[i].SetActive(true);
        }
    }
}
