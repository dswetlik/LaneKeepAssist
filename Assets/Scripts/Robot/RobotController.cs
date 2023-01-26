using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HapticFeedback
{
    public class RobotController : MonoBehaviour
    {

        TrackControl _rightTrack;
        TrackControl _leftTrack;

        float _rightInput;
        float _leftInput;

        float _rightAIInput;
        float _leftAIInput;

        void Start()
        {
            _rightTrack = GameObject.Find("RightTrack").GetComponent<TrackControl>();
            _leftTrack = GameObject.Find("LeftTrack").GetComponent<TrackControl>();
        }

        void Update()
        {
            _rightInput = Engine.DualSense.R2.ReadValue();
            _leftInput = Engine.DualSense.L2.ReadValue();

            _rightInput += _rightAIInput;
            _leftInput += _leftAIInput;

            CheckGear(); 

            _rightTrack.AcceleratorInput(_rightInput);
            _leftTrack.AcceleratorInput(_leftInput);

        }

        void CheckGear()
        {
            if (Engine.DualSense.L1.isPressed || (Engine.DualSense.L1.isPressed && Engine.DualSense.R1.isPressed))
            {
                _rightTrack.SetGear(TrackControl.GearType.brake);
                _leftTrack.SetGear(TrackControl.GearType.brake);
            }
            else if (Engine.DualSense.R1.isPressed && !Engine.DualSense.L1.isPressed)
            {
                _rightTrack.SetGear(TrackControl.GearType.reverse);
                _leftTrack.SetGear(TrackControl.GearType.reverse);
            }
            else
            {
                _rightTrack.SetGear(TrackControl.GearType.forward);
                _leftTrack.SetGear(TrackControl.GearType.forward);
            }
        }

        public void SetAIInput(float left, float right)
        {
            _leftAIInput = left;
            _rightAIInput = right;
        }
    }
}
