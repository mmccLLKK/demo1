using System;
using UnityEngine;

public class TriggerMsg : MonoBehaviour
{
    public Action<GameObject> onEntry;
    public Action<GameObject> onStay;
    public Action<GameObject> onLeave;

    private void OnTriggerEnter(Collider other)
    {
        var otherGameObject = other?.gameObject;
        if (!otherGameObject)
        {
            return;
        }

        onEntry?.Invoke(otherGameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        var otherGameObject = other?.gameObject;
        if (!otherGameObject)
        {
            return;
        }

        onLeave?.Invoke(otherGameObject);
    }

    protected void OnTriggerStay(Collider other)
    {
        var otherGameObject = other?.gameObject;
        if (!otherGameObject)
        {
            return;
        }

        onStay?.Invoke(otherGameObject);
    }
}