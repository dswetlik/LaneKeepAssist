using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HapticFeedback
{
    public class TrackControl : MonoBehaviour
    {

        public struct Track
        {
            public WheelCollider frontWheel;
            public WheelCollider backWheel;
        }

        enum TrackType
        {
            Left,
            Right
        }

        public enum GearType
        {
            forward,
            reverse,
            brake
        }

        [SerializeField] Track _leftTrack;
        [SerializeField] Track _rightTrack;

        [SerializeField] GearType _currentGear;

        [SerializeField] float _accelerator;
        [SerializeField] float _maxTorque;

        WheelCollider _wc;

        private void Start()
        {
            _wc = GetComponent<WheelCollider>();
        }

        void FixedUpdate()
        {
            float input = _accelerator * _maxTorque;
            switch (_currentGear)
            {
                case GearType.forward:
                    _wc.motorTorque = input;
                    break;
                case GearType.reverse:
                    _wc.motorTorque = -input;
                    break;
                case GearType.brake:
                    _wc.motorTorque = 0;
                    _wc.brakeTorque = input;
                    break;
            }

        }

        public void AcceleratorInput(float input)
        {
            _accelerator = input;
        }

        public void SetGear(GearType gear)
        {
            _currentGear = gear;
        }
    }
}
