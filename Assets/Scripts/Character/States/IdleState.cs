using UnityEngine;

namespace MirAI.Character
{
    public class IdleState : CharacterState
    {
        public IdleState(CharacterStateMachine controller) : base(controller) { }

        public override void Enter()
        {
            Controller.AnimationPlayer?.Play(Controller.IdleClip);
        }

        public override void Tick()
        {
            if (!Controller.IsGrounded)
            {
                Controller.ChangeToJump();
                return;
            }

            if (Mathf.Abs(Controller.HorizontalInput) > 0.1f)
            {
                Controller.ChangeToRun();
                return;
            }

            if (Controller.ConsumeJumpQueued() && Controller.IsGrounded)
            {
                Controller.ChangeToJump();
            }
        }
    }
}
