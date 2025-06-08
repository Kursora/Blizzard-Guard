using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GrowingIce : MonoBehaviour
{
    [SerializeField]private float speed,timeStunned,freezeSpeed,dyingTime;
    [SerializeField]private bool isGrowing,isFreezing,stunned,isDying;
    [SerializeField]private GameObject freezeColumn,skull,text,ph;
    [SerializeField]private SpriteRenderer skull1;
    [SerializeField]private Sprite a,b;
    [SerializeField]private AudioSource se;
    [SerializeField]private AudioClip iceBreak,iceCritical,icePreCritical;
    void Start()
    {
        speed=Random.Range(MainGame.game.minSpeed,MainGame.game.maxSpeed);
        skull1=skull.GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        if(timeStunned>0f)
        {
            timeStunned-=Time.deltaTime;
        }
        else stunned=false;
        if(MainGame.game.isPlaying)
        { 
            if(!stunned)
            {
                if(isGrowing && transform.localScale.y<26.74653f)
                {
                    isDying=false;
                    skull.SetActive(false);
                    isFreezing=false;
                }
                if(isFreezing)
                {
                    if(freezeColumn.transform.localScale.y>=26.74653f)
                    {
                        if(!isDying)
                        {
                            se.PlayOneShot(iceCritical,0.5f);
                        }
                        isDying=true;
                        isFreezing=false;
                        skull1.sprite=a;
                        skull.transform.localScale=new Vector3(18f,18f,0);
                    }
                    else
                    {
                        MainGame.game.critical=false;
                    }
                }
                if(freezeColumn.transform.localScale.y>=(26.74653f*0.55f))
                {
                    MainGame.game.critically();
                }
                if(isDying)
                {
                    freezeColumn.transform.localScale=new Vector3(transform.localScale.x,26.74653f,0f);
                    dyingTime-=Time.deltaTime;
                    if(dyingTime<=0)
                    {
                        MainGame.game.gameOver();
                        isDying=false;
                    }
                }
                else dyingTime=Mathf.Clamp(4f/MainGame.game.multiplier,1.5f,4f);
            }
        }
    }
    void FixedUpdate()
    {
        
        if(MainGame.game.isPlaying)
        { 
            if(!stunned)
            {
                if(isGrowing && transform.localScale.y<26.74653f)
                {
                    transform.localScale +=new Vector3(0f,speed*MainGame.game.growthMul,0f);
                    freezeColumn.transform.localScale=new Vector3(freezeColumn.transform.localScale.x,0f,0f);
                }
                if(isFreezing)
                {
                    freezeColumn.transform.localScale+=new Vector3(0f,freezeSpeed*MainGame.game.growthMul,0f);
                }
            }
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(!stunned)
        {
            if(collision.gameObject.CompareTag("Bullet") || collision.gameObject.CompareTag("Phantom"))
            {
                float damage=collision.gameObject.GetComponent<BulletScript>().damage;
                se.PlayOneShot(se.clip, Mathf.Clamp(damage*0.35f,0.1f,1f));
                StartCoroutine(Decreaser(damage));
            }
            if(collision.gameObject.CompareTag("Bullet"))
            {
                Destroy(collision.gameObject);
            }
            if(collision.gameObject.tag=="Dead")
            {
                if(!MainGame.game.safe)
                {
                    skull1.sprite=b;
                    se.PlayOneShot(icePreCritical,0.5f);
                    skull.SetActive(true);
                    transform.localScale=new Vector3(transform.localScale.x,26.74653f,0f);
                    isGrowing=false;
                    freezeSpeed=Random.Range(MainGame.game.minFreeze,MainGame.game.maxFreeze);
                    isFreezing=true;
                }
                else
                {
                    StartCoroutine(ChillOut());
                    MainGame.game.safeZone(false);
                }
            }
        }
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Phantom")
        {
            StartCoroutine(Decreaser(collision.gameObject.GetComponent<BulletScript>().damage));
        }
    }
    IEnumerator Decreaser(float a)
    {
        freezeColumn.transform.localScale=new Vector3(freezeColumn.transform.localScale.x,0f,0f);
        isFreezing=false;
        isGrowing=false;
        isDying=false;
        skull.SetActive(false);
        skull.transform.localScale=new Vector3(15f,15f,0);
        StartCoroutine(MainGame.game.incScore((int)Mathf.Clamp((a)*100,10,100000)* MainGame.game.multiplier));
        for(int i=0;i<20;i++)
        {
            if(transform.localScale.y>0)
            {
                    transform.localScale-=new Vector3(0f,a/20,0f);
                    if(transform.localScale.y>0)
                    {
                        MainGame.game.capacity+=a/20;
                        MainGame.game.iceCleared+=a/20;
                    }
            }
            if(transform.localScale.y<=0f && !stunned)
            {
                stunned=true;
                se.PlayOneShot(iceBreak,0.2f);
                transform.localScale=new Vector3(transform.localScale.x,0,0f);
                MainGame.game.timeDeadCombo=4f;
                MainGame.game.combo++;
                StartCoroutine(MainGame.game.incScore(Mathf.Clamp(1000* MainGame.game.multiplier+100*MainGame.game.multiplier*MainGame.game.combo,0,500*MainGame.game.multiplier*2)));
                ph=Instantiate(text,transform.position,Quaternion.identity);
                MainGame.game.capacity+=1.5f*MainGame.game.multiplier;
                ph.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text=""+(Mathf.Clamp(1000* MainGame.game.multiplier+100*MainGame.game.multiplier*MainGame.game.combo,0,1000*MainGame.game.multiplier*2))+"\nCombo x"+MainGame.game.combo;
                isGrowing=true;
                speed=Random.Range(MainGame.game.minSpeed,MainGame.game.maxSpeed);
                timeStunned=Random.Range(MainGame.game.minRegrowTime,MainGame.game.maxRegrowTime);
                break;
            }
            yield return new WaitForSeconds(0.02f);
        }
        isGrowing=true;
    }
    public IEnumerator ChillOut()
    {
        stunned=true;
        MainGame.game.critical=false;
        isGrowing=false;
        isFreezing=false;
        isDying=false;
        freezeColumn.transform.localScale=new Vector3(freezeColumn.transform.localScale.x,0f,0f);
        float originalYScale = transform.localScale.y;
        speed=0;
        yield return new WaitForSeconds(1f);
        if (originalYScale > 0)
        {
            float targetYScale = originalYScale*1/6;
            float scaleDifference = transform.localScale.y - targetYScale;
            for (int i = 0; i < 20; i++)
            {
                float shrinkAmount = scaleDifference / 20;
                transform.localScale -= new Vector3(0f, shrinkAmount, 0f);
                if (transform.localScale.y < 0)
                {
                    transform.localScale = new Vector3(transform.localScale.x, 0, transform.localScale.z);
                    break;
                }
                yield return new WaitForSeconds(0.01f);
            }
        }
        speed=Random.Range(MainGame.game.minSpeed,MainGame.game.maxSpeed);
        isGrowing=true;
        stunned=false;
    }
}
