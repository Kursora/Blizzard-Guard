using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class EffectDistributor : MonoBehaviour
{
    public List<GameObject> EffectPool;
    public GameObject text,ph;
    public float dieAfter;
    public bool gotShot;
    public AudioSource soundEffect;
    [SerializeField]private AudioClip appear;
    void Start()
    {
        soundEffect=GetComponent<AudioSource>();   
        soundEffect.PlayOneShot(appear,0.35f);
        Destroy(gameObject,dieAfter);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(!gotShot)
        if(collision.gameObject.tag=="Bullet" || collision.gameObject.tag=="Phantom")
        {
            gotShot=true;
            StartCoroutine(MainGame.game.incScore(10000*MainGame.game.multiplier));
            GetComponent<SpriteRenderer>().sprite=null;
            GiveRandomEffect();
            if(soundEffect.clip!=null)
                soundEffect.PlayOneShot(soundEffect.clip);
            Destroy(gameObject,2f);
        }
    }
    void GiveRandomEffect()
    {
        int i=Random.Range(0,EffectPool.Count);
        Instantiate(EffectPool[i],transform.position,Quaternion.identity);
        ph=Instantiate(text,transform.position,Quaternion.identity);
        ph.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text=EffectPool[i].name;
    }
}
