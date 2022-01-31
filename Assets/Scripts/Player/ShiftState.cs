using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using darcproducts;

public enum PlayerWaveType
{
    None,
    Ultraviolet,
    Infrared
}

public class ShiftState : MonoBehaviour
{
    [SerializeField] KeyCode shiftKey = KeyCode.Mouse1;
    [SerializeField] EnemySpawner spawner;
    [SerializeField] PlayerWaveType playerShift = PlayerWaveType.Infrared;
    [SerializeField] ParticleSystem backgroundParticle0, backgroundParticle1;
    [SerializeField] AudioFX shiftFX;

    private void Start() => SwitchBackgroundColors();

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(shiftKey))
        {
            if(playerShift == PlayerWaveType.Infrared)
                playerShift = PlayerWaveType.Ultraviolet;
            else if (playerShift == PlayerWaveType.Ultraviolet)
                playerShift = PlayerWaveType.Infrared;
            SwitchBackgroundColors();
            if (shiftFX != null)
                shiftFX.PlayFX();
        }

        foreach (GameObject obj in spawner._currentEnemies){
            if(obj.GetComponent<Enemy>().type == WaveType.Ultraviolet && playerShift == PlayerWaveType.Infrared)
            {
                obj.GetComponent<ParticleSystem>().Stop();
                obj.GetComponent<SpriteRenderer>().enabled = false;
            }
            else if (obj.GetComponent<Enemy>().type == WaveType.Ultraviolet && playerShift == PlayerWaveType.Ultraviolet)
            {
                obj.GetComponent<ParticleSystem>().Play();
                obj.GetComponent<SpriteRenderer>().enabled = true;
            }

            if (obj.GetComponent<Enemy>().type == WaveType.Infrared && playerShift == PlayerWaveType.Ultraviolet)
            {
                obj.GetComponent<ParticleSystem>().Stop();
                obj.GetComponent<SpriteRenderer>().enabled = false;
            }
            else if (obj.GetComponent<Enemy>().type == WaveType.Infrared && playerShift == PlayerWaveType.Infrared)
            {
                obj.GetComponent<ParticleSystem>().Play();
                obj.GetComponent<SpriteRenderer>().enabled = true;
            }
        }
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
}
