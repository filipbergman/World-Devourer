using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private PlayerInput playerInput;
    [SerializeField]
    private PlayerMovement playerMovement;

    private Transform backpackUI;
    private float UIleftEdge;
    private float UIrightEdge;

    public float interactionRayLength = 5;

    public LayerMask groundMask;
    public bool fly;

    public Animator animator;

    bool isWaiting = false;

    public World world;

    public InventoryHandler inventoryHandler;

    private void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
        playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();
        world = FindObjectOfType<World>();
        inventoryHandler = FindObjectOfType<InventoryHandler>();
        backpackUI = transform.Find("/InventoryUI/BottomUI");
        float UIWidth = backpackUI.transform.GetComponent<RectTransform>().rect.width;

        UIleftEdge = backpackUI.position.x - (UIWidth / 2);
        UIrightEdge = backpackUI.position.x + (UIWidth / 2);

    }

    private void Start()
    {
        playerInput.OnMouseClick += HandleMouseClick;
        playerInput.OnMouseRightClick += HandleMouseRightClick;
        playerInput.OnFly += HandleFlyClick;
        playerInput.OnKeyButtonClick += HandleNumberClick;
        playerInput.OnScrollInput += HandleScrollInput;
        playerInput.OnInventory += HandleInventoryInput;
    }

    private void HandleInventoryInput(KeyCode keyCode)
    {
        if(keyCode == KeyCode.E)
        {
            //FindObjectOfType<CinemachineVirtualCamera>().enabled = !playerInput.backPackOpen;
            //FindObjectOfType<CinemachineBrain>().enabled = !playerInput.backPackOpen;
            Cursor.visible = playerInput.backPackOpen;
            Cursor.lockState = CursorLockMode.Confined;
            inventoryHandler.ToggleInventory();
        }
        if(keyCode == KeyCode.Q)
        {
            // TODO: drop one item of current item istead:
            inventoryHandler.DropOneItem();
        }
    }

    private void HandleScrollInput(float val)
    {
        inventoryHandler.ScrollWheelChangeCurrentItem(val);
    }

    private void HandleNumberClick(int number)
    {
        inventoryHandler.SetCurrentItem(number);
    }

    private void HandleFlyClick()
    {
        fly = !fly;
    }

    private void Update()
    {
        
        if(fly)
        {
            animator.SetFloat("speed", 0);
            animator.SetBool("isGrounded", false);
            animator.ResetTrigger("jump"); 
            playerMovement.Fly(playerInput.MovementInput, playerInput.IsJumping, playerInput.RunningPressed);
        }
        else
        {
            animator.SetBool("isGrounded", playerMovement.IsGrounded);
            if(playerMovement.IsGrounded && playerInput.IsJumping && isWaiting == false)
            {
                animator.SetTrigger("jump");
                isWaiting = true;
                StopAllCoroutines();
                StartCoroutine(ResetWaiting());
            }
            animator.SetFloat("speed", playerInput.MovementInput.magnitude);
            playerMovement.HandleGravity(playerInput.IsJumping);
            if (playerInput.backPackOpen == true)
            {
                return;
            }
            playerMovement.Walk(playerInput.MovementInput, playerInput.RunningPressed);
        }
    }

    IEnumerator ResetWaiting()
    {
        yield return new WaitForSeconds(0.1f);
        animator.ResetTrigger("jump");
        isWaiting = false;
    }



    private void HandleMouseClick(Vector2 mousePos)
    {
        if(playerInput.backPackOpen == false)
        {
            Ray playerRay = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(playerRay, out hit, interactionRayLength, groundMask))
            {
                ModifyTerrain(hit);
            }
        } else if(inventoryHandler.HoldingItem())
        {
            // TODO: Drop only if click is outside backpack:
            Debug.Log("MOUSE POS: " + mousePos);
            if(mousePos.x < UIleftEdge || mousePos.x > UIrightEdge)
            {
                inventoryHandler.DropAllItems();
            }
        }
    }

    private void HandleMouseRightClick()
    {
        Ray playerRay = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(playerRay, out hit, interactionRayLength, groundMask))
        {
            BlockType block = inventoryHandler.GetCurrentBlock();
            if (block != BlockType.Nothing)
            {
                AddBlockToTerrain(hit, block);
            }
            
        }
    }

    private void ModifyTerrain(RaycastHit hit, BlockType block = BlockType.Air)
    {
        bool blockSet = world.SetBlock(hit, block);
    }

    private void AddBlockToTerrain(RaycastHit hit, BlockType block)
    {
        bool blockSet = world.SetBlock(hit, block);
        if(blockSet == true)
        {
            inventoryHandler.ChangeInventorySlotAmount(-1);
        }
    }


}
