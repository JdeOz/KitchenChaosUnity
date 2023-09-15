using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounter : BaseCounter {
    // public event EventHandler OnPlayerGrabbedObject;
    public event EventHandler OnPlayerGrabbedObject;

    [SerializeField] private KitchenObjectSO kitchenObjectSO;
    [SerializeField] private GameObject spriteObject;

    private void Start() {
        SpriteRenderer spriteRenderer = spriteObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = kitchenObjectSO.sprite;
    }

    public override void Interact(Player player) {
        if (!player.HasKitchenObject()) {
            Transform kitchenObjectSoTransform = Instantiate(kitchenObjectSO.prefab);
            kitchenObjectSoTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(player);

            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
        }
    }
}