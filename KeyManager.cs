using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class KeyManager : MonoBehaviour {
    public static KeyManager instance;
    public List<GameObject> keyIcons;
    private int level = 0;

    int badItemsLength = Enum.GetNames(typeof(BadItemType)).Length;
    int goodItemsLength = Enum.GetNames(typeof(GoodItemType)).Length;

    // Start is called before the first frame update
    void Start() {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        GameManager.manager.ResetEvent += Reset;

    }
    public void Reset() {
        level = 0;
        for (int i = 1; i < badItemsLength; i++) {
            keyIcons[i].GetComponent<Image>().color = Color.black;
            keyIcons[i].gameObject.SetActive(false);
        }
    }

    public void LevelUp() {
        if (level < badItemsLength) {
            keyIcons[level].GetComponent<Image>().color = Color.black;
            level += 1;
            if (level < badItemsLength) {
                keyIcons[level].SetActive(true);
                keyIcons[level].GetComponent<Image>().color = Color.red;
            }
        }
    }

    public BadItemType GenBadItem() {
        int index = Random.Range(0, level);
        return (BadItemType)index;
    }
    public GoodItemType GenGoodItem() {
        int index = Random.Range(0, goodItemsLength);
        return (GoodItemType)index;
    }
}
