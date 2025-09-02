namespace Game.Service
{
    public abstract class BaseState : IState
    {
        public virtual void FixedUpdate()
        {
        }

        public virtual void OnEnter()
        {
        }

        public virtual void OnExit()
        {
        }

        public virtual void Update()
        {
        }
    }
}