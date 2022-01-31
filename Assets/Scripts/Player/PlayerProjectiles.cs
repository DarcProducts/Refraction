using UnityEngine;

public class PlayerProjectiles : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Camera mainCamera;
    [SerializeField] ParticleSystem.MinMaxGradient projectileColor = Color.white;
    [SerializeField] ObjectPool projectilePool;
    [SerializeField] float projectileSpeed;
    [SerializeField] float projectileDamage;
    [SerializeField] PauseMenu pauseMenu;
    [SerializeField] GameEvent OnFiredProjectile;
    Projectile _currentProjectile;

    void Awake() => player = GameObject.FindWithTag("Player").transform;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            ShootProjectile();    
    }

    void ShootProjectile()
    {
        if (!player.gameObject.activeSelf) return;
        if (pauseMenu.IsPaused) return;
        OnFiredProjectile?.Invoke(gameObject);
        _currentProjectile = null;
        GameObject pObj = projectilePool.GetObject();
        _currentProjectile = pObj.GetComponent<Projectile>();

        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -mainCamera.transform.position.z;
        Vector3 mousePos3D = mainCamera.ScreenToWorldPoint(mousePos2D);
        if (_currentProjectile != null)
        {
            _currentProjectile.transform.position = player.position;
            _currentProjectile.projectileColor = projectileColor;
            _currentProjectile.projectileSpeed = projectileSpeed;
            _currentProjectile.projectileDamage = projectileDamage;
            _currentProjectile.currentDirection = (mousePos3D - player.position).normalized;
            pObj.SetActive(true);
            _currentProjectile.FiredByPlayer = true;
        }
    }
}