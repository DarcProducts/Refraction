using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class Projectile : MonoBehaviour, IWavelength
{
    [SerializeField] SpeedFixer speedFixer;
    public float projectileSpeed { get; set; }
    public float projectileDamage { get; set; }
    public Vector3 currentDirection { private get; set; }
    public ParticleSystem.MinMaxGradient projectileColor { private get; set; }
    public bool FiredByPlayer { get; set; }
    [SerializeField] float maxDistance = 32;
    [SerializeField] float topOffset, bottomOffset;
    ParticleSystem parts;
    WaveType _currentType = WaveType.None;
    ScreenBounds screenBounds;

    private void Start()
    {
        if (parts == null)
            parts = GetComponent<ParticleSystem>();
        if (speedFixer == null)
            speedFixer = FindObjectOfType<SpeedFixer>();
        if (screenBounds == null)
            screenBounds = FindObjectOfType<ScreenBounds>();
    }

    void OnEnable()
    {
        FiredByPlayer = false;

        if (parts != null)
        {
            ParticleSystem.MainModule particles = parts.main;
            particles.startColor = projectileColor;
        }
    }

    void LateUpdate()
    {
        if (speedFixer == null) return;
        if (transform.position.x < screenBounds.ScreenWidth() * .5f)
            gameObject.SetActive(false);
        if (transform.position.x > Mathf.Abs(screenBounds.ScreenWidth() * .5f))
            gameObject.SetActive(false);
        if (transform.position.y < (screenBounds.ScreenHeight() * .5f) + bottomOffset)
            gameObject.SetActive(false);
        if (transform.position.y > Mathf.Abs(screenBounds.ScreenHeight() * .5f) + topOffset)
            gameObject.SetActive(false);
        if (transform.position.z < -maxDistance | transform.position.z > maxDistance)
            gameObject.SetActive(false);
        transform.Translate(projectileSpeed * speedFixer.speedMutliplier * Time.fixedDeltaTime * currentDirection.normalized);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !FiredByPlayer)
        {
            if (other.TryGetComponent<IDamagable>(out IDamagable damagable))
                damagable.Damage(projectileDamage);
            gameObject.SetActive(false);
        }
        if (other.CompareTag("Enemy") && FiredByPlayer)
        {
            if (other.TryGetComponent<IDamagable>(out IDamagable damagable))
                damagable.Damage(projectileDamage);
            gameObject.SetActive(false);
        }
        if (other.CompareTag("Projectile") && FiredByPlayer)
            gameObject.SetActive(false);
    }

    public WaveType GetWaveType() => _currentType;

    public void SetWaveType(WaveType newType) => _currentType = newType;

    public void SwitchVisibility()
    {
        if (transform.position.z != 0) return;
        if (parts != null)
        {
            if (WavelengthControler.CurrentWaveType == _currentType)
                parts.Play();
            else
                parts.Stop();
        }
    }
}
