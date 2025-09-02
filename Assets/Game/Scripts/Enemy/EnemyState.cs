using Game.Service;
using UnityEngine;

public class EnemyState : BaseState
{
    private readonly EnemyController controller;
    private readonly Animator animator;
    public EnemyState(EnemyController controller, Animator animator)
    {
        this.controller = controller;
        this.animator = animator;
    }
}
