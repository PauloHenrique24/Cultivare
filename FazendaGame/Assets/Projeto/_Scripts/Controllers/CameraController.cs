using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject objUp;

    private PlayerController player;

    [Header("Colors")]
    public Color cortransparent;
    public Color corNormal;

    void Start()
    {
        player = FindFirstObjectByType<PlayerController>();    
    }

    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(player.transform.position, Vector3.forward);

        if (hit.collider && hit.collider.tag != "Player" && hit.collider.GetComponent<SpriteRenderer>()
            && hit.collider.GetComponent<SpriteRenderer>().sortingOrder > player.GetComponent<SpriteRenderer>().sortingOrder)
        {
            objUp = hit.collider.gameObject;
        }
        else
        {
            if (objUp != null)
            {
                objUp.GetComponent<SpriteRenderer>().color = corNormal;
                objUp = null;
            }
        }

        if (objUp != null && objUp.GetComponent<SpriteRenderer>())
        {
            objUp.GetComponent<SpriteRenderer>().color = cortransparent;
        }
    }
}
