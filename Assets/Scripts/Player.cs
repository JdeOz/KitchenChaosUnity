using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour, IKitchenObjectParent {
    public static Player Instance { get; private set; }
    

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 25f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;

    private Vector3 lastDir = Vector3.zero;
    private bool isWalking;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;
    
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs {
        public BaseCounter selectedCounter;
    }

    private void Awake() {
        if (Instance != null) {
            Debug.LogError("Hay mas de una jugador");
        }

        Instance = this;
    }

    private void Start() {
        gameInput.OnInteractAction += GameInputOnInteractAction;
        gameInput.OnInteractAlternateAction += GameInputOnInteractAlternateAction;
    }

    private void GameInputOnInteractAction(object sender, EventArgs e) {
        if (selectedCounter != null) {
            selectedCounter.Interact(this);
        }
    }
    
    private void GameInputOnInteractAlternateAction(object sender, EventArgs e) {
        if (selectedCounter != null) {
            selectedCounter.InteractAlternate(this);
        }
    }

    private void Update() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        HandleMovement(inputVector);
        HandleInteractions(inputVector);
    }

    private void HandleInteractions(Vector2 inputVector) {
        if (inputVector != Vector2.zero) {
            lastDir = new Vector3(inputVector.x, 0f, inputVector.y);
            transform.forward = Vector3.Slerp(transform.forward, lastDir, Time.deltaTime * rotationSpeed);
        }

        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, lastDir, out RaycastHit raycastHit, interactDistance,
                countersLayerMask)) {
            if (raycastHit.transform.TryGetComponent(out BaseCounter baseCounter)) {
                if (baseCounter != selectedCounter) {
                    SetSelectedCounter(baseCounter);
                }
            }
            else {
                SetSelectedCounter(null);
            }
        }
        else {
            SetSelectedCounter(null);
        }
    }

    private void HandleMovement(Vector2 inputVector) {
        Vector3 moveX = new Vector3(inputVector.x, 0f, 0f);
        Vector3 moveZ = new Vector3(0f, 0f, inputVector.y);
        Vector3 moveDir = new Vector3(0f, 0f, 0f);

        float playerRadious = .6f;
        float playerHeight = .7f;
        float moveDistance = moveSpeed * Time.deltaTime;
        var position = transform.position;

        bool canMoveX = !Physics.CapsuleCast(position, position + Vector3.up * playerHeight,
            playerRadious, moveX, moveDistance);
        if (canMoveX) {
            moveDir += moveX;
        }

        bool canMoveZ = !Physics.CapsuleCast(position, position + Vector3.up * playerHeight,
            playerRadious, moveZ, moveDistance);
        if (canMoveZ) {
            moveDir += moveZ;
        }

        isWalking = moveDir != Vector3.zero;
        transform.position += moveDir * (moveSpeed * Time.deltaTime);
    }

    private void SetSelectedCounter(BaseCounter selectedCounter) {
        this.selectedCounter = selectedCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs {
            selectedCounter = selectedCounter
        });
    }
    
    public bool GetIsWalking() {
        return isWalking;
    }
    
    public Transform GetKitchenObjectFollowTransform() {
        return kitchenObjectHoldPoint;
    }

    // Interface functions
    public void SetKitchenObject(KitchenObject kitchenObject) {
        this.kitchenObject = kitchenObject;
    }

    public KitchenObject GetKitchenObject() {
        return kitchenObject;
    }

    public void ClearKitchenObject() {
        kitchenObject = null;
    }
    
    public bool HasKitchenObject() {
        return kitchenObject != null;
    }
}