using HutongGames.PlayMaker;
using UnityEngine;

namespace OldKekmet
{
    public class AntiStall : MonoBehaviour
    {

        Drivetrain _d;
        PlayMakerFSM _i;
        FsmBool _b;

        public float minRPM;

        void Start()
        {
            _d = GetComponent<Drivetrain>();
            _i = transform.GetChild(4).GetChild(0).GetComponents<PlayMakerFSM>()[0];
            _b = _i.FsmVariables.GetFsmBool("ACC");
        }

        void Update()
        {
            if (_b.Value)
                _d.minRPM = minRPM;
        }

        void LateUpdate()
        {
            if (_b.Value)
                _d.minRPM = minRPM;
        }

    }
}
