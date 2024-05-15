using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Tether : MonoBehaviour
{
    private TetherRange tetherRange;
    private LineRenderer lRend;
    public GameObject lineObject;


    void Awake() {
        tetherRange = GetComponentInChildren<TetherRange>();
    }

    void Update() {
        if (tetherRange.playerIn) {
            Vector3[] positions = new Vector3[2];
            positions[0] = new Vector3(transform.position.x, transform.position.y, 0);
            positions[1] = new Vector3(tetherRange.playerLoc.x, tetherRange.playerLoc.y, 0);
            if (lRend == null) {
                GameObject go = Instantiate(lineObject);
                lRend = go.GetComponent<LineRenderer>();
                lRend.SetPositions(positions);
            } 
            lRend.SetPositions(positions);
        }
        if (!tetherRange.playerIn && lRend != null) {
            Destroy(lRend.gameObject);
        }


    }

}
