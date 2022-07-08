using UnityEngine;


namespace DefaultNamespace
{
    public class CoinController: MonoBehaviour
    {
        private Animator _animator;
        private static readonly int _coinAnimation =
            Animator.StringToHash("CoinAnimation");

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }
        
        private void Update()
        {
            CoinRotation();
        }

        private void CoinRotation()
        {
            _animator.Play(_coinAnimation);
        }
    }
}