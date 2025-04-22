using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs 
    {
        public ClearCounter selectedCounter;
    } 

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Animator anim;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;


    // name of Parameters in "PlayerController" Animator 
    private const string ANIM_MOVE_X = "AnimMoveX";
    private const string ANIM_MOVE_Y = "AnimMoveY";
    private const string ANIM_MOVE_MAGNITUDE = "AnimMoveMagnitude";
    private const string ANIM_LAST_MOVE_X= "AnimLastMoveX";
    private const string ANIM_LAST_MOVE_Y= "AnimLastMoveY";


    private Vector2 moveDir;
    private Vector2 lastMoveDir;
    private ClearCounter selectedCounter;

    private void Awake()
    {
        // Singleton Pattern
        if (Instance != null)
        {
            Debug.LogError("There is more then one Player instance");
        }
            Instance = this;

    }
    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        // input Event
        if (selectedCounter != null)
        {
            selectedCounter.Interact();
        }
    }

    void Update()
    {
        ProcessInputs();
        Animate();
        HandleInteractions();
        Move();
    }
    private void FixedUpdate()
    {
        // Physics Calculations
    }

    void ProcessInputs()
    {
        
        // get input vector from GameInput.cs
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();


        // Set player vector 
        moveDir = inputVector; 

        // Set last move dir for idle animation state
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
        Debug.DrawRay(transform.position, lastMoveDir * interactDistance, Color.red, 1f);
        Debug.DrawRay(transform.position, moveDir * interactDistance, Color.blue, 1f);

        // check, do player has object in front to interact
        RaycastHit2D hit = Physics2D.Raycast(transform.position, lastMoveDir, interactDistance, countersLayerMask);
        if (hit.collider != null)
        { //hit any object

            
            
            if (hit.transform.TryGetComponent(out ClearCounter clearCounter))
            {
                // Object has ClearCounter
               

                if (clearCounter != selectedCounter)
                { // 

                    SetSelectedCounter(clearCounter);
                }
                
            }
            else
            {
                
                // hit any object
                SetSelectedCounter(null);
            }

        }
        else
        {  // any object was not hitted     
            SetSelectedCounter(null);
        }

    }

    private void SetSelectedCounter(ClearCounter selectedCounter)
    {
        //if (this.selectedCounter == selectedCounter) return; // <-- to linia ratuj¹ca ¿ycie

        this.selectedCounter = selectedCounter;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs {
            selectedCounter = selectedCounter
        });
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
