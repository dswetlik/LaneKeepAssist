using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HapticFeedback
{
    public class DecisionState : BaseState
    {

        protected StateMachine _stateMachine;

        public DecisionState(StateMachine stateMachine) : base("Decision", stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void UpdateLogic()
        {
            base.UpdateLogic();

            if (_stateMachine._robot.CanHearSomething || _stateMachine._robot.CanSeeSomething)
            {
                _stateMachine.SetDecision(StateMachine.Decision.AttemptLocate);
            }
            else
            {
                if (!_stateMachine._robot.CurrentSector.HasBeenExplored)
                {
                    _stateMachine.SetDecision(StateMachine.Decision.ExploreSector);
                }
                else
                {
                    _stateMachine.SetDecision(StateMachine.Decision.LeaveSector);
                }
            }

            _stateMachine.ChangeState(_stateMachine._planState);
        }

        public override void UpdatePhysics()
        {
            base.UpdatePhysics();
        }

        public override void Exit()
        {
            base.Exit();
        }

    }
}
