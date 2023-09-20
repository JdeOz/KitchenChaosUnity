using System;
using UnityEngine;

public class CuttingCounter : BaseCounter {

    public event EventHandler<OnProgressChangedEventArgs> OnProgressChanged;

    public class OnProgressChangedEventArgs : EventArgs {
        public float progressNormalized;
    }
    
    
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    private int CuttingProgress;

    public override void Interact(Player player) {
        if (!HasKitchenObject()) {
            if (player.HasKitchenObject()) {
                if (GetRecipeForInput(player.GetKitchenObject().GetKitchenObjectSO())) {
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    CuttingProgress = 0;
                }
            }
        }
        else {
            if (!player.HasKitchenObject()) {
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

    public override void InteractAlternate(Player player) {
        if (HasKitchenObject()) {
            CuttingRecipeSO cuttingRecipeSO = GetRecipeForInput(GetKitchenObject().GetKitchenObjectSO());
            if (cuttingRecipeSO != null) {
                CuttingProgress++;
                OnProgressChanged?.Invoke(this,new OnProgressChangedEventArgs {
                    progressNormalized = (float) CuttingProgress / cuttingRecipeSO.cuttingProgressMax
                });
                if (CuttingProgress >= cuttingRecipeSO.cuttingProgressMax) {
                    GetKitchenObject().DestroySelf();
                    KitchenObject.SpawnKitchenObject(cuttingRecipeSO.output, this);
                }
            }
        }
    }

    private CuttingRecipeSO GetRecipeForInput(KitchenObjectSO inputKitchenObjectSO) {
        foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray) {
            if (cuttingRecipeSO.input == inputKitchenObjectSO) {
                return cuttingRecipeSO;
            }
        }

        return null;
    }
}