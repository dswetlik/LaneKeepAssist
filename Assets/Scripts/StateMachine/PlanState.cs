using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HapticFeedback
{
    public class PlanState : BaseState
    {

        protected StateMachine _stateMachine;

        

        public PlanState(StateMachine stateMachine) : base("Plan", stateMachine)
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

            _stateMachine.ChangeState(_stateMachine._executeState);
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
