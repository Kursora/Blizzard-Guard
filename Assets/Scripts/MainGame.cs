using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGame : MonoBehaviour
{
    public static MainGame game;
    public Transform player;
    public Camera mCam;
    [SerializeField]private UIStuff ui;
    public int multiplier,score,diffculty,money,increasement,combo;
    public float minSpeed,maxSpeed,minRegrowTime,maxRegrowTime,minFreeze,maxFreeze,growthRate,freezeRate,growthMul,displayScore,iceCleared;
    public bool isPlaying,safe,critical;
    public float capacity,MaxCap,timePlayed,timeDeadCombo,firerateMul,attackMul;
    public List<GameObject> ice;
    public GameObject greenSafe,shock;
    [SerializeField]public AudioSource soundEffect,soundEffect2,bmg;
    [SerializeField]private AudioClip multiply,criticalSound,valve;
    private Coroutine music;
    void Awake()
    {
        game=this;
    }
    void Start()
    {
        bmg.Play();
        music=StartCoroutine(CheckLoop(12.745f));
        MaxCap=increasement*diffculty;
    }
    void Update()
    {
        if(isPlaying)
            timePlayed+=Time.deltaTime;
        if(timeDeadCombo>0f)
        {
            timeDeadCombo-=Time.deltaTime;
        }
        else
        {
            combo=0;
        }
        if(capacity>=MaxCap)
        {   
            maxSpeed+=growthRate*multiplier;
            minSpeed+=growthRate*multiplier;
            minFreeze+=freezeRate*multiplier;
            maxFreeze+=freezeRate*multiplier;
            capacity=capacity-MaxCap;
            MaxCap+=increasement;
            multiplier++;
            Chilling();
            ui.Slidering(capacity/MaxCap,2f);
            StartCoroutine(ui.ShowLog("Multipler x"+multiplier,3f));
            soundEffect.PlayOneShot(multiply,0.5f);
        }
    }
    public void critically()
    {
        if(!soundEffect2.isPlaying)
            soundEffect2.PlayOneShot(criticalSound);
        ui.SetIntensify(-0.1f);
    }
    public void Chilling()
    {
        soundEffect.PlayOneShot(valve,0.3f);
        StartCoroutine(ui.Fume());
        for(int i=0;i<10;i++)
            StartCoroutine(ice[i].GetComponent<GrowingIce>().ChillOut());
    }
    public void gameOver()
    {
        isPlaying=false;
        StopCoroutine(music);
        bmg.Stop();
        GetComponent<UIStuff>().GameOver();
    }
    public void safeZone(bool a)
    {
        if(a)
        {
            safe=true;
            greenSafe.SetActive(true);
        }
        else
        {
            safe=false;
            greenSafe.SetActive(false);
        }
    }
    public IEnumerator incScore(int a)
    {
        score+=a;
        displayScore=score;
        yield return new WaitForSeconds(0.01f);
    }
    public void changeWeaponSprite(Sprite sw)
    {
        ui.weapon.sprite=sw;
    }
    public IEnumerator EnableShock()
    {
        shock.SetActive(true);
        yield return new WaitForSeconds(5f);
        shock.SetActive(false);
    }
    IEnumerator CheckLoop(float time)
    {
        while (true)
        {
            yield return null;
            if (!bmg.isPlaying)
            {
                bmg.time = time;
                bmg.Play();
            }
        }
    }
}
