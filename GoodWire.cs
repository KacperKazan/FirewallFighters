using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodWire : Wire
{
    public static GoodWire goodWire;
    public AudioSource[] sound;

    public override void EndOfWire(Packet p) {
        bool isBad = p.IsBad();
        BadItem badItem = null;
        if (isBad) {
            badItem = p.getBadItem();
        }
        DestroyImmediate(p.gameObject); 
        if (isBad) {
            //Lose
            sound[0].Play();
            GameManager.manager.Lose(badItem);
        } else {
            sound[1].Play();
            EmotionManager.emotionManager.setHappy();
            Scorer.scorer.IncreaseScore();
        }
    }

    public override void SetUpStaticRef() {
        sound = GetComponents<AudioSource>();
        goodWire = this;
    }
}
