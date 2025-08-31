using UnityEngine;

namespace Game.Scripts.Gameplay
{
    public class PlayerController : MonoBehaviour
    {
        private AnimationManager animationManager;
        private MovementManager movementManager;
        private PlayerAttackManager attackManager;
        private PhysicManager physicManager;
        private PlayerAttackManager playerAttackManager;

        public MovementManager MovementManager => movementManager;
        public PlayerAttackManager AttackManager => attackManager;
        public AnimationManager AnimationManager => animationManager;
        public PhysicManager PhysicManager => physicManager;
        public PlayerAttackManager PlayerAttackManager => playerAttackManager;

        private void Start()
        {
            animationManager = GetComponent<AnimationManager>();
            movementManager = GetComponent<MovementManager>();
            attackManager = GetComponent<PlayerAttackManager>();
            physicManager = GetComponent<PhysicManager>();
            playerAttackManager = GetComponent<PlayerAttackManager>();
        }
    }
}