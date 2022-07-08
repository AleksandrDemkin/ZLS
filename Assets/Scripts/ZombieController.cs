using UnityEngine;

namespace DefaultNamespace
{
    public class ZombieController : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _speed = 0.5f;

        private Animator _animator;
        private AudioSource _zombieSound;
        private bool _isAttacking;
        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int Walk = Animator.StringToHash("Walk");
        

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _zombieSound = GetComponent<AudioSource>();
        }

        private void Update()
        {
            ZMove();
            IsAttacking();
        }

        private void ZMove()
        {
            ZWalk();
            TargetPosition();
        }

        private void ZWalk()
        {
            _animator.Play(Walk);
        }

        private void TargetPosition()
        {
            transform.position = Vector3.MoveTowards(transform.position,
            _target.position, _speed * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider player)
        {
            if (!player.CompareTag("Player")) return;
            _isAttacking = true;
            _zombieSound.Play();
        }

        private void IsAttacking()
        {
            if (!_isAttacking) return;
            _animator.SetBool(Attack, true);
        }
    }
}