using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float deadTime,force,damage,cooldownTime,inaccuracy,Rmin,Rmax;
    public Vector3 mousePos;
    public bool allowRandom,isParent,noSpin,changeSkin,explosive,explosiveDead;
    public Transform[] children;
    public Transform player;
    [SerializeField]private GameObject explode;
    [SerializeField]private Rigidbody2D rb;
    private Camera mCam;
    void Awake()
    {
        player=MainGame.game.player;
        if(isParent)
        foreach (Transform child in children) 
        {
            child.SetParent(null, true);
        }
        damage=MainGame.game.attackMul*damage;
    }
    void Start()
    {
        if(allowRandom)
        {
            inaccuracy=Random.Range(Rmin,Rmax);
        }
        if(!noSpin)
        {
            mCam=MainGame.game.mCam;
            mousePos= mCam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 dir=mousePos - transform.position;
            Vector3 rot= transform.position - mousePos;
            float baseAngle = Mathf.Atan2(dir.y,dir.x)*Mathf.Rad2Deg;
            float spreadAngle = baseAngle + inaccuracy;
            Vector3 spreadDirection = new Vector3(Mathf.Cos(spreadAngle * Mathf.Deg2Rad), Mathf.Sin(spreadAngle*Mathf.Deg2Rad), 0);//Math
            rb.velocity = spreadDirection * force;
            transform.rotation = Quaternion.Euler(0f,0f,spreadAngle+90);
        }
        Destroy(gameObject,deadTime);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Ice" && explosive)
            Instantiate(explode,transform.position,Quaternion.identity);
        if(collision.gameObject.tag=="Finish")
        {
            Destroy(gameObject);
        }
        
    }
    void OnDestroy()
    {
        if(explosiveDead)
            Instantiate(explode,transform.position,Quaternion.identity);
    }
}
