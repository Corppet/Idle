using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Autofill : RegexAction
{
    public override void Activate()
    {
        if (isOnCooldown)
            return;

        GameManager gm = GameManager.instance;

        if (gm.remainingString.Length == 0)
        {
            gm.OnCompleteWord.Invoke();
        }
        else
        {
            gm.AddBalance(gm.charAmplifier);
            gm.completedString += gm.remainingString[0];
            gm.remainingString = gm.remainingString.Substring(1);
            gm.UpdateText();
        }

        remainingCooldown = cooldownDuration;
    }
}
