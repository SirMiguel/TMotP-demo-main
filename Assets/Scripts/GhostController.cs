using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour, IForm
{
    public int id;

    private GameObject player;
    private PlayerController playerController;

    public int maxMove;
    private bool moveReady = true;
    private float moveTimer = 0f;
    private int moveCount = 0;

    void Start()
    {
        player = transform.parent.gameObject;
        playerController = player.GetComponent<PlayerController>();
    }

    void Update()
    {
        player.transform.position = Vector3.MoveTowards(player.transform.position, playerController.moveDest, playerController.moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, playerController.moveDest) == 0f)
        {
            if (moveTimer <= 0f) moveReady = true;
            else moveTimer -= Time.deltaTime;
        }

        if (moveReady)
        {
            if (Input.GetKeyDown("space"))
            {
                playerController.SwapIn();
            }
            else if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
            {
                MakeMove(new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f));
            }
            else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
            {
                MakeMove(new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f));
            }
        }
    }

    void MakeMove(Vector3 dir)
    {
        if (Physics2D.OverlapCircle(playerController.moveDest + dir, .2f, playerController.stopsMove)) return;
        if (moveCount >= maxMove)
        {
            Object.Destroy(this.gameObject);
            playerController.GameOver();
        }
        else
        {
            playerController.moveDest += dir;
            moveReady = false;
            moveTimer = playerController.moveDelay;
            SetMoveCount(moveCount + 1);
        }
    }

    void SetMoveCount(int count)
    {
        moveCount = count;
        UIManager.instance.setStep(maxMove - count);
    }

    public void Wake()
    {
        this.enabled = true;
        UIManager.instance.setForm(id);
        moveReady = false;
        moveTimer = playerController.moveDelay * 3;
        SetMoveCount(0);
    }

    public void Sleep()
    {
        this.enabled = false;
        moveCount = 0;
    }
}
