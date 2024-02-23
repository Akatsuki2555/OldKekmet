using HutongGames.PlayMaker;
using UnityEngine;

namespace OldKekmet
{
    public class AntiStall : MonoBehaviour
    {

        Drivetrain drivertrain;
        PlayMakerFSM ignition;
        FsmBool accState;

#if ADVANCED_FEATURES
        public float minRPM;
#endif

        void Start()
        {
            drivertrain = GetComponent<Drivetrain>();
            ignition = transform.GetChild(4).GetChild(0).GetComponents<PlayMakerFSM>()[0];
            accState = ignition.FsmVariables.GetFsmBool("ACC");
        }

        void Update()
        {
#if ADVANCED_FEATURES
            if (accState.Value)
                drivertrain.minRPM = minRPM;
#else
            if (accState.Value)      
                drivertrain.minRPM = 500;
#endif
        }

        void LateUpdate()
        {
#if ADVANCED_FEATURES
            if (accState.Value)
                drivertrain.minRPM = minRPM;
#else
            if (accState.Value)
                drivertrain.minRPM = 500;
#endif
        }

    }
}
