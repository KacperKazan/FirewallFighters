using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Wire : MonoBehaviour
{
    public class PacketPosition
    {
        public void IncrementPosition(float dist)
        {
            position += dist;
        }
        public Packet packet;
        public float position;
        public PacketPosition(Packet p, float pos)
        {
            packet = p;
            position = pos;
        }
    }

    [SerializeField]
    private float wireLength;

    Queue<PacketPosition> queuedPackets;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.manager.GameOverEvent += Clear;
        queuedPackets = new Queue<PacketPosition>();
        SetUpStaticRef();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.manager.GameOver) {
            float distance = (float)(Time.deltaTime * GameManager.manager.GetWireSpeed());

            // Move each packet to the right by speed * time
            foreach (PacketPosition p in queuedPackets) {
                AdvancePacket(p, distance);
            }

            // Remove packets which are off the end of the screen
            while (queuedPackets.Count > 0 && queuedPackets.Peek().position >= 1f) {
                EndOfWire(queuedPackets.Dequeue().packet);
            }
        }
    }

    // Called when a packet reaches the end of the wire,
    // always called with the packet which reached the end
    public abstract void EndOfWire(Packet p);

    public abstract void SetUpStaticRef();

    public void AddPacket(Packet p)
    {
        p.blip.SetActive(true);
        p.transform.SetParent(transform);
        p.transform.localPosition = new Vector3(-0.5f * wireLength, 0);
        queuedPackets.Enqueue(new PacketPosition(p, 0f));
    }

    void AdvancePacket(PacketPosition p, float dist) {
        p.IncrementPosition(dist);
        p.packet.transform.localPosition = new Vector3(
        Math.Min(0.5f * (wireLength), (p.position - 0.5f) * wireLength), 0);
    }

    public void Clear() {
        while (queuedPackets.Count > 0) {
            DestroyImmediate(queuedPackets.Dequeue().packet.gameObject);
        }
    }
}
