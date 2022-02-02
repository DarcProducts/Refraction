using UnityEngine;

public class WavelengthControler : MonoBehaviour
{
    public static WavelengthControler S { get; private set; }
    [Header("Colors")]
    public static WaveType CurrentWaveType = WaveType.Infrared;
    [SerializeField] Light directionalLight;
    [SerializeField] ParticleSystem.MinMaxGradient ultraviolet;
    [SerializeField] ParticleSystem.MinMaxGradient infrared;
    ParticleSystem.ColorOverLifetimeModule colorMod;

    void Awake() => S = this;

    public void SetWavelengthColor(ParticleSystem particleSystem, WaveType type)
    {
        colorMod = particleSystem.colorOverLifetime;
        if (type == WaveType.Ultraviolet)
        {
            colorMod.color = ultraviolet;
            CurrentWaveType = WaveType.Ultraviolet;
        }
        else
        {
            colorMod.color = infrared;
            CurrentWaveType = WaveType.Infrared;
        }
    }

    public void SetLightColor()
    {
        if (CurrentWaveType == WaveType.Ultraviolet)
            directionalLight.color = ultraviolet.color;
        else if (CurrentWaveType == WaveType.Infrared)
            directionalLight.color = infrared.color;
    }

    public ParticleSystem.MinMaxGradient GetUltravioletColor() => ultraviolet;
    public ParticleSystem.MinMaxGradient GetInfraredColor() => infrared;
}

