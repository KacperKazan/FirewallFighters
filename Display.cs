using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Display : MonoBehaviour
{
    public static Display display;
    public GameObject screen;
    public Color detectedColour;
    public Color defaultColour;

    public DeathScreen DeathScreen;

    [SerializeField]
    private GameObject border;

    private Queue<Packet> queuedPackets;
    private float packetPosition;
    private bool packetInScanner;
    private Packet currentPacket;
    private bool detected;
    private float screenWidth;
    private float screenHeight;
    private float scrollMult;
    private bool distracted;
    private bool packetBlocked;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.manager.GameOverEvent += Clear;
        packetBlocked = false;
        distracted = false;
        scrollMult = 1f;
        display = this;
        queuedPackets = new Queue<Packet>();
        if (screen == null) {
            screen = GameObject.Find("Screen");
        }
        packetInScanner = false;
        detected = false;
        Vector2 size = border.GetComponent<SpriteRenderer>().size;
        screenWidth = size.x;
        screenHeight = size.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.manager.GameOver) {
            if (distracted && packetInScanner && !packetBlocked) {
                packetBlocked = true;
                currentPacket.DisableContents();
            }
            if (Input.GetKey(KeyCode.LeftArrow) && !distracted && GameManager.manager.stamina > 0) {
                GameManager.manager.stamina -= GameManager.manager.staminaDepletionRate * Time.deltaTime;
                scrollMult = 0.5f;
            } else if (Input.GetKey(KeyCode.RightArrow) && !distracted) {
                scrollMult = 2f;
                if (GameManager.manager.stamina < 1) {
                    GameManager.manager.stamina += 3f * GameManager.manager.staminaRecoveryRate * Time.deltaTime;
                }
            } else {
                scrollMult = 1f;
                if (GameManager.manager.stamina < 1) {
                    GameManager.manager.stamina += GameManager.manager.staminaRecoveryRate * Time.deltaTime;
                }
            }
            if (!packetInScanner && queuedPackets.Count > 0) {
                PutInScanner(queuedPackets.Dequeue());
            } else if (packetInScanner) {
                if (!detected && Input.GetKeyDown(KeyCode.Space) && !distracted) {
                    SetDetected();
                }
                AdvancePacket();
                if (packetPosition >= 1f) {
                    RemoveFromScanner();
                }
            }
        }
    }

    public void AddPacket(Packet p) {
        queuedPackets.Enqueue(p);
    }

    void AdvancePacket() {
        packetPosition += (float)(Time.deltaTime * GameManager.manager.GetScreenSpeed() * scrollMult);
        currentPacket.transform.localPosition = new Vector3(
        Math.Min(-0.5f * (screenWidth + Packet.boxWidth) + (screenWidth + Packet.boxWidth) * packetPosition,
            0.5f*(screenWidth + Packet.boxWidth)), 0);
    }

    void PutInScanner(Packet p) {
        currentPacket = p;
        currentPacket.transform.SetParent(screen.transform);
        currentPacket.transform.localPosition = new Vector3(-0.5f * (screenWidth + Packet.boxWidth), 0);
        p.box.SetActive(true);
        packetPosition = 0f;
        packetInScanner = true;
        if (distracted) {
            packetBlocked = true;
            currentPacket.DisableContents();
        }
    }

    void RemoveFromScanner() {
        currentPacket.box.SetActive(false);
        if (detected) {
            BadWire.badWire.AddPacket(currentPacket);
            ResetDetected();
        } else {
            GoodWire.goodWire.AddPacket(currentPacket);
        }
        packetInScanner = false;
        packetBlocked = false;
    }

    void ResetDetected() {
        detected = false;
        screen.GetComponent<SpriteRenderer>().color = defaultColour;
    }

    void SetDetected() {
        detected = true;
        //play noise 
        screen.GetComponent<SpriteRenderer>().color = detectedColour;
    }
    public void EndDistraction() {
        distracted = false;
        if (packetBlocked) {
            packetBlocked = false;
            currentPacket.EnableContents();
        }
    }
    public void StartDistraction() {
        distracted = true;
        if (packetInScanner) {
            packetBlocked = true;
            currentPacket.DisableContents();
        }
    }

    public void Clear() {
        if (currentPacket != null) {
            DestroyImmediate(currentPacket.gameObject);
            currentPacket = null;
            packetBlocked = false;
            packetInScanner = false;
            ResetDetected();
        }
        detected = false;
        while (queuedPackets.Count > 0) {
            DestroyImmediate(queuedPackets.Dequeue().gameObject);
        }
    }

    public void showLostInfo(BadItem badItem) {
        DeathScreen.gameObject.SetActive(true);
        DeathScreen.UpdateDeathScreenText(badItem);
    }
}
