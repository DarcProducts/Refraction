using UnityEngine;

[RequireComponent(typeof(ParticleSystem), typeof(SpriteRenderer))]
public class Enemy : MonoBehaviour, IDamagable, IWavelength
{
    [SerializeField] SpeedFixer speedFixer;
    public float fireRate;
    [SerializeField] float moveRandomness;
    [SerializeField] float moveSpeed;
    [SerializeField] float maxHealth;
    [SerializeField] float projectileDamage;
    [SerializeField] float collideDamage;
    [SerializeField] float projectileSpeed;
    [SerializeField] float maxFireDistance;
    [SerializeField] float aimOffset;
    public WaveType type { get; set; }
    [SerializeField] GameEvent EnemyHasDied;
    [SerializeField] GameEvent OnEnemyHit;
    [SerializeField] ParticleSystem.MinMaxGradient startColor;
    SpriteRenderer sRend;
    ParticleSystem.MinMaxGradient _projectileColor;
    ParticleSystem parts;
    EnemySpawner enemySpawner;
    bool IsDamagable;
    float _currentRate;
    ObjectPool explosionPool;
    ObjectPool projectilePool;
    Transform _playerTransform;
    float _currentHealth;
    Vector3 _currentMoveLocation;
    bool IsVisible;

    void OnEnable()
    {
        _currentHealth = maxHealth;
        if (parts == null)
            parts = GetComponent<ParticleSystem>();
        if (sRend == null)
            sRend = GetComponent<SpriteRenderer>();

        if (Random.value > .5f)
            type = WaveType.Ultraviolet;
        else
            type = WaveType.Infrared;

        WavelengthControler.S.SetWavelengthColor(parts, type);

        ParticleSystem.MainModule particles = parts.main;
        particles.startColor = startColor.color;

        if (sRend != null)
            sRend.color = new Color(1, 1, 1, startColor.color.a);

        SwitchVisibility();

        if (speedFixer == null)
            speedFixer = FindObjectOfType<SpeedFixer>();
        if (enemySpawner == null)
            enemySpawner = FindObjectOfType<EnemySpawner>();
        if (projectilePool == null)
            projectilePool = GameObject.FindGameObjectWithTag("ProjectilePool").GetComponent<ObjectPool>();
        if (explosionPool == null)
            explosionPool = GameObject.FindGameObjectWithTag("EnemyExplosionPool").GetComponent<ObjectPool>();

        IsDamagable = false;

        _currentHealth = maxHealth;

        if (_playerTransform == null)
            _playerTransform = GameObject.FindWithTag("Player").transform;

        SetRandomLocation();
    }

    void FixedUpdate()
    {
        if (speedFixer == null) return;
        if (_playerTransform == null || projectilePool == null || sRend == null) return;
        _currentRate = _currentRate < 0 ? 0 : _currentRate -= Time.fixedDeltaTime;
        if (Vector3.Distance(transform.position, _playerTransform.position) < maxFireDistance && _currentRate.Equals(0) && transform.position.z == _playerTransform.position.z)
        {
            if (!IsDamagable)
            {
                ParticleSystem.MainModule particles = parts.main;
                IsDamagable = true;
                sRend.color = Color.white;
                if (type == WaveType.Ultraviolet)
                    particles.startColor = WavelengthControler.S.GetUltravioletColor();
                else if (type == WaveType.Infrared)
                    particles.startColor = WavelengthControler.S.GetInfraredColor();
            }
            GameObject pObj = projectilePool.GetObject();
            if (pObj.TryGetComponent<Projectile>(out Projectile p))
            {
                _currentRate = fireRate;
                p.transform.position = transform.position;
                SetProjectileColor(p);
                var dirRand = Random.insideUnitCircle * aimOffset;
                p.currentDirection = (_playerTransform.position - transform.position).normalized * projectileSpeed + new Vector3(dirRand.x, dirRand.y, 0).normalized;
                p.projectileDamage = projectileDamage;
                p.SetWaveType(type);
                pObj.SetActive(true);
                p.FiredByPlayer = false;
                p.projectileSpeed = projectileSpeed;
            }
        }
        if (transform.position != _currentMoveLocation)
        {
            _currentMoveLocation = new Vector3(_currentMoveLocation.x, _currentMoveLocation.y, 0);
            transform.position = Vector3.MoveTowards(transform.position, _currentMoveLocation, moveSpeed * speedFixer.speedMutliplier * Time.fixedDeltaTime);
        }
        else
            SetRandomLocation();
    }

    void ExplodeObject()
    {
        if (explosionPool == null) return;
        GameObject o = explosionPool.GetObject();
        if (o != null)
        {
            GameObject newObject = Instantiate(o, transform.position, Quaternion.Euler(0, 0, Random.Range(-180f, 180f)), explosionPool.transform);
            newObject.transform.position = transform.position;
            if (newObject.transform.GetChild(0).TryGetComponent<Explosion>(out Explosion e))
                e.ChangeParticleColor(_projectileColor);
            newObject.SetActive(true);
        }
    }

    void SetProjectileColor(Projectile projectile)
    {
        if (type == WaveType.Ultraviolet)
            _projectileColor = WavelengthControler.S.GetUltravioletColor();
        if (type == WaveType.Infrared)
            _projectileColor = WavelengthControler.S.GetInfraredColor();
        projectile.projectileColor = _projectileColor;
    }

    void SetRandomLocation()
    {
        if (_playerTransform == null) return;
        var newLoc = Random.insideUnitSphere * moveRandomness;
        _currentMoveLocation = _playerTransform.position + new Vector3(newLoc.x, newLoc.y, _playerTransform.position.z);
    }

    public void Damage(float amount)
    {
        _currentHealth -= amount;
        OnEnemyHit?.Invoke(gameObject);
        if (_currentHealth < 0)
        {
            _currentHealth = maxHealth;
            ExplodeObject();
            EnemyHasDied?.Invoke(gameObject);
            enemySpawner.RemoveEnemyFromList(gameObject);
            gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            if (other.TryGetComponent<IDamagable>(out IDamagable damager))
                damager.Damage(collideDamage);
    }

    public void SwitchVisibility()
    {
        if (parts != null)
        {
            if (type == WavelengthControler.CurrentWaveType)
                IsVisible = true;
            else
                IsVisible = false;

            if (IsVisible)
            {
                parts.Play();
                sRend.enabled = true;
            }
            else
            {
                parts.Stop();
                sRend.enabled = false;
            }
        }
    }
}