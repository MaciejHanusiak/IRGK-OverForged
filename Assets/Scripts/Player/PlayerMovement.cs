using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Vector2 dir;
    [SerializeField] float moveSpeed = 5f;
    Animator animator;

    float moveX, moveY;

    float moveStance;
    int direction;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        
    }

    
    void Update()
    {
        // Get player Input
        moveX = Input.GetAxisRaw("Horizontal");
        moveY = Input.GetAxisRaw("Vertical");
        
        // set player vector
        dir.x = moveX;
        dir.y = moveY;
        dir.Normalize();

        moveStance = Mathf.Abs(dir.x) + Mathf.Abs(dir.y);
        animator.SetFloat("SpeedFloat", moveStance);



        if (dir.y < 0)      // Down
                direction = 0;
        else if (dir.x < 0) // Left
                direction = 1;
        else if (dir.y > 0) // Up
                direction = 2;
        else if (dir.x > 0) // Right
                direction = 3;

        animator.SetInteger("DirectionInt", direction);

        transform.Translate(Time.deltaTime * dir * moveSpeed);
        Debug.Log("MoveStance:" + moveStance + " Direction:" + direction + " " +  dir);
    }
}
