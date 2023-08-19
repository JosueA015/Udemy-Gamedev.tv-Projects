using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowboarder : MonoBehaviour
{
    [SerializeField] float spinSpeed = 2f;
    [SerializeField] float baseSpeed = 20f;
    [SerializeField] float boostSpeed = 30f;
    Rigidbody2D rb2d;
    SurfaceEffector2D surface;
    bool canMove = true;

    // Start is called before the first frame update
    void Start()
    {
         rb2d = GetComponent<Rigidbody2D>();
         surface = FindObjectOfType<SurfaceEffector2D>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerSpin();
        RespondToBoost();
    }

    void RespondToBoost()
    {
        if(Input.GetKey(KeyCode.UpArrow) && canMove)
        {
            surface.speed = boostSpeed;
        }
        else
        {
            surface.speed = baseSpeed;
        }
    }

    private void PlayerSpin()
    {
        if (Input.GetKey(KeyCode.LeftArrow) && canMove)
        {
            rb2d.AddTorque(spinSpeed);
        }

        else if (Input.GetKey(KeyCode.RightArrow) && canMove)
        {
            rb2d.AddTorque(-spinSpeed);
        }                               
    }

    public void disableControls()
    {
        canMove = false;
    }
}
