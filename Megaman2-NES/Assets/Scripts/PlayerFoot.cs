using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFoot : MonoBehaviour
{
        private int footContact_ = 0;
        private int spikeContact_ = 0;

    public int FootContact => footContact_;
    public int SpikeContact => spikeContact_;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
                footContact_++;
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Spike"))
        {
            spikeContact_++;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Platform"))
        {
                footContact_--;
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Spike"))
        {
            spikeContact_--;
        }
    }
}
