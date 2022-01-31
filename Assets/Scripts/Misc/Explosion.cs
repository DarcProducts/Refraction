using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] ParticleSystem particles;

    public void ChangeParticleColor(ParticleSystem.MinMaxGradient color)
    {
        ParticleSystem.MainModule mainParticles = particles.main;
        mainParticles.startColor = color;
    }
}
