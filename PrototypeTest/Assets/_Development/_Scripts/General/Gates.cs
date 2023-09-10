using TMPro;
using UnityEngine;

public class Gates : MonoBehaviour
{
    [SerializeField] int leftValue;
    [SerializeField] int rightValue;

    [SerializeField] bool leftMultiply;
    [SerializeField] bool rightMultiply;

    [SerializeField] Gate leftGate;
    [SerializeField] Gate rightGate;

    [SerializeField] TextMeshPro rightTMP;
    [SerializeField] TextMeshPro leftTMP;

    private bool triggered;

    private void Start()
    {
        SetGates();
    }

    private void OnEnable()
    {
        leftGate.triggered += LeftGate;
        rightGate.triggered += RightGate;
    }

    private void OnDisable()
    {
        leftGate.triggered -= LeftGate;
        rightGate.triggered -= RightGate;
    }

    void SetGates()
    {
        if (rightMultiply)
            rightTMP.text = "x" + rightValue;
        else
            rightTMP.text = "+" + rightValue;

        if (leftMultiply)
            leftTMP.text = "x" + leftValue;
        else
            leftTMP.text = "+" + leftValue;
    }

    void LeftGate()
    {
        if (triggered) return;
        triggered = true;
        PlayerManager.Instance.SpawnMob(leftValue, leftMultiply);
        leftGate.gameObject.SetActive(false);
    }

    void RightGate()
    {
        if (triggered) return;
        triggered = true;
        PlayerManager.Instance.SpawnMob(rightValue, rightMultiply);
        rightGate.gameObject.SetActive(false);
    }
}
