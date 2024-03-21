using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ARButtonManager : MonoBehaviour
{
    public Button GenerateButton;
    public GameObject Enemy;
    public Transform[] EnemyPoints;
    public Transform Player;

    public Button BackButton;

    public Button LookAtMeButton;
    public GameObject ClueLookAtMe;

    public Button TurnToMeButton;
    public GameObject ClueTurnToMe;

    public Button UseJoystickButton;
    public FixedJoystick Controller;
    public float MovingSpeed = 1f;

    public Text StatusText;
    private readonly string initialInfo = "Basic Transformation";
    private readonly string turnAndLookInfo = "turn and/or look";
    private readonly string useJoystickInfo = "using Joystick";

    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        GenerateButton.onClick.AddListener(Generate);
        BackButton.onClick.AddListener(BackPage);
        LookAtMeButton.onClick.AddListener(LookAtMe);
        TurnToMeButton.onClick.AddListener(TurnToMe);
        UseJoystickButton.onClick.AddListener(UseJoystick);

        anim = Player.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (KnightState.Instance.isUsingJoystick)
        {
            if (MathF.Abs(Controller.Vertical) > 0.01f)
            {
                Vector3 direction = Vector3.forward * Controller.Vertical + Vector3.right * Controller.Horizontal;
                //Debug.DrawRay(Player.transform.position, direction, Color.red);

                Player.rotation = Quaternion.LookRotation(direction);
                Player.Translate(MovingSpeed * Time.deltaTime * Vector3.forward);
                anim.SetBool("Walk", true);
            }
            else
            {
                anim.SetBool("Walk", false);
            }
        }
    }

    private void Generate()
    {
        int index = UnityEngine.Random.Range(0, EnemyPoints.Length);
        GameObject enemyModel = Instantiate(Enemy, EnemyPoints[index].position, Quaternion.LookRotation(Player.position - EnemyPoints[index].transform.position));
        enemyModel.AddComponent<EnemyMotor>();
        BoxCollider box = enemyModel.AddComponent<BoxCollider>();
        box.center = new Vector3(0, 1, 0);
        box.size = new Vector3(2, 2, 1.5f);
        KnightState.Instance.Destination = new Vector3(enemyModel.transform.position.x, 0, enemyModel.transform.position.z);
        KnightState.Instance.state = KnightState.State.Enemy;
    }

    private void BackPage()
    {
        SceneManager.LoadScene("Welcome");
    }

    private void LookAtMe()
    {
        KnightState.Instance.isLookingAtMe = !KnightState.Instance.isLookingAtMe;
        if (KnightState.Instance.isLookingAtMe)
        {
            ClueLookAtMe.SetActive(true);
            StatusText.text = turnAndLookInfo;
        }
        else
        {
            ClueLookAtMe.SetActive(false);
            StatusText.text = initialInfo;
        }
    }

    private void TurnToMe()
    {
        KnightState.Instance.isTurningToMe = !KnightState.Instance.isTurningToMe;
        if (KnightState.Instance.isTurningToMe)
        {
            ClueTurnToMe.SetActive(true);
            StatusText.text = turnAndLookInfo;
        }
        else
        {
            ClueTurnToMe.SetActive(false);
            StatusText.text = initialInfo;
        }
    }

    private void UseJoystick()
    {
        KnightState.Instance.isUsingJoystick = !KnightState.Instance.isUsingJoystick;
        if (KnightState.Instance.isUsingJoystick)
        {
            Controller.gameObject.SetActive(true);
            StatusText.text = useJoystickInfo;
        }
        else
        {
            Controller.gameObject.SetActive(false);
            StatusText.text = initialInfo;
        }
    }
}
