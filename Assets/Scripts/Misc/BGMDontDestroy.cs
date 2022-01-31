using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMDontDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }
    private static BGMDontDestroy instance = null;
    public static BGMDontDestroy Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }
}