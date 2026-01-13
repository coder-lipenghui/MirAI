using MirAI.FSM;

namespace MirAI.Character
{
    public abstract class CharacterState : IState
    {
        protected CharacterStateMachine Controller { get; }

        protected CharacterState(CharacterStateMachine controller)
        {
            Controller = controller;
        }

        public virtual void Enter() { }
        public virtual void Tick() { }
        public virtual void Exit() { }
    }
}
