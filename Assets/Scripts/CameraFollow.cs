using Unity.VisualScripting;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    [SerializeField]
    private Vector2 hardFollow = new Vector2(5f, 3f);
    [SerializeField]
    private Vector2 mapSize = new Vector2(10f, 5f);
    public GameObject player;
    private Transform playerTrans;
    public float easing = 0.05f;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (player != null) {
            playerTrans = player.transform;
            transform.position = playerTrans.position;
        } else Debug.LogError("Player Reference on Camera is null!");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerTrans != null) {
            Vector3 destination = playerTrans.position;
            destination = Vector3.Lerp(transform.position, destination, easing);
            destination.z = -10f;
            transform.position = destination;
        }
    }
}
