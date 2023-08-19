using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    [SerializeField] float moveSpeed = 2.5f;
    public Rigidbody2D enemyRigidBody;
    public Animator enemyAnimator;
    PlayerMvmt playermvmt;
    public CapsuleCollider2D enemyCapsuleCollider;
    public BoxCollider2D enemyBoxCollider;

    public bool enemyIsAlive = true;
    
    void Start()
    {
        enemyRigidBody = GetComponent<Rigidbody2D>();
        enemyAnimator = GetComponent<Animator>();
        playermvmt = FindObjectOfType<PlayerMvmt>();
        enemyCapsuleCollider = GetComponent<CapsuleCollider2D>();   
        enemyBoxCollider = GetComponent<BoxCollider2D>();
    }

    
    void Update()
    {
        if (!enemyIsAlive) { return; }

        enemyRigidBody.velocity = new Vector2(moveSpeed, 0f); 
        if (enemyRigidBody.velocity.x > Mathf.Epsilon)
        {
            enemyAnimator.SetBool("IsWalking", true);
        }

        if (playermvmt.isAlive == false)
        {
            enemyCapsuleCollider.enabled = false;
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            moveSpeed = -moveSpeed;
            FlipEnemyFacing();
        }
    }

    private void FlipEnemyFacing()
    {
        transform.localScale = new Vector2((-enemyRigidBody.velocity.x), 2.5f);
    }

    void DestroyEnemy()
    {
        enemyCapsuleCollider.enabled = false;
    }
}
