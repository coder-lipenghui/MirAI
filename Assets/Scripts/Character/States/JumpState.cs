using UnityEngine;

namespace MirAI.Character
{
    public class JumpState : CharacterState
    {
        public JumpState(CharacterStateMachine controller) : base(controller) { }

        public override void Enter()
        {
            Controller.AnimationPlayer?.Play(Controller.JumpClip);
        }

        public override void Tick()
        {
            if (!Controller.IsGrounded)
            {
                return;
            }

            if (Mathf.Abs(Controller.HorizontalInput) > 0.1f)
            {
                Controller.ChangeToRun();
            }
            else
            {
                Controller.ChangeToIdle();
            }
        }
    }
}
