using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class Projectile : MonoBehaviour
{
    public float projectileSpeed { get; set; }
    public float projectileDamage { get; set; }
    public Vector3 currentDirection { private get; set; }
    public ParticleSystem.MinMaxGradient projectileColor { private get; set; }
    public bool FiredByPlayer { get; set; }
    public float maxDistance = 32;
    [SerializeField] float topOffset, bottomOffset;
    ParticleSystem.MainModule particles;
    ScreenBounds screenBounds;

    void OnEnable()
    {
        FiredByPlayer = false;
        ParticleSystem.MainModule particles = GetComponent<ParticleSystem>().main;
        particles.startColor = projectileColor;

        if (screenBounds == null)
            screenBounds = FindObjectOfType<ScreenBounds>();
    }

    void LateUpdate()
    {
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
        transform.Translate(projectileSpeed * Time.fixedDeltaTime * currentDirection);
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
    }
}
