using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform player1;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float speed;


    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.isMultiplayerSelected = true)
        {
            MultiplayerCamera();
        }
        else
        {
            Vector3 desiredPos = player.position + offset;
            transform.position = Vector3.Lerp(transform.position, desiredPos, speed * Time.deltaTime);

        }

    }
    private Vector3 GetMidPoint()
    {
        return (player.position + player1.position) * 0.5f;
    }
    void MultiplayerCamera()
    {
        if (player == null || player1 == null)
            return;

        Vector3 desiredPos = GetMidPoint() + offset;
        transform.position = Vector3.Lerp(
            transform.position,
            desiredPos,
            speed * Time.deltaTime);
    }
}
