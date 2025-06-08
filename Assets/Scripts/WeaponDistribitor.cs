using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponDistribitor : MonoBehaviour
{
    public List<GameObject> WeaponPool;
    public List<Sprite> weapon;
    public List<AudioClip> sounds;
    public SpriteRenderer playerSkin;
    public Rotate player;
    public GameObject text,ph;
    public float dieAfter;
    public bool gotShot;
    [SerializeField]private AudioSource soundEffect;
    private AudioSource playerSound;
    [SerializeField]private AudioClip appear;
    void Start()
    {
        soundEffect.PlayOneShot(appear);
        Destroy(gameObject,dieAfter);
        player=GameObject.Find("PlayerWeapon").GetComponent<Rotate>();
        playerSkin=GameObject.Find("PlayerWeapon").GetComponent<SpriteRenderer>();
        playerSound=GameObject.Find("PlayerWeapon").GetComponent<AudioSource>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(!gotShot)
        if(collision.gameObject.tag=="Bullet" || collision.gameObject.tag=="Phantom")
        {
            gotShot=true;
            GetComponent<SpriteRenderer>().sprite=null;
            soundEffect.PlayOneShot(soundEffect.clip,0.5f);
            GiveRandomWeapon();
            StartCoroutine(MainGame.game.incScore(10000*MainGame.game.multiplier));
            Destroy(gameObject,2f);
            
        }
    }
    void GiveRandomWeapon()
    {
        int i=Random.Range(0,WeaponPool.Count);
        ph=Instantiate(text,transform.position,Quaternion.identity);
        ph.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text=WeaponPool[i].name;
        playerSkin.sprite=weapon[i];
        MainGame.game.changeWeaponSprite(weapon[i]);
        player.weapon=WeaponPool[i];
        if(sounds[i]!=null)
            playerSound.clip=sounds[i];
        else 
            playerSound.clip=null; 
        player.realcd=0f;
    }
}
