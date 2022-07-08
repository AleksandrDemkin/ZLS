using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class LevelGenerator : MonoBehaviour
    {
        [SerializeField] private List<Transform> _grounds;
        [SerializeField] private float _minimumDistance;
        [SerializeField] private Transform _player;

        private Transform _lastObject;
        private Transform _firstObject;
        private float _distance;
        private Vector3 _offset;

        private void Update()
        {
            _lastObject = _grounds[_grounds.Count - 1];
            _distance = Vector3.Distance(_lastObject.position, _player.position);

            if (!(_distance < _minimumDistance)) return;
            _firstObject = _grounds[0];
            _firstObject.position = _lastObject.position;
            _offset = _lastObject.GetComponent<Collider>().bounds.extents +
                      _firstObject.GetComponent<Collider>().bounds.extents;
            _firstObject.position += Vector3.forward * _offset.z;
            _grounds.Remove(_firstObject);
            _grounds.Add(_firstObject);
        }
    }
}