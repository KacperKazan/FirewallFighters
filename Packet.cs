using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class Packet : MonoBehaviour {
    public static float boxWidth = 7;
    public static float boxHeight = 7;
    public Vector3 iconScale;
    public float iconRadius; // Each good/bad icon is a circle; this is the radius of each
    public List<Item> items;
    public GameObject blip;
    public GameObject box;
    public GameObject contents;
    public GameObject packetBorderPrefab;
    public GameObject itemHolderPrefab;
    private bool containsBad;
    private BadItem badItem;
    private float k = 0.1f;

    public float startLevel;
    public float incrementLevel;

    public bool IsBad() {
        return containsBad;
    }
    public BadItem getBadItem() {
        return badItem;
    }

    private void Start() {
        GameManager gameManager = GameManager.manager;
        containsBad = gameManager.ContainsBad();
        int sizeGood = GameManager.manager.GetPacketSize();

        sizeGood = containsBad ? sizeGood - 1 : sizeGood;

        for (int i = 0; i < sizeGood; i++) {
            addGoodItem();
        }
        if (containsBad) {
            addBadItem();
        }

        var border = Instantiate(packetBorderPrefab, box.transform);
        border.GetComponent<SpriteRenderer>().size = new Vector2(boxWidth, boxHeight);

        foreach (var item in items) {
            item.transform.localScale = iconScale;
        }

        
        float width = items[0].transform.parent.GetComponent<SpriteRenderer>().size.x;

        //iconRadius = width * 1.4142f * 2f;
        iconRadius = width * 1.4142f * iconScale.x * 0.7f;
        PositionItems();
    }


    private Vector3 RandomRotation() {
        return new Vector3(0, 0, Random.Range(0, 360));
    }

    private Vector3 RandomPosition(float depth) {
        return new Vector3(Random.Range(-0.5f * boxWidth + iconRadius * 2, 0.5f * boxWidth - iconRadius * 2),
        Random.Range(-0.5f * boxHeight + iconRadius * 2, 0.5f * boxHeight - iconRadius * 2), depth);
    }

    float SquareDistance(Vector3 v1, Vector3 v2) {
        return (v1.x - v2.x) * (v1.x - v2.x) + (v1.y - v2.y) * (v1.y - v2.y);
    }

    private void PositionItems() {
        for (int i = 0; i < items.Count; i++) {
            bool failed = true;
            Vector3 temp = Vector3.zero;
            int attempts = 0;
            while (failed) {
                failed = false;
                temp = RandomPosition(startLevel + i * incrementLevel);
                for (int j = 0; j < i; j++) {
                    if (SquareDistance(temp, items[j].transform.parent.localPosition) < Mathf.Exp(-k * attempts) * iconRadius * iconRadius * 4) {
                        //too close, try again
                        failed = true;
                        break;
                    }
                }
                attempts++;
            }
            items[i].transform.parent.localPosition = temp;
            items[i].transform.parent.localEulerAngles = RandomRotation();
        }
    }

    public void DisableContents() {
        contents.SetActive(false);
    }
    public void EnableContents() {
        contents.SetActive(true);
    }

    private void addBadItem() {
        var type = KeyManager.instance.GenBadItem();
        GameObject prefab = Resources.Load("Prefabs/BadItems/" + type.ToString()) as GameObject;
        GameObject holder = Instantiate(itemHolderPrefab, contents.transform);
        GameObject item = Instantiate(prefab, holder.transform);


        var itemSize = item.GetComponent<SpriteRenderer>().size;
        float maxHeight = Mathf.Sqrt(iconScale.x * itemSize.x * iconScale.x * itemSize.x +
 iconScale.y * itemSize.y * iconScale.y * itemSize.y) * 1.1f;
        holder.GetComponent<SpriteRenderer>().size = new Vector2(maxHeight, maxHeight);
        badItem = item.GetComponent<BadItem>();
        int index = Random.Range(0, items.Count);
        items.Insert(index, item.GetComponent<Item>());
    }

    private void addGoodItem() {
        var type = KeyManager.instance.GenGoodItem();
        GameObject prefab = Resources.Load("Prefabs/GoodItems/" + type.ToString()) as GameObject;
        GameObject holder = Instantiate(itemHolderPrefab, contents.transform);
        GameObject item = Instantiate(prefab, holder.transform);

        var itemSize = item.GetComponent<SpriteRenderer>().size;
        float maxHeight = Mathf.Sqrt(iconScale.x * itemSize.x * iconScale.x * itemSize.x +
 iconScale.y * itemSize.y * iconScale.y * itemSize.y) * 1.1f;
        holder.GetComponent<SpriteRenderer>().size = new Vector2(maxHeight, maxHeight);

        items.Add(item.GetComponent<Item>());
    }

}
