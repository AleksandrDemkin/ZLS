using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Light _light;
        
        [SerializeField] private Text _coinsCount;
        [SerializeField] private CoinCollect _coinCollect;
        [SerializeField] private int _coins;
        [SerializeField] private bool _movingAuto;
        
        private Rigidbody _playerRigidbody;
        private Vector3 _direction;
        private Vector3 _offset;
        private Vector3 _lightOffset;

        private const float JumpForce = 15f;
        private const float Speed = 10f;
        private const float SideSpeed = 4f;
        
        private void Start()
        {
            _playerRigidbody = GetComponent<Rigidbody>();
            
            CameraOffset();
            
            _coins = 0;
            _coinsCount.text = $"Coins: {_coins.ToString()}";
            _movingAuto = true;
        }

        private void FixedUpdate()
        {
            CameraPosition();
            
            if (!_movingAuto)
            {
                MovingManual();
                //Debug.Log("Manual moving to forward");
            }
            else
            {
                MovingAuto();
                //Debug.Log("Automatic moving to forward");
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }
        
        private void CameraPosition()
        {
            _camera.transform.position = 
                _playerRigidbody.transform.position + _offset;
        }

        private void CameraOffset()
        {
            _offset = 
                _camera.transform.position - _playerRigidbody.transform.position;
        }
        
        private void MovingManual()
        {
            _direction = _playerRigidbody.velocity;
            _direction.x = Input.GetAxisRaw("Horizontal") * SideSpeed;
            _direction.z = Input.GetAxisRaw("Vertical") * Speed;
            _playerRigidbody.velocity = _direction;
        }
        
        private void MovingAuto()
        {
            _direction = _playerRigidbody.velocity;
            _direction.x = Input.GetAxisRaw("Horizontal") * SideSpeed;
            _direction.z = Speed;
            _playerRigidbody.velocity = _direction;
        }

        private void Jump()
        {
            _playerRigidbody.AddForce(Vector3.up * JumpForce);
        }
        
        private void OnTriggerEnter(Collider coin)
        {
            if (!coin.GetComponent(typeof(CoinController))) return;
            _coinCollect.StartCoinMove(coin.transform.position, () => 
            {
                _coins++;
                _coinsCount.text = $"Coins: {_coins.ToString()}";
            });
            coin.gameObject.SetActive(false);
            //Destroy(coin.gameObject);
        }
    }
}