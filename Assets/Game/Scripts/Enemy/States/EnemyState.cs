using Game.Service;
using UnityEngine;

public class EnemyState : BaseState
{
    protected readonly EnemyBase controller;
    public EnemyState(EnemyBase controller)
    {
        this.controller = controller;
    }
}
