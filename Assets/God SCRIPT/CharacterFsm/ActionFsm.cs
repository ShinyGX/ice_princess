﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ActionFsm :BaseFsm{

   private enum  PlayerState:int{
        Idle=0 ,
        Walk = 1,
        Run = 2
        };

    private int _double = 1;//控制是否跑动
    private float _temp = 1;//用来作为跑动lerp的插值
    private Animator _ani;

    private const float _runTime = 0.36f;
    private float _runTimer = 0f;

    private GameObject player = null;

    private RandomAudioPlayer _audio;

    private float _timeLost = 0.0f;//记录是否连续按下

    public override void MyUpdate(Animator _ani)//主循环方法
    {
        if(Input.GetMouseButtonDown(0))
        {
            ControlFsm fsm = GameObject.FindWithTag("Player").GetComponent<ControlFsm>();
            if(_ani.GetFloat("forward")>=1.5f)
            {
                _ani.SetBool("IsRunAttack", true);
            }
            fsm.Translate(fsm.GetFsmAssemble(1));
        }
        
        LocalMotion(_ani);//移动
        LocalRotation();//旋转

        _runTimer += Time.deltaTime;

        WalkOrRun();
    }

    private void LocalRotation()//专门用来旋转的
    { 
        
        player.transform.Rotate(player.transform.up,45f*10.0f * Time.deltaTime * CharacterInput.Instance.m_MovementRight);
        
    }

    private void LocalMotion(Animator _ani)//专门用来移动的
    {
        if (_ani.GetFloat("forward") > 1.5)
        {
            if (_runTimer > _runTime)
            {
                _runTimer = 0f;
            }

        }

        _temp = Mathf.Lerp(_temp, _double,0.05f);//计算插值
        _ani.SetFloat("forward", CharacterInput.Instance.m_MovementForward*_temp);//设置为插值
        _ani.SetFloat("right", CharacterInput.Instance.m_MovementRight);
        PlayStepAudio(_ani);//播放是否发出脚步声
    }
    public override void PrepareEnter(Animator _ani)
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        //_ani.SetInteger("State", (int)PlayerState.Idle);

        _audio = _ani.GetComponent<RandomAudioPlayer>();
    }

    public override void PrepareExit(Animator _ani)
    {
        _temp = 1;
        _double = 1;
        _ani.SetFloat("forward", 0f);
        _ani.SetFloat("right", 0f);
        PlayInfo.instance._actionInfo = PlayInfo.actionInfo.walk;
    }
    public void Rotate()
    {
        GameObject player = GameObject.FindWithTag("Player");
        GameObject ca = GameObject.FindWithTag("FollowCamera");
        Vector3 temptarget = new Vector3(ca.transform.position.x, player.transform.position.y, ca.transform.position.z);
        player.transform.rotation = Quaternion.Slerp(player.transform.rotation, Quaternion.LookRotation(-temptarget + player.transform.position), 5);
    }
    private void WalkOrRun()
    {
        //更具是否是跑的状态，调整到是否为跑，以便于攻击，闪避能快速的进入
        if (PlayInfo.Instance._actionInfo==PlayInfo.actionInfo.Run|| PlayInfo.Instance._actionInfo==PlayInfo.actionInfo.BackRun)
        {
            _double = 2;
        }
        else
        {
            _double = 1;
        }
    }
    private void PlayStepAudio(Animator _ani)
    {
        float footCurve = _ani.GetFloat("footCurve");

        if (footCurve > 0.01f && _audio.canPlay)
        {
            _audio.isPlaying = true;
            _audio.canPlay = false;
            _audio.RandomPlay();
        }
        else if (footCurve < 0.01f && !_audio.canPlay)
        {
            _audio.canPlay = true;
        }

        if (CharacterInput.Instance.m_MovementForward > 0.1f)
        {
            if (!_audio.isPlaying)
            {
                // _audio.RandomPlay(null, _double == 1 ? 1 : 0);
                _audio.isPlaying = true;
            }
            else
            {
                _audio.isPlaying = false;
            }
        }
    }
}
