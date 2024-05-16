using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;
public class HoldToLoadLevel : MonoBehaviour
{
    public float holdDuration = 1f;//thoi gian hold de load level
    public Image fillCircle;

    private float holdTimer = 0;
    private bool isHolding = false;


    public static event Action OnHoldComplete;


    // Update is called once per frame
    void Update()
    {
        if (isHolding)
        {
            holdTimer += Time.deltaTime;
            fillCircle.fillAmount = holdTimer / holdDuration;
            if (holdTimer >= holdDuration)
            {
                //loadLevel
                OnHoldComplete.Invoke();
                ResetHold();
            }
        }
    }


    public void OnHold(InputAction.CallbackContext context)
    {
        if (context.started)//start se tinh tu luc bat dau nhan phim
        {
            isHolding = true;
        }
        else if(context.canceled)//tinh tu frame cuoi khi k nhan phim
        {
            ResetHold();
        }
    }

    private void ResetHold()
    {
        isHolding = false;
        holdTimer = 0;
        fillCircle.fillAmount = 0;
    }
}
