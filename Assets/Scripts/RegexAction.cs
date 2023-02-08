using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RegexAction : MonoBehaviour
{
    public float cooldownDuration;

    [HideInInspector] public int ID;
    [HideInInspector] public bool isOnCooldown;

    public abstract void Activate();

    protected abstract IEnumerator StartCooldown(float duration);
}
