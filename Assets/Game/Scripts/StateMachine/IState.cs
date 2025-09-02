using UnityEngine;

namespace Game.Service
{
    public interface IState 
    {
        void OnEnter();
        void Update();
        void FixedUpdate();
        void OnExit();
    }

}