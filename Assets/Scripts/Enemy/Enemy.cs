using UnityEngine;

namespace darcproducts
{
    public class Enemy : MonoBehaviour, IDamagable
    {
        public float fireRate;
        [SerializeField] float moveSpeed;
        [SerializeField] float maxHealth;
        [SerializeField] float projectileDamage;
        [SerializeField] float collideDamage;
        [SerializeField] float projectileSpeed;
        [SerializeField] float maxFireDistance;
        [SerializeField] public WaveType type;
        [SerializeField] GameEvent EnemyHasDied;
        [SerializeField] GameEvent OnEnemyHit;
        [SerializeField] ParticleSystem.MinMaxGradient startColor;
        SpriteRenderer sRend;
        ParticleSystem.MinMaxGradient _projectileColor;
        EnemySpawner enemySpawner;
        bool IsDamagable;
        float _currentRate;
        ObjectPool explosionPool;
        ObjectPool projectilePool;
        Transform _playerTransform;
        float _currentHealth;
        Vector3 _currentMoveLocation;

        void OnEnable()
        {
            if (sRend == null)
                sRend = GetComponent<SpriteRenderer>();
            if (enemySpawner == null)
                enemySpawner = FindObjectOfType<EnemySpawner>();
            if (projectilePool == null)
                projectilePool = GameObject.FindGameObjectWithTag("ProjectilePool").GetComponent<ObjectPool>();
            if (explosionPool == null)
                explosionPool = GameObject.FindGameObjectWithTag("EnemyExplosionPool").GetComponent<ObjectPool>();

            IsDamagable = false;
            ParticleSystem parts = GetComponent<ParticleSystem>();
            ParticleSystem.MainModule pCol = parts.main;
            pCol.startColor = startColor;

            if (sRend != null)
                sRend.color = new Color(1, 1, 1, startColor.color.a);

            _currentHealth = maxHealth;

            if (_playerTransform == null)
                _playerTransform = GameObject.FindWithTag("Player").transform;

            if (Random.value > .5f)
                type = WaveType.Ultraviolet;
            else
                type = WaveType.Infrared;

            SetRandomLocation();
            WavelengthControler.S.SetWavelengthColor(GetComponent<ParticleSystem>(), type);
        }

        void FixedUpdate()
        {
            if (_playerTransform == null || projectilePool == null || sRend == null) return;
            _currentRate = _currentRate < 0 ? 0 : _currentRate -= Time.fixedDeltaTime;
            if (Vector3.Distance(transform.position, _playerTransform.position) < maxFireDistance && _currentRate.Equals(0) && transform.position.z == _playerTransform.position.z)
            {
                if (!IsDamagable)
                {
                    IsDamagable = true;
                    ParticleSystem parts = GetComponent<ParticleSystem>();
                    ParticleSystem.MainModule pCol = parts.main;
                    sRend.color = Color.white;
                    if (type == WaveType.Ultraviolet)
                        pCol.startColor = WavelengthControler.S.GetUltravioletColor();
                    else if (type == WaveType.Infrared)
                        pCol.startColor = WavelengthControler.S.GetInfraredColor();
                }
                GameObject pObj = projectilePool.GetObject();
                if (pObj.TryGetComponent<Projectile>(out Projectile p))
                {
                    _currentRate = fireRate;
                    p.transform.position = transform.position;
                    SetProjectileColor(p);
                    p.currentDirection = (_playerTransform.position - transform.position).normalized * projectileSpeed;
                    p.projectileDamage = projectileDamage;
                    pObj.SetActive(true);
                    p.FiredByPlayer = false;
                    p.projectileSpeed = projectileSpeed;
                }
            }
            if (transform.position != _currentMoveLocation)
                transform.position = Vector3.MoveTowards(transform.position, _currentMoveLocation, moveSpeed * Time.fixedDeltaTime);
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
            var newLoc = Random.insideUnitSphere;
            _currentMoveLocation = _playerTransform.position + new Vector3(newLoc.x, newLoc.y, _playerTransform.position.z) * 5;
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
    }
}