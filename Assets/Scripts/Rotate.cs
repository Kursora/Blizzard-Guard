using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public Camera mCam;
    private Vector3 target;
    private bool firable;
    public bool notSentry;
    public GameObject weapon;
    public float coolDown,realcd,add;
    public SpriteRenderer sr;
    private AudioSource soundEffect;
    void Start()
    {
        sr=GetComponent<SpriteRenderer>();
        mCam=GameObject.Find("Main Camera").GetComponent<Camera>();
        soundEffect=GetComponent<AudioSource>();
    }
    void Update()
    {
        if(MainGame.game.isPlaying)
        {
            target=mCam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 dif = target - transform.position;
            float rotZ=Mathf.Atan2(dif.y,dif.x) * Mathf.Rad2Deg;
            transform.rotation =Quaternion.Euler(0f,0f,rotZ+90+add);
            if(notSentry)
                if (dif.x < 0)
                {
                    sr.flipY = false;
                }
                else 
                {
                    sr.flipY = true;
                }            
            if(!firable)
            {
                realcd-=Time.deltaTime;
                if(realcd<=0f)
                {
                    firable=true;
                }
            }
            if(Input.GetMouseButton(0) && firable)
            {
                firable=false;
                if(soundEffect.clip!=null)
                    soundEffect.PlayOneShot(soundEffect.clip,0.5f);
                Instantiate(weapon,transform.position,Quaternion.identity);
                realcd=weapon.GetComponent<BulletScript>().cooldownTime*MainGame.game.firerateMul;
            }
        }
    }
}
