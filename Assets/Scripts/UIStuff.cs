using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UIStuff : MonoBehaviour
{
    public TMP_Text A,mul,C,D,E,log,capacity,dr1,dr2,dr3,dr4;
    public Slider B,musicSlider;
    public bool isIncreasing;
    public RawImage snow;
    public GameObject fume,GO,pause;
    public Image weapon;
    [SerializeField] private float r1,r2,r3,r4;
    [SerializeField] private AudioClip attackSound,fireRateSound;
    void Start()
    {
        StartCoroutine(Starting());
    }
    void Update()
    {
        Debuger();
        SetIntensify(-0.001f-(0.0005f*MainGame.game.multiplier));
        if(isIncreasing)
            B.value=MainGame.game.capacity/MainGame.game.MaxCap;
        mul.text="X"+MainGame.game.multiplier;
        C.text=""+MainGame.game.score;
        capacity.text=""+MainGame.game.capacity.ToString("0");
        if(MainGame.game.isPlaying)
        {
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                AbilityChill();
            }
            if(Input.GetKeyDown(KeyCode.Alpha2))
            {
                AbilityShock();
            }
            if(Input.GetKeyDown(KeyCode.Alpha3))
            {
                UpgradeWeapon();
            }
            if(Input.GetKeyDown(KeyCode.Alpha4))
            {
                UpgradeFireRate();
            }
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                MainGame.game.isPlaying=false;
                Time.timeScale=0f;
                pause.SetActive(true);
            }
        }
        else if(!MainGame.game.isPlaying)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                Continue();
            }
        }
    }
    public void Continue()
    {
        MainGame.game.isPlaying=true;
        Time.timeScale=1f;
        pause.SetActive(false);
    }
    public void ChangeVolume()
    {
        MainGame.game.bmg.volume=musicSlider.value;
    }
    void Debuger()
    {
        A.text="Score: "+MainGame.game.score+"\nMultiply: x"+MainGame.game.multiplier+"\nMin Grow Speed: "+MainGame.game.minSpeed+"\nMax Grow Speed: "+MainGame.game.maxSpeed;
    }
    public void GameOver()
    {
        GO.SetActive(true);
        float minutes = Mathf.Floor(MainGame.game.timePlayed / 60);
        float seconds = MainGame.game.timePlayed%60;
        E.text="Your Score:"+ MainGame.game.score+"\nMultiplier: "+ MainGame.game.multiplier+"\nTime Played: "+minutes.ToString("00")+":"+seconds.ToString("00")+"\nIce Broken: "+MainGame.game.iceCleared+" m";
    }
    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void Slidering(float a,float timer)
    {
        isIncreasing=false;
        StartCoroutine(DecreaseOverTime(B.value,a,timer));
    }
    IEnumerator DecreaseOverTime(float startValue, float targetValue, float time)
    {
        float elapsedTime = 0f;
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            float newValue = Mathf.Lerp(startValue, targetValue, elapsedTime / time);
            B.value = newValue;
            yield return null;
        }
        B.value = MainGame.game.capacity/MainGame.game.MaxCap;
        isIncreasing=true;
    }
    public IEnumerator Fume()
    {
        fume.SetActive(true);
        yield return new WaitForSeconds(1.2f);
        fume.SetActive(false);
    }
    public void SetIntensify(float intensify)
    {
        snow.uvRect=new Rect(snow.uvRect.position.x+0.01f*intensify,snow.uvRect.position.y+0.01f*-intensify,snow.uvRect.width,snow.uvRect.height);
    }
    IEnumerator Starting()
    {
        yield return new WaitForSeconds(3f);
        MainGame.game.isPlaying=true;
        D.text="Go";
        yield return new WaitForSeconds(0.5f);
        D.gameObject.SetActive(false);
    }
    public IEnumerator ShowLog(string a,float time)
    {
        log.text=a;
        log.gameObject.SetActive(true);
        yield return new WaitForSeconds(time);
        log.gameObject.SetActive(false);
    }
    public void AbilityChill()
    {
        if(MainGame.game.capacity>=r1)
        {
            Slidering((MainGame.game.capacity-r1)/MainGame.game.MaxCap,1f);
            MainGame.game.capacity-=r1;
            MainGame.game.Chilling();
            StartCoroutine(ShowLog("Chilling",3f));
            r1+=100f;
            dr1.text=""+r1.ToString("0");
        }
        else
        {
            StartCoroutine(ShowLog("Not Enough Capacity",3f));
        }
    }
    public void AbilityShock()
    {
        if(MainGame.game.capacity>=r2)
        {
            Slidering((MainGame.game.capacity-r2)/MainGame.game.MaxCap,1f);
            MainGame.game.capacity-=r2;
            StartCoroutine(MainGame.game.EnableShock());
            StartCoroutine(ShowLog("Shocking",3f));
            r2+=150f;
            dr2.text=""+r2.ToString("0");
        }
        else
        {
            StartCoroutine(ShowLog("Not Enough Capacity",3f));
        }
    }
    public void UpgradeWeapon()
    {
        if(MainGame.game.capacity>=r3)
        {
            MainGame.game.soundEffect2.PlayOneShot(attackSound);
            StartCoroutine(ShowLog("Attack Upgraded!",3f));
            Slidering((MainGame.game.capacity-r3)/MainGame.game.MaxCap,1f);
            MainGame.game.capacity-=r3;
            MainGame.game.attackMul+=0.075f;
            r3+=150f;
            dr3.text=""+r3.ToString("0");
        }
        else
        {
            StartCoroutine(ShowLog("Not Enough Capacity",3f));
        }
    }
    public void UpgradeFireRate()
    {
        if(MainGame.game.capacity>=r4)
        {
            MainGame.game.soundEffect2.PlayOneShot(fireRateSound);
            StartCoroutine(ShowLog("Fire Rate Upgraded!",3f));
            Slidering((MainGame.game.capacity-r4)/MainGame.game.MaxCap,1f);
            MainGame.game.capacity-=r4;
            MainGame.game.firerateMul-=0.035f;
            r4+=150f;
            dr4.text=""+r4.ToString("0");
        }
        else
        {
            StartCoroutine(ShowLog("Not Enough Capacity",3f));
        }
    }
}
