using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wildchar : RegexAction
{
    public override void Activate()
    {
        if (isOnCooldown)
            return;

        StartCoroutine(StartCooldown(cooldownDuration));
    }

    protected override IEnumerator StartCooldown(float duration)
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(duration);
        isOnCooldown = false;
    }
}
