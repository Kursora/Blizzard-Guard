using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effects : MonoBehaviour
{
    public float aliveTime;
    public int effectType;
    void Start()
    {
        Destroy(gameObject,aliveTime);
        switch(effectType)
        {
            case 0:
                MainGame.game.growthMul=0.5f;
                break;
            case 1:
                MainGame.game.Chilling();
                Destroy(gameObject);
                break;
            case 2:
                MainGame.game.safeZone(true);
                break;
            case 3:
                MainGame.game.firerateMul*=0.5f;
                break;
            case 4:
                StartCoroutine(MainGame.game.EnableShock());
                break;
            default:
                break;
        }
    }
    void OnDestroy()
    {
        switch(effectType)
        {
            case 0:
                MainGame.game.growthMul=1f;
                break;
            case 3:
                MainGame.game.firerateMul/=0.5f;
                break;
            default:
                break;
        }
    }
}
