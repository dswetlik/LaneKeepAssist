using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HapticFeedback
{
    public class StateMachine : BaseMachine
    {



        public Robot _robot;

        BaseState _currentState;

        public DecisionState _decisionState;
        public PlanState _planState;
        public ExecuteState _executeState;

        #region State Machine Action Variables

        public enum Decision
        {
            off,
            ExploreSector,
            LeaveSector,
            AttemptLocate
        }

        public Decision CurrentDecision { get; private set; }

        #endregion

        private void Awake()
        {
            CurrentDecision = Decision.off;

            _decisionState = new DecisionState(this);
            _planState = new PlanState(this);
           
        }

        protected override BaseState GetInitialState()
        {
            return _decisionState;
        }

        public void SetDecision(Decision decision) { CurrentDecision = decision; }

    }
}
