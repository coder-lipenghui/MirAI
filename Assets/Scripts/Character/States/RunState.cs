using UnityEngine;

namespace MirAI.Character
{
    public class RunState : CharacterState
    {
        public RunState(CharacterStateMachine controller) : base(controller) { }

        public override void Enter()
        {
            Controller.AnimationPlayer?.Play(Controller.RunClip);
        }

        public override void Tick()
        {
            if (!Controller.IsGrounded)
            {
                Controller.ChangeToJump();
                return;
            }

            if (Mathf.Abs(Controller.HorizontalInput) <= 0.1f)
            {
                Controller.ChangeToIdle();
                return;
            }

            if (Controller.ConsumeJumpQueued() && Controller.IsGrounded)
            {
                Controller.ChangeToJump();
            }
        }
    }
}
