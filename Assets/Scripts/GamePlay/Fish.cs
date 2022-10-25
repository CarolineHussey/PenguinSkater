using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    private Animator anim;

    private void Start()
    {
        anim = GetComponentInParent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            PickUpFish();
    }

    private void PickUpFish()
    {
        //Increment FishCount
        anim?.SetTrigger("PickUp"); 
        GameStat.Instance.CollectFish();
        //Increment Score
        //Play sfx
        //Trigger Animation
        
    }

    public void OnShowChunk()
    {
        anim?.SetTrigger("Idle");
    }
}
