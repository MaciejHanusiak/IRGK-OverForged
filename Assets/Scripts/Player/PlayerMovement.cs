using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Animator anim;
    [SerializeField] private GameInput gameInput;

    private const string ANIM_MOVE_X = "AnimMoveX";
    private const string ANIM_MOVE_Y = "AnimMoveY";
    private const string ANIM_MOVE_MAGNITUDE = "AnimMoveMagnitude";
    private const string ANIM_LAST_MOVE_X= "AnimLastMoveX";
    private const string ANIM_LAST_MOVE_Y= "AnimLastMoveY";


    private Vector2 moveDir;
    private Vector2 lastMoveDir;

    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        HandleInteractions();
    }

    void Update()
    {
        ProcessInputs();
        
        Animate();
    }
    private void FixedUpdate()
    {
        // Physics Calculations
        Move();
       
    }

    void ProcessInputs()
    {
        

        Vector2 inputVector = gameInput.GetMovementVectorNormalized();


        // Set player vector 
        moveDir = inputVector; 

        if (moveDir != Vector2.zero)
        {
            lastMoveDir = moveDir;
        }
    }

    void Move()
    {
        
        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = 0.2f;
        // check, do player hit any object circlecast
        bool canMove  = !Physics2D.CircleCast(transform.position, playerRadius, moveDir, moveDistance);

         
        if (!canMove)
        {
            // Cannot move towards moveDir

            // Attempt only X movement
            Vector2 moveDirX = new Vector2(moveDir.x, 0f).normalized;
            canMove = !Physics2D.CircleCast(transform.position, playerRadius, moveDirX, moveDistance);
            //canMove = !hit;
            if (canMove)
            {
                moveDir = moveDirX;
                Debug.Log("PlayerMov.cs 72/ moveDir:" + moveDir + " moveDirX:" + moveDirX);


            }
            else
            {
                // Cannot move towards moveDirX

                // Attempt  only Y movement
                Vector2 moveDirY = new Vector2(0f, moveDir.y).normalized;
                canMove = !Physics2D.CircleCast(transform.position, playerRadius, moveDirY, moveDistance);

                if (canMove)
                {
                    moveDir = moveDirY;
                    Debug.Log("PlayerMov.cs 72/ moveDir:" + moveDir + " moveDirY:" + moveDirY);
                }

            }
        }

        if (canMove)
        {
            // change player position in world
            transform.position += new Vector3(moveDir.x * moveDistance, moveDir.y * moveDistance, 0f);

        }
        
    }

    void HandleInteractions()
    {
        float interactDistance = 1f;
        Debug.DrawRay(transform.position, lastMoveDir, Color.red, 1f);
        Debug.DrawRay(transform.position, moveDir, Color.blue, 1f);

        // check, do player has object in front to interact
        RaycastHit2D hit = Physics2D.Raycast(transform.position, lastMoveDir, interactDistance);
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
        anim.SetFloat(ANIM_MOVE_X, moveDir.x);
        anim.SetFloat(ANIM_MOVE_Y, moveDir.y);
        anim.SetFloat(ANIM_MOVE_MAGNITUDE, moveDir.magnitude);
        anim.SetFloat(ANIM_LAST_MOVE_X,lastMoveDir.x);
        anim.SetFloat(ANIM_LAST_MOVE_Y,lastMoveDir.y);

        
    }
}
