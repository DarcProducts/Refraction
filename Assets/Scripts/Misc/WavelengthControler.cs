using UnityEngine;

public class WavelengthControler : MonoBehaviour
{
    public static WavelengthControler S { get; private set; }
    [Header("Colors")]
    [SerializeField] ParticleSystem.MinMaxGradient ultraviolet;
    [SerializeField] ParticleSystem.MinMaxGradient infrared;

    void Awake() => S = this;

    public void SetWavelengthColor(ParticleSystem particleSystem, WaveType type)
    {
        ParticleSystem.ColorOverLifetimeModule colorMod = particleSystem.colorOverLifetime;
        if (type == WaveType.Ultraviolet)
            colorMod.color = ultraviolet;
        else
            colorMod.color = infrared;
    }


    public ParticleSystem.MinMaxGradient GetUltravioletColor() => ultraviolet;
    public ParticleSystem.MinMaxGradient GetInfraredColor() => infrared;
}
