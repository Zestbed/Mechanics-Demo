using UnityEngine;

public class TetherRange : MonoBehaviour
{
    public bool playerIn;
    public Vector2 playerLoc;

    void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            if (playerIn) {}
            playerLoc = other.transform.position;
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            playerIn = true;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            playerIn = false;
        }
    }
}
