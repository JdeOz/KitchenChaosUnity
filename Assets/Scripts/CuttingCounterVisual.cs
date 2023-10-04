using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounterVisual : MonoBehaviour {

    private const string CUT = "Cut";

    [SerializeField] private CuttingCounter cuttingCounter;

    private Animator animator;
    
    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void Start() {
        cuttingCounter.OnProgressChanged += CuttingCounter_OnCut;
    }

    private void CuttingCounter_OnCut(object sender, CuttingCounter.OnProgressChangedEventArgs e) {
        animator.SetTrigger(CUT);
    }
}