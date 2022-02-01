using UnityEngine;

public class EnergyParticle : MonoBehaviour
{
    [SerializeField] SpeedFixer speedFixer;
    [SerializeField] float collectSpeed;
    [SerializeField] int collectAmount;
    [SerializeField] GameEvent OnCollected;
    Transform _player;

    void Awake() => _player = GameObject.FindWithTag("Player").transform;

    private void Start()
    {
        if (speedFixer == null)
            speedFixer = FindObjectOfType<SpeedFixer>();
    }

    void FixedUpdate()
    {
        if (_player == null) return;
        transform.position = Vector3.MoveTowards(transform.position, _player.position, collectSpeed * speedFixer.speedMutliplier * Time.fixedDeltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ScoreTracker.S.UpdateScore(collectAmount);
            OnCollected?.Invoke(gameObject);
            gameObject.SetActive(false);
        }
    }
}
