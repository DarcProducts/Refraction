using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Utils
{
    public static bool IsInLayerMask(GameObject obj, LayerMask layer) => ((layer.value & (1 << obj.layer)) > 0);

    public static void PlayAtSourceVolumePitchRange(this AudioSource source, AudioClip clip, float minVol = 1, float maxVol = 1, float minPitch = 1, float maxPitch = 1)
    {
        if (source == null || clip == null) return;
        var v = Random.Range(Mathf.Min(minVol, maxVol), Mathf.Max(minVol, maxVol));
        var p = Random.Range(Mathf.Min(minPitch, maxPitch), Mathf.Max(minPitch, maxPitch));
        source.volume = Mathf.Clamp01(v);
        source.pitch = Mathf.Clamp(p, -3, 3);
        source.PlayOneShot(clip);
    }

    public static float LerpP(float v0, float v1, float t) => (1 - t) * v0 + t * v1;

    public static float LerpI(float v0, float v1, float t) => v0 + t * (v1 - v0);


    public static float RemapClamped(this float aValue, float aIn1, float aIn2, float aOut1, float aOut2)
    {
        var t = (aValue - aIn1) / (aIn2 - aIn1);
        if (t > 1f)
            return aOut2;
        if (t < 0f)
            return aOut1;
        return aOut1 + (aOut2 - aOut1) * t;
    }

    public static List<T> Find<T>()
    {
        var l = new List<T>();
        var t = GameObject.FindObjectsOfType<MonoBehaviour>().OfType<T>();
        foreach (T i in t)
            l.Add(i);
        return l;
    }

    public static void DrawCircle(this LineRenderer line, float radius, float lineWidth)
    {
        if (line == null) return;
        var segments = 360;
        var pointCount = segments;
        var points = new Vector3[pointCount];

        line.useWorldSpace = false;
        line.startWidth = lineWidth;
        line.endWidth = lineWidth;
        line.positionCount = segments;

        for (int i = 0; i < pointCount; i++)
        {
            var rad = Mathf.Deg2Rad * (i * 360f / segments);
            points[i] = new Vector3(Mathf.Sin(rad) * radius, 0, Mathf.Cos(rad) * radius);
        }

        line.loop = true;
        line.SetPositions(points);
    }
}