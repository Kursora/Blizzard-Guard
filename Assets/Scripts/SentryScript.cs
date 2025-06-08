using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class SentryScript : MonoBehaviour
{
    public float timeToDie;
    public TMP_Text a;
    void Update()
    {
        if(timeToDie>0f)
        {
            a.text=""+timeToDie.ToString("0");
            timeToDie-=Time.deltaTime;
        }
        else Destroy(gameObject);
    }
}
