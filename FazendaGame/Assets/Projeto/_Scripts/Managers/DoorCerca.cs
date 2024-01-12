using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorCerca : MonoBehaviour
{
    private PlayerController player;
    private Button btnUse;

    private bool distance;
    private bool notdistance;

    private bool door;

    void Start()
    {
        player = FindFirstObjectByType<PlayerController>(); 
        btnUse = FindFirstObjectByType<BtnUse>().transform.GetChild(0).GetComponent<Button>();
    }

    void Update()
    {
        if (Vector2.Distance(transform.position, player.transform.position) < 0.8f)
        {
            if (!distance)
            {
                btnUse.onClick.RemoveAllListeners();
                btnUse.onClick.AddListener(OpenOrClose);
                btnUse.gameObject.SetActive(true);
                BtnUse.doorBtn = true;
                distance = true;
            }
            notdistance = false;
        }
        else
        {
            distance = false;
            if (!notdistance)
            {
                btnUse.onClick.RemoveAllListeners();
                btnUse.gameObject.SetActive(false);
                BtnUse.doorBtn = false;
                notdistance = true;
            }
        }
    }

    public void OpenOrClose()
    {
        door = !door;

        if (door)
        {
            GetComponent<Animator>().SetTrigger("open");
        }
        else
        {
            GetComponent<Animator>().SetTrigger("close");
        }
    }
}
