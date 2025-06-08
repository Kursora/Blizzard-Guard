using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawn : MonoBehaviour
{
    public float minX,maxX,minY,maxY;
    public float minTimeSpawn,maxTimeSpawn,timeCount,atX,atY;
    public GameObject[] distributor;
    public int rn;
    void Start()
    {
        timeCount=Random.Range(minTimeSpawn,maxTimeSpawn);
    }
    void Update()
    {
        if(MainGame.game.isPlaying)
        {
            if(timeCount>0f)
            {
                timeCount-=Time.deltaTime;
            }
            else
            {
                rn=Random.Range(0,distributor.Length);
                Instantiate(distributor[rn],RandomLocation(),Quaternion.identity);
                timeCount=Random.Range(minTimeSpawn,maxTimeSpawn);
            }
        }
    }
    Vector3 RandomLocation()
    {
        atX=Random.Range(minX,maxX);
        atY=Random.Range(minY,maxY);
        return new Vector3(atX,atY,0f);
    }
}
