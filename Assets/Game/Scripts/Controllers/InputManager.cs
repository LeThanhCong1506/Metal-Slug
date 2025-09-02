using Game.Scripts.Gameplay;
using UnityEngine;

namespace Assets.Game.Scripts.Controllers
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private PlayerController player;
        private PlayerInputActions inputActions;
        private Vector2 moveInput;

        void Awake()
        {
            inputActions = new PlayerInputActions();
        }

        void OnEnable()
        {
            inputActions.Player.Enable();

            inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
            inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;

            inputActions.Player.Attack.performed += ctx => Attack();
            inputActions.Player.Jump.performed += ctx => Jump();
            inputActions.Player.Throw_Grenade.performed += ctx => ThrowGrenade();
        }

        void OnDisable()
        {
            inputActions.Player.Disable();
        }

        void Update()
        {
            HandleInputController();
        }

        private void HandleInputController()
        {
            // Corrected ternary operator syntax
            player.MovementManager.HorizontalMovement(moveInput.x < 0 ? Vector3.left : (moveInput.x > 0 ? Vector3.right : Vector3.zero));

            if (moveInput.x == 0) player.MovementManager.StopMoving();

            if (moveInput.y > 0)
            {
                Debug.Log("Up key pressed");
                player.MovementManager.LookUp();
            }
            else if (moveInput.y < 0)
            {
                Debug.Log("Down key pressed");
                player.MovementManager.DownMovement();
            }
            else
                player.MovementManager.DefaultBodyPosition();
        }

        void Attack()
        {
            player.AttackManager.Attack();
        }

        void Jump()
        {
            player.MovementManager.Jump();
        }

        void ThrowGrenade()
        {
            player.AttackManager.Throw();
        }
    }
}
