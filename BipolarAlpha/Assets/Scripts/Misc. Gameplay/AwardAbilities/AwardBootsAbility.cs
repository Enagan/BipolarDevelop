﻿using UnityEngine;
using System.Collections;

/// <summary>
/// Awards the player with boots ability when collided with
///  Requires a Collision Volume with onTrigger enabled
/// </summary>
public class AwardBootsAbility : AwardPlayerAbility {

  /// <summary>
  ///  Called by parent when player collides, awards player the MagnetBoots left and right abilities
  /// </summary>
  public override void awardAbility(PlayerController player)
  {
      SMConsole.Log("Awarding Boots Ability", "Ability", SMLogType.NORMAL);

    player.AddAbility("Boots", new AbilityMagnetBoots(GameObject.Find("BootsMagnetism").GetComponent<MagnetBoots>(), player.getPlayerCamera(), player));
  }
}
