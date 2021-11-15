using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableController : MonoBehaviour
{
    private GameObject player;
    private PlayerController playerController;

    private Vector3 moveDest;
    private bool isMoving;
    private bool fellPit = false;

    void Start()
    {   
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        moveDest = transform.position;
        isMoving = false;
    }

    void Update()
    {
        if (fellPit && Vector3.Distance(transform.position, moveDest) == 0f)
        {
            Object.Destroy(this.gameObject);
            return;
        }
        isMoving = (Vector3.Distance(transform.position, moveDest) != 0f);
        if (isMoving) transform.position = Vector3.MoveTowards(transform.position, moveDest, playerController.moveSpeed * Time.deltaTime);
    }

    public bool TryMove(Vector3 dir)
    {
        if (isMoving) return false;
        Collider2D col = Physics2D.OverlapCircle(transform.position + dir, .2f);
        if(Physics2D.OverlapCircle(transform.position + dir, .2f) && col.tag == "Pit")
        {
            fellPit = true;
            moveDest += dir;
            return true;
        } 
        if (col) return false;
        moveDest += dir;
        return true;
    }
}
