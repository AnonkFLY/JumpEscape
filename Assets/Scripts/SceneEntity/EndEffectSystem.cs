using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndEffectSystem : MonoBehaviour
{
    private Animator _ainmator;
    private ParticleSystem _particle;
    private void Awake()
    {
        _ainmator = GetComponentInChildren<Animator>();
        _particle = GetComponentInChildren<ParticleSystem>();
        //Play();
    }
    //private void Update()
    //{
    //    if(Input.GetKeyDown(KeyCode.P))
    //    {
    //        Play();
    //    }
    //}

    public void Play()
    {
        _ainmator.Play("Effect");
        _particle.Play();
    }
}
