using System;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public Action triggered;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Mob"))
            triggered?.Invoke();
    }
}
