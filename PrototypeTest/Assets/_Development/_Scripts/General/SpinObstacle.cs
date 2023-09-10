using DG.Tweening;
using UnityEngine;

public class SpinObstacle : MonoBehaviour
{
    void Start()
    {
        transform.DORotate(new(0, 360, 0), 1, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);
    }
}
