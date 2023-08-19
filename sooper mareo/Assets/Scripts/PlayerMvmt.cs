using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMvmt : MonoBehaviour
{

    Vector2 moveInput;
    Rigidbody2D myRigidBody;

    [Header("Speeds")]
    [SerializeField] float runSpeed = 1000f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 1.0f;

    [Header("Animations")]
    public Animator knightAnimator;
    [SerializeField] AnimationClip Attack;

    [Header("Colliders")]
    CapsuleCollider2D myCapsuleCollider;
    BoxCollider2D myFeetCollider;
    CircleCollider2D myAttackCollider;

    [Header("Enemies")]
    EnemyMovement enemymvmt;
    EnemyMovement closestEnemy;
    EnemyMovement[] enemies;
    float startingGravity;
    public bool isAlive = true;


    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        knightAnimator = GetComponent<Animator>();
        myCapsuleCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        myAttackCollider = GetComponent<CircleCollider2D>();
        enemies = FindObjectsOfType<EnemyMovement>();
        myAttackCollider.enabled = false;
        startingGravity = myRigidBody.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        knightAnimator.SetFloat("AirSpeedY", myRigidBody.velocity.y);
        if (!isAlive) { return; }
        getClosestEnemy();
        DieEnemy();
        Run();
        FlipSprite();
        climbLadder();
        DieKnight();
        controlVelocity();
    }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), 1f);
        }

    }

    void OnMove(InputValue value)
    {
        if (!isAlive) { return; }
        moveInput = value.Get<Vector2>();

    }

    void OnJump(InputValue value)
    {
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }
        if (!isAlive) { return; }

        if (value.isPressed)
        {
            myRigidBody.velocity += new Vector2(0f, jumpSpeed);
        }
    }

    void OnAttack(InputValue value)
    {
        if (value.isPressed && isAlive)
        {
            knightAnimator.SetBool("Attack3", true);
            myAttackCollider.enabled = true;

            Invoke("AttackLogic", Attack.length);
            
        }
    }

    void OnRoll(InputValue value)
    {
        if (value.isPressed && isAlive)
        {
            knightAnimator.SetBool("Roll", true);
        }
    }

    void AttackLogic()
    {
        myAttackCollider.enabled = false;
    }

    void climbLadder()
    {
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing"))) 
        {
            myRigidBody.gravityScale = startingGravity;
            knightAnimator.SetBool("WallSlide", false);
            return;
        }

        myRigidBody.gravityScale = 0f;
        knightAnimator.SetBool("WallSlide", true);
        Vector2 climbVelocity = new Vector2(myRigidBody.velocity.x, moveInput.y * climbSpeed);
        myRigidBody.velocity = climbVelocity;
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        { 
            knightAnimator.SetInteger("AnimState", 1);
        }
        else
        {
            knightAnimator.SetInteger("AnimState", (int) Mathf.Epsilon);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            knightAnimator.SetBool("Grounded", true);
        }
    }

    void DieKnight()
    {
        if (myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Hazards")) || 
           (myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Enemies"))) && 
           closestEnemy.enemyIsAlive)
        {
            knightAnimator.SetBool("Death", true);
            isAlive = false;
            myRigidBody.velocity = new Vector2(0f, 0f);
            Invoke("reloadScene", 2.25f);
        }
    }

    void DieEnemy()
    {

        if (myAttackCollider.IsTouchingLayers(LayerMask.GetMask("Enemies")))
        {
            closestEnemy.enemyIsAlive = false;
            closestEnemy.enemyAnimator.SetBool("Death", true);
            closestEnemy.Invoke("DestroyEnemy", 1.2f);
            closestEnemy.enemyRigidBody.velocity = new Vector2(0f, 0f);

            Invoke("getClosestEnemy", 1.4f);

        }
    }

    private void getClosestEnemy()
    {
        float oldDistance = Mathf.Infinity;

        foreach (EnemyMovement m in enemies)
        {
            if (m == null) { return; }
            float dist = Vector3.Distance(gameObject.transform.position, m.transform.position);
            if (dist < oldDistance)
            {
                closestEnemy = m;
                oldDistance = dist;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            knightAnimator.SetBool("Grounded", false);
            
        }       
    }

    void reloadScene()
    {
        FindObjectOfType<GameSession>().ProcessPlayerDeath();
    }

    void controlVelocity()
    {
        if (myRigidBody.velocity.y > 30f)
        {
            myRigidBody.velocity = new Vector2(myRigidBody.velocity.x, 30f);
        }

        if (myRigidBody.velocity.y < -100f)
        {
            reloadScene();
        }
    }
}
