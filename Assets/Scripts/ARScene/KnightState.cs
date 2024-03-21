using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class KnightState : MonoBehaviour
{
    public static KnightState Instance;

    public enum State
    {
        Walk,
        Wield,//wield the sword
        Attack,//walk to an enemy, and then change state to attack
        Idle,
        Enemy,//walk to an enemy
        Turn//turn around to face camera
    }
    public State state = State.Idle;

    public Vector3 Destination { get; set; }
    public float EnemyThreshold = 1f;
    public bool isGenerating { get; set; } = false;//generating an enemy, not doing transformation
    public bool isLookingAtMe { get; set; } = false;//look at camera or not
    public bool isTurningToMe { get; set; } = false;
    public bool isUsingJoystick { get; set; } = false;//use joystick to control, it will ban use basic transformation

    private void Start()
    {
        Instance = this;
    }
}
