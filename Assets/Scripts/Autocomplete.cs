using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Autocomplete : RegexAction
{
    public override void Activate()
    {
        if (isOnCooldown)
            return;

        GameManager gm = GameManager.instance;
        gm.AddBalance(gm.remainingString.Length * gm.charAmplifier);
        gm.remainingString = string.Empty;
        gm.FinishWord();

        remainingCooldown = cooldownDuration;
        AudioManager.instance.PlayAutocomplete();
    }
}
