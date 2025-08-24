using System.Collections;
using DenkKits.GameServices.Audio.Scripts;
using Game.Scripts.Controllers;
using UnityEngine;

namespace Game.Scripts.Gameplay
{
    public class PlayerController : MonoBehaviour
    {
        // this should be in movement manager
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float jumpForce = 10f;
        [SerializeField] private Transform groundCheck;
        [SerializeField] private float groundCheckRadius = 0.2f;
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private Rigidbody2D rb;


        private AnimationManager _animationManager;
        private MovementManager _movementManager;
        private PlayerAttackManager _attackManager;
        private PhysicManager _physicManager;

        // this should be in movement manager
        private bool _isGrounded;
        private int _jumpCount = 0;

        private const int _maxJumpCount = 2;

        public MovementManager MovementManager => _movementManager;
        public PlayerAttackManager AttackManager => _attackManager;
        public AnimationManager AnimationManager => _animationManager;
        public PhysicManager PhysicManager => _physicManager;

        //// invicible
        //[SerializeField] private GameObject visualObject;
        //[SerializeField] private float invincibleDuration = 1f;
        //private bool _isInvincible = false;

        private void Start()
        {
            _animationManager = GetComponent<AnimationManager>();
            _movementManager = GetComponent<MovementManager>();
            _attackManager = GetComponent<PlayerAttackManager>();
            _physicManager = GetComponent<PhysicManager>();
        }

        public void Stop()
        {
            rb.linearVelocity = Vector2.zero;
        }

        private void Update()
        {
            _isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

            if (_isGrounded)
                _jumpCount = 0;

            //animator.SetBool(IsJump, !_isGrounded);
        }

        // this should be in attack manager
        public void Shoot()
        {

            //Vector2 direction = new Vector2(transform.localScale.x, 0); // trái hoặc phải
            //// Nếu không trúng gì → tạo đạn bay tới
            //GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            ////bullet.GetComponent<Bullet>().Shoot(direction, bulletSpeed, bulletDistance, bulletHitLayer);
        }

        //public void Move(Vector2 input)
        //{
        //    rb.linearVelocity = new Vector2(input.x * moveSpeed, rb.linearVelocity.y);

        //    if (Mathf.Abs(input.x) > 0.01f && _isGrounded)
        //    {
        //        _animationManager.StartRunningAnimation();
        //    }

        //    if (Mathf.Abs(input.x) > 0.01f)
        //    {
        //        transform.localScale = new Vector3(Mathf.Sign(input.x), 1, 1);
        //    }
        //}
        
        // this should be in movement manager
        public void Jump()
        {
            if (_jumpCount < _maxJumpCount)
            {
                AudioManager.Instance.PlaySfx(AudioName.Gameplay_ChangeElemet);
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                _jumpCount++;
            }
        }

        //public void StartInvincibility()
        //{
        //    if (_isInvincible) return;
        //    AudioManager.Instance.PlaySfx(AudioName.Gameplay_GotHit);

        //    StartCoroutine(InvincibilityCoroutine());
        //}

        //private IEnumerator InvincibilityCoroutine()
        //{
        //    _isInvincible = true;

        //    float timer = 0f;
        //    bool visible = true;

        //    while (timer < invincibleDuration)
        //    {
        //        visible = !visible;
        //        if (visualObject != null)
        //            visualObject.SetActive(visible);

        //        yield return new WaitForSeconds(0.1f);
        //        timer += 0.1f;
        //    }

        //    if (visualObject != null)
        //        visualObject.SetActive(true);

        //    _isInvincible = false;
        //}

        // this shold be in pick up system and detect controller
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("EndPoint"))
            {
                Destroy(other.gameObject);
                GameController.Instance.ShowEndGame();
            }
            // pick up system

            if (other.CompareTag("Coin"))
            {
                Destroy(other.gameObject);
                GameController.Instance.EarnCoin();
            }
            if (other.CompareTag("Apple"))
            {
                Destroy(other.gameObject);
                GameController.Instance.EarnApple();
            }

            //if (other.CompareTag("Trap") && !_isInvincible)
            //{
            //    GameController.Instance.TakeDamage(1);
            //    StartInvincibility();
            //}

            //if (other.CompareTag("Enemy"))
            //{
            //    if (!_isInvincible)
            //    {
            //        GameController.Instance.TakeDamage(1);
            //        StartInvincibility();
            //    }
            //}
        }

    }
}