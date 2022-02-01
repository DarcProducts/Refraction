using UnityEngine;

public enum PlayerWaveType
{
    None,
    Ultraviolet,
    Infrared
}

public class ShiftState : MonoBehaviour
{
    public static ShiftState S { get; private set; }
    [SerializeField] KeyCode shiftKey = KeyCode.Mouse1;
    [SerializeField] EnemySpawner spawner;
    [SerializeField] ObjectPool projectilePool;
    [SerializeField] PlayerWaveType playerShift = PlayerWaveType.Infrared;
    [SerializeField] ParticleSystem backgroundParticle0, backgroundParticle1;
    [SerializeField] AudioFX shiftFX;

    void Awake() => S = this;

    private void Start() => SwitchBackgroundColors();

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(shiftKey))
        {
            if (playerShift == PlayerWaveType.Infrared)
            {
                WavelengthControler.CurrentWaveType = WaveType.Ultraviolet;
                playerShift = PlayerWaveType.Ultraviolet;
            }
            else if (playerShift == PlayerWaveType.Ultraviolet)
            {
                WavelengthControler.CurrentWaveType = WaveType.Infrared;
                playerShift = PlayerWaveType.Infrared;
            }
            
            SwitchBackgroundColors();
            
            if (shiftFX != null)
                shiftFX.PlayFX();

            foreach (GameObject obj in spawner._currentEnemies)
                if (obj.TryGetComponent<Enemy>(out Enemy e))
                    e.SwitchVisibility();

            foreach (GameObject obj in projectilePool.pooledObjects)
                if (obj.TryGetComponent<Projectile>(out Projectile e))
                    e.SwitchVisibility();
        }
    }

    public void ShiftCurrentState()
    {
        if (playerShift == PlayerWaveType.Infrared)
        {
            WavelengthControler.CurrentWaveType = WaveType.Ultraviolet;
            playerShift = PlayerWaveType.Ultraviolet;
        }
        else if (playerShift == PlayerWaveType.Ultraviolet)
        {
            WavelengthControler.CurrentWaveType = WaveType.Infrared;
            playerShift = PlayerWaveType.Infrared;
        }

        SwitchBackgroundColors();

        if (shiftFX != null)
            shiftFX.PlayFX();

        foreach (GameObject obj in spawner._currentEnemies)
            if (obj.TryGetComponent<Enemy>(out Enemy e))
                e.SwitchVisibility();

        foreach (GameObject obj in projectilePool.pooledObjects)
            if (obj.TryGetComponent<Projectile>(out Projectile e))
                e.SwitchVisibility();
    }

    public void ShiftToState(WaveType type)
    {
        WaveType w = type;
        if (w == WaveType.Ultraviolet)
        {
            WavelengthControler.CurrentWaveType = WaveType.Ultraviolet;
            playerShift = PlayerWaveType.Ultraviolet;
        }
        else if (w == WaveType.Infrared)
        {
            WavelengthControler.CurrentWaveType = WaveType.Infrared;
            playerShift = PlayerWaveType.Infrared;

        }

        SwitchBackgroundColors();

        if (shiftFX != null)
            shiftFX.PlayFX();

        foreach (GameObject obj in spawner._currentEnemies)
            if (obj.TryGetComponent<Enemy>(out Enemy e))
                e.SwitchVisibility();

        foreach (GameObject obj in projectilePool.pooledObjects)
            if (obj.TryGetComponent<Projectile>(out Projectile e))
                e.SwitchVisibility();
    }

    void SwitchBackgroundColors()
    {
        if (playerShift == PlayerWaveType.Infrared)
        {
            ParticleSystem.MainModule p0 = backgroundParticle0.main;
            ParticleSystem.MainModule p1 = backgroundParticle1.main;
            p0.startColor = WavelengthControler.S.GetInfraredColor();
            p1.startColor = WavelengthControler.S.GetInfraredColor();
        }
        else if (playerShift == PlayerWaveType.Ultraviolet)
        {
            ParticleSystem.MainModule p0 = backgroundParticle0.main;
            ParticleSystem.MainModule p1 = backgroundParticle1.main;
            p0.startColor = WavelengthControler.S.GetUltravioletColor();
            p1.startColor = WavelengthControler.S.GetUltravioletColor();
        }
    }

    public WaveType GetCurrentPlayerWaveType()
    {
        switch (playerShift)
        {
            case PlayerWaveType.Ultraviolet:
                return WaveType.Ultraviolet;
            case PlayerWaveType.Infrared:
                return WaveType.Infrared;
        }
        return WaveType.None;
    }
}
