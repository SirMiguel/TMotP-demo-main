using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaseController : MonoBehaviour, IForm
{
    public int id;

    private GameObject player;
    private PlayerController playerController;
    private PushableController pushableController;

    public Sprite baseSprite;
    public Sprite playerSprite;
    private SpriteRenderer spriteRenderer;

    private bool moveReady = true;
    private float moveTimer = 0f;
    private int moveCount = 0;
    private Vector3 lastMove;
    private bool fellPit = false;

    void Start()
    {
        this.enabled = false;
        player = GameObject.FindGameObjectWithTag("Player");
        pushableController = this.gameObject.GetComponent<PushableController>();
        playerController = player.GetComponent<PlayerController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (fellPit == true && Vector3.Distance(transform.position, playerController.moveDest) == 0f && moveCount > 0)
        {
            Object.Destroy(this.gameObject);
            return;
        }
        
        if (Vector3.Distance(transform.position, playerController.moveDest) == 0f && moveCount > 0)
        {
         Object.Destroy(this.gameObject);
         playerController.FormDestroyed();
        }

        player.transform.position = Vector3.MoveTowards(player.transform.position, playerController.moveDest, playerController.moveSpeed * Time.deltaTime * 1.5f);

        if (Vector3.Distance(transform.position, playerController.moveDest) == 0f)
        {
            if (moveTimer <= 0f) moveReady = true;
            else moveTimer -= Time.deltaTime;
        }

        if (moveReady)
        {
            if (Input.GetKeyDown("space"))
            {
                playerController.SwapOut();
            }
            else if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
            {
                lastMove = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
                MakeMove(lastMove);
            }
            else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
            {
                lastMove = new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
                MakeMove(lastMove);
            }
        }
    }

    void MakeMove(Vector3 dir)
    {
        Collider2D col;
        for (int i = 0; i < 100 && !Physics2D.OverlapCircle(playerController.moveDest + dir, .2f); i++)
        {
            playerController.moveDest += dir;
            moveTimer = playerController.moveDelay;
        }
        col = Physics2D.OverlapCircle(playerController.moveDest + dir, .2f);
        if (col.tag == "Pit")
        {
            playerController.moveDest += dir;
            moveTimer = playerController.moveDelay;
            fellPit = true;
        }
        moveCount++;
        moveReady = false;
    }

    public void Wake()
    {
        this.enabled = true;
        pushableController.enabled = false;
        UIManager.instance.setForm(id);
        spriteRenderer.sprite = playerSprite;
        moveReady = false;
        moveTimer = playerController.moveDelay;
        moveCount = 0;
        UIManager.instance.setStep(-1);
    }

    public void Sleep()
    {
        this.enabled = false;
        pushableController.enabled = true;
        spriteRenderer.sprite = baseSprite;
    }
}