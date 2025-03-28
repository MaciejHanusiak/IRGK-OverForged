using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Vector2 dir;
    [SerializeField] float moveSpeed = 5f;
    
    void Start()
    {
        
    }

    
    void Update()
    {
        dir.x = Input.GetAxisRaw("Horizontal");
        dir.y = Input.GetAxisRaw("Vertical");
        dir.Normalize();
        transform.Translate(Time.deltaTime * dir * moveSpeed);
    }
}
