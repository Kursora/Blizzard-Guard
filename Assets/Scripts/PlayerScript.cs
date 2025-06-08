using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float speed;
    public float x,y;
    public bool moving;
    private Animator anim;
    private AudioSource footstep;
    void Start()
    {
        anim=GetComponent<Animator>();
        footstep=GetComponent<AudioSource>();
    }
    void Update()
    {
        if(MainGame.game.isPlaying)
        {
            x=Input.GetAxisRaw("Horizontal");
            y=Input.GetAxisRaw("Vertical");
            if (x != 0 || y != 0)
            {
                anim.Play("Running");
                if(!footstep.isPlaying)
                {
                    footstep.PlayOneShot(footstep.clip);
                }
                if(!moving)
                {
                    moving=true;
                }
            }
            else 
            {
                if(moving)
                {
                    moving = false;
                }
                anim.Play("Idle");
            }
        }
        else
        {
            x=0;
            y=0;
            anim.StopPlayback();
        }
        
    }
    void FixedUpdate()
    {   
        Vector3 movement = new Vector3(x,y,0f);
        movement = movement.normalized * speed*Time.fixedDeltaTime;
        transform.Translate(movement);
    }
}
