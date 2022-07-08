using System;
using System.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    public class CoinCollect : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private GameObject _coinPrefab;
        [SerializeField] private Camera _camera;

        private AudioSource _collectSound;
        private float _speed;
        private float _time;
        private int _distance;

        private void Start()
        {
            _collectSound = GetComponent<AudioSource>();
            _speed = 1f;
            _distance = 1;
            
            if (_camera == null)
            {
                _camera = Camera.main;
            }
        }

        public void StartCoinMove(Vector3 initial, Action onComplete)
        {
            var position = _camera.transform.position;
            var targetPos = _camera.ScreenToWorldPoint(new Vector3(_target.position.x, _target.position.y,
                position.z * _distance));
            var coin = Instantiate(_coinPrefab, transform);

            StartCoroutine(MoveCoin(coin.transform, initial, targetPos, onComplete));
        }

        private IEnumerator MoveCoin(Transform obj, Vector3 startPosition, Vector3 endPosition, Action onComplete)
        {
            _time = 0;
            
            while (_time < 1)
            {
                _time += _speed * Time.deltaTime;
                obj.position = Vector3.Lerp(startPosition, endPosition, _time);

                yield return new WaitForEndOfFrame();
            }
            
            onComplete.Invoke();
            _collectSound.Play();
            Destroy(obj.gameObject);
        }
    }
}