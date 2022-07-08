using System.Collections.Generic;
using UnityEngine;

public class ObstaclesCreate : MonoBehaviour
{
    [SerializeField] private Transform[] _obstacle;
    [SerializeField] private float _spawnStep;
    [SerializeField] private float _spawnDistanceToPlayer;
    [SerializeField] private Vector2 _segmentWidth;
    [SerializeField] private Transform _player;

    private Vector3 _lastPosition;
    private List<Transform> _spawnedObstacles = new List<Transform>();
    private Transform _newObstacle;

    public List<Transform> SpawnedObstacles
    {
        get
        { _spawnedObstacles.RemoveAll(TransformIsNull);
            return _spawnedObstacles;
        }
    }

    private bool TransformIsNull(Transform obj)
    {
        return obj == null;
    }

    private void Start()
    {
        _player = transform;
        _lastPosition = _player.position;
    }

    private void Update()
    {
        if (!(_player.position.z > _lastPosition.z + _spawnStep)) return;
        _lastPosition.z += _spawnStep;
        _newObstacle = _obstacle[Random.Range(0, _obstacle.Length)];
        _spawnedObstacles.Add(Instantiate(_newObstacle, 
            new Vector3(Random.Range(_segmentWidth.x, _segmentWidth.y), 
                0, _lastPosition.z+ _spawnDistanceToPlayer), 
            Quaternion.identity));
    }
}
