using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadWire : Wire
{
    public static BadWire badWire;
    public AudioSource[] sound;

    public override void SetUpStaticRef() {
        sound = GetComponents<AudioSource>();
        badWire = this;
    }

    public override void EndOfWire(Packet p) {
        bool isBad = p.IsBad();
        DestroyImmediate(p.gameObject);
        if (isBad) {
            sound[0].Play();
            Scorer.scorer.IncreaseScore();
            EmotionManager.emotionManager.setHappy();

            // So far leveling up once you spot a malicious item
            KeyManager.instance.LevelUp();
        } else {
            sound[1].Play();
            GameManager.manager.Penalise();
        }
    }
}
