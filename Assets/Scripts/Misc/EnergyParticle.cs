using UnityEngine;

public class EnergyParticle : MonoBehaviour
{
    [SerializeField] float collectSpeed;
    [SerializeField] int collectAmount;
    [SerializeField] GameEvent OnCollected;
    Transform _player;

    void Awake() => _player = GameObject.FindWithTag("Player").transform;

    void FixedUpdate()
    {
        if (_player == null) return;
        transform.position = Vector3.MoveTowards(transform.position, _player.position, collectSpeed * Time.fixedDeltaTime);
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
