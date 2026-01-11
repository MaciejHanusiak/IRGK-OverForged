using System;
using UnityEngine;

public class Player : MonoBehaviour, ISmithObjectParent
{
    public static Player Instance { get; private set; }

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs 
    {
        public BaseCounter selectedCounter;
    } 

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Animator animator;
    [SerializeField] private Animator hitAnimator;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private Transform smithObjectHoldPoint;
    [SerializeField] private SmithObjectSO objectToActivateWeaponStandUI;

    [SerializeField] private ParticleSystem interactAlternateAnvilVfxPrefab;
    [SerializeField] private Vector3 interactAlternateAnvilVfxOffset = Vector3.zero;

    [SerializeField] private Transform animationTransform;
    [SerializeField] private SpriteRenderer animationSpriteRenderer;
    [SerializeField] private SpriteRenderer playerSpriteRenderer;

    // name of Parameters in "PlayerController" Animator 
    private const string ANIM_MOVE_X = "AnimMoveX";
    private const string ANIM_MOVE_Y = "AnimMoveY";
    private const string ANIM_MOVE_MAGNITUDE = "AnimMoveMagnitude";
    private const string ANIM_LAST_MOVE_X= "AnimLastMoveX";
    private const string ANIM_LAST_MOVE_Y= "AnimLastMoveY";


    private Vector2 moveDir;
    private Vector2 lastMoveDir;
    private Vector3 baseLocalPos;
    private BaseCounter selectedCounter;
    private SmithObject smithObject;

    private enum LookDirection
    {
        Up,
        Down,
        Left,
        Right
    }
    private LookDirection lookDir;

    void UpdateLookDirection()
    {
        if (Mathf.Abs(lastMoveDir.x) > Mathf.Abs(lastMoveDir.y))
        {
            lookDir = lastMoveDir.x > 0 ? LookDirection.Right : LookDirection.Left;
        }
        else
        {
            lookDir = lastMoveDir.y > 0 ? LookDirection.Up : LookDirection.Down;
        }
    }

    private void Awake()
    {
        // Singleton Pattern
        if (Instance != null)
        {
            Debug.LogError("There is more then one Player instance");
        }
            Instance = this;
        baseLocalPos = animationTransform.localPosition;

    }
    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }


    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        // input Event
        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
    }
    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
    {
        // input Event
        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);

            SpawnInteractAlternateVfx();

            if(selectedCounter is KnifesBenchCounter || selectedCounter is  AnvilCounter)
            hitAnimator.SetTrigger("InteractAlternate");
            

        }
    }

    void Update()
    {
        ProcessInputs();
        UpdateLookDirection();
        Animate();
        HandleInteractions();
        Move();

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

            
            
            if (hit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                // Object has ClearCounter
               

                if (baseCounter != selectedCounter)
                { 
                    SetSelectedCounter(baseCounter);
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

    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs {
            selectedCounter = selectedCounter
        });
    }
    void Animate()
    {
     
        

        // Set parameters in animator
        animator.SetFloat(ANIM_MOVE_X, moveDir.x);
        animator.SetFloat(ANIM_MOVE_Y, moveDir.y);
        animator.SetFloat(ANIM_MOVE_MAGNITUDE, moveDir.magnitude);
        animator.SetFloat(ANIM_LAST_MOVE_X,lastMoveDir.x);
        animator.SetFloat(ANIM_LAST_MOVE_Y,lastMoveDir.y);
        UpdateAnimationRotation();
        UpdateAnimationSortingOrder();


        
    }
    private void SpawnInteractAlternateVfx()
    {
        if (interactAlternateAnvilVfxPrefab == null) return;
        if (selectedCounter == null) return;

        Vector3 pos = selectedCounter.transform.position + interactAlternateAnvilVfxOffset;
        Instantiate(interactAlternateAnvilVfxPrefab, pos,Quaternion.Euler(-90f, 0f, 0f));
    }
    void UpdateAnimationRotation()
    {
        float zRot = lookDir switch
        {
            LookDirection.Right => -90f,
            LookDirection.Up => 0f,
            LookDirection.Left => 90f,
            LookDirection.Down => 180f,
            _ => 0f
        };
        Vector3 animationPosition = lookDir switch
        {
            LookDirection.Right => new Vector3(0.5f, 0f, 0f),
            LookDirection.Up => new Vector3(0f, 0.5f, 0f),
            LookDirection.Left => new Vector3(-0.5f, 0f, 0f),
            LookDirection.Down => new Vector3(0f, -0.5f, 0f),
            _ => new Vector3(0f, 0f, 0f),
        };
        animationTransform.localRotation = Quaternion.Euler(0f, 0f, zRot);
        
        animationTransform.localPosition = baseLocalPos + animationPosition;
    }

    void UpdateAnimationSortingOrder()
    {
        if (lookDir == LookDirection.Up)
        {
            animationSpriteRenderer.sortingOrder =
                playerSpriteRenderer.sortingOrder - 1;
        }
        else
        {
            animationSpriteRenderer.sortingOrder = 
                playerSpriteRenderer.sortingOrder + 1;
        }
    }

    public Transform GetSmithObjectFollowTransform()
    {
        return smithObjectHoldPoint;
    }
    public void SetSmithObject(SmithObject smithObject)
    {
        this.smithObject = smithObject;
    }
    public SmithObject GetSmithObject()
    {
        return smithObject;
    }
    public void ClearSmithObject()
    {
        smithObject = null;
    }
    public bool HasSmithObject()
    {
        return smithObject != null;
    }
    public string GetObjectParentName()
    {
        return this.name;
    }
    public virtual SmithObjectSO GetOutputSmithObjectSO() => null;
}
