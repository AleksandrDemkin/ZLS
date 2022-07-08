using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class ObstaclesDelete : MonoBehaviour
    {
        [SerializeField] private Transform _player;
        [SerializeField] private ObstaclesCreate _spawner;
        [SerializeField] private float _destroyDistanceZ;
        
        private List<Transform> _obstacles;

        private void Update()
        {
            _obstacles = new List<Transform>(_spawner.SpawnedObstacles);

            for (int i = 0; i < _obstacles.Count; i++)
            {
                if (_player.position.z > _obstacles[i].position.z + _destroyDistanceZ)
                {
                    Destroy(_obstacles[i].gameObject);
                }
            }
        }
    }
}