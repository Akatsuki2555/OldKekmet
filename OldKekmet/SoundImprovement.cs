using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace OldKekmet
{
    public class SoundImprovement : MonoBehaviour
    {
        AudioSource throttle, noThrottle;

#if ADVANCED_FEATURES
        public float threshold;
#endif


        void Start()
        {
            noThrottle = transform.GetChild(21).GetComponent<AudioSource>();
            throttle = transform.GetChild(22).GetComponent<AudioSource>();
        }

        void Update()
        {
#if ADVANCED_FEATURES
            if (throttle.volume > threshold) noThrottle.Stop();
#endif
            if (throttle.volume > 0.5f) noThrottle.Stop();
            else if (!noThrottle.isPlaying) noThrottle.Play();
        }
    }
}
