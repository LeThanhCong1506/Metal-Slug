using Game.Service;
using UnityEngine;

public class EnemyState : BaseState
{
    protected readonly EnemyController controller;
    public EnemyState(EnemyController controller)
    {
        this.controller = controller;
    }
}
