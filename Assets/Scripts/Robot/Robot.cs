using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Polarith.AI.Move;

namespace HapticFeedback
{
    public class Robot : MonoBehaviour
    {

        public RobotController RC { get; private set; }
        //public AIMContext AIMC { get; private set; }
        
        public bool CanHearSomething { get; private set; }
        public bool CanSeeSomething { get; private set; }

        public Sector CurrentSector { get; private set; }

        public float ConfidenceRating { get; private set; }

        public Vector3 GetPosition()
        {
            return transform.position;
        }

        private void Start()
        {
            RC = GetComponent<RobotController>();
            //AIMC = GetComponent<AIMContext>();
        }
    }
}
