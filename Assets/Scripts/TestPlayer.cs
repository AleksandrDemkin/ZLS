using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

public class TestPlayer : MonoBehaviour
{
    [SerializeField] private Text _coinsCount;
    [SerializeField] private CoinCollect _coinCollect;
    [SerializeField] private int _coins;
    
    private float _speed;
    private float _jumpForce;
    private Rigidbody _playerRigidbody;
    private Transform _playerTransform;

    private void Start()
    {
        _speed = 10f;
        _jumpForce = 50f;
        _coins = 0;
        _playerRigidbody = GetComponent<Rigidbody>();
        _playerTransform = GetComponent<Transform>();
        _coinsCount.text = $"Coins: {_coins.ToString()}";
    }

    private void FixedUpdate()
    {
        var x = Input.GetAxisRaw("Horizontal") * Time.deltaTime * _speed;
        var z = Input.GetAxisRaw("Vertical") * Time.deltaTime * _speed;
        
        _playerTransform.Translate(new Vector3(x,0, z));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _playerRigidbody.AddForce(Vector3.up * _jumpForce);
        }
    }

    private void OnTriggerEnter(Collider coin)
    {
        if (!coin.CompareTag("Coin")) return;
        _coinCollect.StartCoinMove(coin.transform.position, () => 
        { _coins++;
            _coinsCount.text = $"Coins: {_coins.ToString()}";
            
        });
        //coin.gameObject.SetActive(false);
        Destroy(coin.gameObject);
    }
}
