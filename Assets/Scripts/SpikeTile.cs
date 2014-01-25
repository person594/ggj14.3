using UnityEngine;
using System.Collections;

public class SpikeTile : BaseVisibilityObject
{
    public override void OnItemEnterRadius(Player player, Player.PlayerType type)
    {
        IsSolid = false;
    }

    public override void OnItemExitRadius(Player player, Player.PlayerType type)
    {
        IsSolid = true;
    }
}
