using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour {
    public GameObject instructions;
    public static GameManager manager;
    public int maxPacketSize;
    public float packetPeriodConstant;
    public float packetSizeIncreaseConstant;
    public float badProbability;
    [Range(0, 1)]
    public float wireSpeed;
    [Range(0.5f, 2.0f)]
    public float packetPeriod;
    public int initialPacketSize;
    public int initialPacketPeriod;
    public float maxSpeedTime;
    public GameObject packetPrefab;
    public float sinceLastPacket;
    public float minPacketPeriod;
    public bool GameOver;
    public int health;

    public StaminaBar staminaBar;
    public float stamina;
    public float staminaDepletionRate;
    public float staminaRecoveryRate;
    public AudioSource sounds;


    public delegate void GameOverHandler();
    public event GameOverHandler GameOverEvent;

    public delegate void ResetHandler();
    public event ResetHandler ResetEvent;

    private float time;

    void Awake() {
        if (manager == null) {
            manager = this;
        } else {
            Destroy(this);
        }
        sinceLastPacket = 0;
        GameOver = true;
        GameOverEvent += () => GameOver = true;
        ResetEvent += () => GameOver = false;
        ResetEvent += Reset;
    }

    void Start() {
        time = 0f;
        sounds = GetComponent<AudioSource>();
        stamina = 1f;
        health = 3;
        staminaBar.setColour(Color.green);
    }

    // Update is called once per frame
    void Update() {
        if (!GameOver) {
            time += Time.deltaTime;
            ManageStaminaBar();
            sinceLastPacket += Time.deltaTime;
            UpdatePacketPeriod();
            if (sinceLastPacket > packetPeriod) {
                Packet p = GeneratePacket();
                InputWire.inputWire.AddPacket(p);
                sinceLastPacket = 0;
            }
        } else {
            if (Input.GetKeyDown(KeyCode.Space)) {
                ResetEvent();
                instructions.SetActive(false);
            }
        }
    }

    public void ManageStaminaBar() {
        staminaBar.setSize(stamina);
        if (stamina < 0.3f) {
            if ((int)(stamina * 100f) % 2 == 0) {
                staminaBar.setColour(Color.white);
            } else {
                staminaBar.setColour(Color.green);
            }
        }
    }

    public void LoseHealth()
    {
        HeartManager.manager.LoseHeart();
        health--;
        if(health == 0)
        {
            GameOverEvent();
        }
    }

    Packet GeneratePacket() {
        GameObject temp = Instantiate(packetPrefab);
        temp.GetComponent<Packet>().box.SetActive(false);
        return temp.GetComponent<Packet>();
    }

    public double GetWireSpeed() {
        return wireSpeed;
    }
    public double GetScreenSpeed() {                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 
        return 1d / packetPeriod;
    }
    public void UpdatePacketPeriod()
    {
        float a = (initialPacketPeriod - minPacketPeriod) / (maxSpeedTime * maxSpeedTime) ;
        float b = 2 * (minPacketPeriod - initialPacketPeriod) / maxSpeedTime;
        float c = initialPacketPeriod;
        float t = time;
        packetPeriod = t < maxSpeedTime ? a * t * t + b * t + c : minPacketPeriod;
    }
    public int GetPacketSize() {
        float a = (initialPacketSize - maxPacketSize) / (maxSpeedTime * maxSpeedTime);
        float b = (maxPacketSize - initialPacketSize) / maxSpeedTime;
        float c = initialPacketSize;
        return (int)Mathf.Min(maxPacketSize, a * time * time + b * time + c);
    }

    public bool ContainsBad() {
        return (Random.value < badProbability);
    }
    public long DifficultyMult() {
        return (long)((double)GetPacketSize() / initialPacketSize);
    }
    public long CalcScore() {
        return (long)(1000L * GetScreenSpeed() * DifficultyMult());
    }
    public void Lose(BadItem badItem) {
        HeartManager.manager.LoseAll();
        Display.display.showLostInfo(badItem);
        sounds.Stop();
        GameOverEvent();
    }
    public void Penalise() {
        //Distract FF
        FF.fF.CheckMonitor();
        Display.display.StartDistraction();
        LoseHealth();
    }
    private void Reset() {
        sinceLastPacket = 0;
        stamina = 1f;
        time = 0f;
        sounds.pitch = 1f;
        sounds.Play();
        health = 3;
        HeartManager.manager.GainAll();
    }
}
