using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    //[SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator anim;

    private const string ANIM_MOVE_X = "AnimMoveX";
    private const string ANIM_MOVE_Y = "AnimMoveY";
    private const string ANIM_MOVE_MAGNITUDE = "AnimMoveMagnitude";
    private const string ANIM_LAST_MOVE_X= "AnimLastMoveX";
    private const string ANIM_LAST_MOVE_Y= "AnimLastMoveY";


    private Vector2 moveDirection;
    private Vector2 lastMoveDirection;
    
   
    void Update()
    {
        ProcessInputs();
        HandleInteractions();
        Animate();
    }
    private void FixedUpdate()
    {
        // Physics Calculations
        Move();
       
    }

    void ProcessInputs()
    {
        // Get player Input
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");


        // Set player last vector for Idle Animation
        if ((moveX == 0 && moveY == 0) && moveDirection.x != 0 || moveDirection.y != 0)
        {
            lastMoveDirection = moveDirection;
        }


        // Set player vector 
        moveDirection = new Vector2(moveX, moveY).normalized;  
    }

    void Move()
    {
        
        float moveDistance = moveSpeed * Time.deltaTime;

        // check, do player hit any object circlecast
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 0.5f, moveDirection, moveDistance);

        bool canMove = !hit;
        if (canMove)
        {
            // change player position in world
        transform.position += new Vector3(moveDirection.x * moveDistance, moveDirection.y * moveDistance, 0f);

        }
        
    }

    void HandleInteractions()
    {
        float interactDistance = 1f;
        Debug.DrawRay(transform.position, lastMoveDirection, Color.red, 1f);
        // check, do player has object in front to interact
        RaycastHit2D hit = Physics2D.Raycast(transform.position, lastMoveDirection, interactDistance);
        if (hit.collider != null) 
        {
             if (hit.transform.TryGetComponent(out ClearCounter clearCounter))
            {
                clearCounter.Interact();

            }
            
        }
        
    }
    void Animate()
    {
        
        // Set parameters in animator
        anim.SetFloat(ANIM_MOVE_X, moveDirection.x);
        anim.SetFloat(ANIM_MOVE_Y, moveDirection.y);
        anim.SetFloat(ANIM_MOVE_MAGNITUDE, moveDirection.magnitude);
        anim.SetFloat(ANIM_LAST_MOVE_X,lastMoveDirection.x);
        anim.SetFloat(ANIM_LAST_MOVE_Y,lastMoveDirection.y);

        
    }
}
