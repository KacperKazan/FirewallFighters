using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputWire : Wire
{
    public static InputWire inputWire;
    public AudioSource sound;

    public override void SetUpStaticRef() {
        sound = GetComponent<AudioSource>();
        inputWire = this;
    }


    public override void EndOfWire(Packet p)
    {
        //sounds.Play();
        p.blip.SetActive(false);
        Display.display.AddPacket(p);
    }
}
