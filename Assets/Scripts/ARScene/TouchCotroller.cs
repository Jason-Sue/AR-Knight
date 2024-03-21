using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchCotroller : MonoBehaviour
{
    public Transform Target;
    public float rotateSensitivity = .4f;
    public float scaleSensitivity = .001f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 && !KnightState.Instance.isUsingJoystick)
        {
            BasicTransformation();
        }
    }

    //basic transformation, including scale, rotation,interaction with model
    private void BasicTransformation()
    {
        //interaction with model
        if (Input.GetTouch(0).phase == TouchPhase.Stationary)
        {
            RaycastHit hit;

            //最后一句为防止UI误触
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.GetTouch(0).position), out hit) && !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                if (hit.collider.gameObject.CompareTag("Sword"))
                {
                    KnightState.Instance.state = KnightState.State.Wield;
                }
                if (hit.collider.gameObject.CompareTag("Ground"))
                {
                    KnightState.Instance.state = KnightState.State.Walk;
                    KnightState.Instance.Destination = hit.point;
                }
            }
        }
        //scale, rotation, if model is looking at camera, do not do it because it is meaningless
        if (Input.GetTouch(0).phase == TouchPhase.Moved && !KnightState.Instance.isLookingAtMe && !KnightState.Instance.isTurningToMe)
        {
            float delta_x = Input.GetTouch(0).deltaPosition.x;
            float delta_y = Input.GetTouch(0).deltaPosition.y;
            if (Mathf.Abs(delta_x) > Mathf.Abs(delta_y))
            {
                Target.Rotate(new Vector3(0, -Input.GetTouch(0).deltaPosition.x * rotateSensitivity, 0));
            }
            else
            {
                Target.localScale += delta_y * Vector3.one * scaleSensitivity;
            }
        }
    }
}
