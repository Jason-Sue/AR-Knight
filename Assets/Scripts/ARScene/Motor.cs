using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motor : MonoBehaviour
{
    private Animator anim;

    public float WalkingSpeed = 1;

    public float DestinationThreshold = .2f;

    private Vector3 LookToCameraVector;
    private Quaternion lookToCamera;

    public Transform HeadIKTarget;


    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (KnightState.Instance.state == KnightState.State.Walk)
        {
            //WalkToPoint();
        }
        if (KnightState.Instance.state == KnightState.State.Wield)
        {
            AttackAction();
        }

        //to finish the process that walk to an enemy and attack, when the attack animation is finished, change the state to Idle
        //if look at camera is needed, then do the rotation
        //whithout invoke, it will directly change state to Idle and rotate immediately without finishing the attack animation
        if (KnightState.Instance.state == KnightState.State.Enemy)
        {
            WalkToEnemy();
        }
        if (KnightState.Instance.state == KnightState.State.Attack)
        {
            Invoke(nameof(FinishAttack), 2);
        }

        if ((KnightState.Instance.state == KnightState.State.Idle || KnightState.Instance.state == KnightState.State.Turn) && KnightState.Instance.isTurningToMe)
        {
            LookAtCameraWhenIdle();
        }
    }

    //walk to a point
    private void WalkToPoint()
    {
        transform.LookAt(new Vector3(KnightState.Instance.Destination.x, 0, KnightState.Instance.Destination.z));
        anim.SetBool("Walk", true);
        transform.Translate((KnightState.Instance.Destination - transform.position) * Time.deltaTime * WalkingSpeed);
        if ((Vector3.Distance(transform.position, KnightState.Instance.Destination) < DestinationThreshold))
        {
            anim.SetBool("Walk", false);
            KnightState.Instance.state = KnightState.State.Idle;
        }
    }

    //walk to an enemy, and then attack
    private void WalkToEnemy()
    {
        transform.LookAt(new Vector3(KnightState.Instance.Destination.x, 0, KnightState.Instance.Destination.z));
        anim.SetBool("Walk", true);
        transform.Translate(Vector3.forward * Time.deltaTime * WalkingSpeed);
        //if ((Vector3.Distance(transform.position, KnightState.Instance.Destination) < KnightState.Instance.EnemyThreshold))
        //{
        //    AttackAction();
        //    ResetAnim();
        //    anim.SetTrigger("Attack");
        //    KnightState.Instance.state = KnightState.State.Attack;
        //}
    }
    private void OnTriggerEnter(Collider other)
    {
        ResetAnim();
        anim.SetTrigger("Attack");
        KnightState.Instance.state = KnightState.State.Attack;
    }

    //finish the attack animation, and then change to Idle, if without invoke, it will change state even before animation starts
    //while look at camera is needed, it will rotate and attack, which is wielding to camera.
    private void FinishAttack()
    {
        if (KnightState.Instance.state == KnightState.State.Attack)
        {
            KnightState.Instance.state = KnightState.State.Idle;
        }
    }

    private void AttackAction()
    {
        ResetAnim();
        anim.SetTrigger("Attack");
        KnightState.Instance.state = KnightState.State.Idle;
    }

    private void ResetAnim()
    {
        anim.SetBool("TurnLeft", false);
        anim.SetBool("TurnRight", false);
        anim.SetBool("Walk", false);
    }

    private void AlwaysLookToCamera()
    {
        if (Quaternion.Angle(transform.rotation, lookToCamera) > 5f)
        {
            KnightState.Instance.state = KnightState.State.Turn;
        }
        else
        {
            KnightState.Instance.state = KnightState.State.Idle;
            ResetAnim();
        }
    }

    //when look at camera is needed, call this method
    private void LookAtCameraWhenIdle()
    {
        LookToCameraVector = new Vector3(Camera.main.transform.position.x, transform.position.y, Camera.main.transform.position.z) - transform.position;
        lookToCamera = Quaternion.LookRotation(LookToCameraVector);
        AlwaysLookToCamera();
        if (KnightState.Instance.state == KnightState.State.Turn)
        {
            if (Vector3.Angle(Vector3.Cross(transform.forward, LookToCameraVector), transform.up) == 0)
            {
                anim.SetBool("TurnRight", true);
            }
            else
            {
                anim.SetBool("TurnLeft", true);
            }
            transform.rotation = Quaternion.Lerp(transform.rotation, lookToCamera, .1f);
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (KnightState.Instance.isLookingAtMe)
        {
            if (HeadIKTarget)
            {
                anim.SetLookAtWeight(1);
                anim.SetLookAtPosition(HeadIKTarget.position);
            }
        }
    }
}
