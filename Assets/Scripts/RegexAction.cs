using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RegexAction : MonoBehaviour
{
    public float cooldownDuration;

    [HideInInspector] public int ID;
    [HideInInspector] public float remainingCooldown;
    [HideInInspector] public bool isOnCooldown
    {
        get
        {
            return remainingCooldown > 0f;
        }
    }

    public abstract void Activate();

    protected void Update()
    {
        if (remainingCooldown > 0f)
            remainingCooldown -= Time.deltaTime;
    }
}
