using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour {
    
    [SerializeField] private KitchenObjectSO KitchenObjectSO;
    
    private IKitchenObjectParent kitchenObjectParent;
    
    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent) {
        if (this.kitchenObjectParent != null) {
            this.kitchenObjectParent.ClearKitchenObject();
        }
        this.kitchenObjectParent = kitchenObjectParent;
        if (kitchenObjectParent.HasKitchenObject()) {
            Debug.LogError("counter already has KitchenObject");
        }
        
        kitchenObjectParent.SetKitchenObject(this);
        
        transform.parent = this.kitchenObjectParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero;

    }

    public void SliceKitchenObject() {
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);
    }
    
    // public KitchenObjectSO GetKitchenObjectSO() {
    //     return KitchenObjectSO;
    // }

    // public IKitchenObjectParent GetKitchenObjectParent() {
    //     return kitchenObjectParent;
    // }
}