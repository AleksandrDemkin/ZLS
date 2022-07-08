using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class Flashlight : MonoBehaviour
    {
        [SerializeField] private Rigidbody _playerRigidbody;
        [SerializeField] private KeyCode _flashlightKey;
        
        private Light _flashlight;
        private Vector3 _lightOffset;
        
        private void Start()
        {
            _flashlight = GetComponent<Light>();
            FlashlightOffset();
        }

        private void FixedUpdate()
        {
            FlashlightPosition();
            
            if (Input.GetKeyDown(_flashlightKey))
            {
                LightIsOn();
            }
        }

        private void FlashlightPosition()
        {
            _flashlight.transform.position = 
                _playerRigidbody.transform.position + _lightOffset;
        }
        
        private void FlashlightOffset()
        {
            _lightOffset = 
                _flashlight.transform.position - _playerRigidbody.transform.position;
        }
        
        private void LightIsOn()
        {
            //_light.enabled = _light.enabled == false;
            if (_flashlight.enabled == false)
            {
                _flashlight.enabled = true;
            }
            else
            {
                _flashlight.enabled = false;
            }
        }
    }
}