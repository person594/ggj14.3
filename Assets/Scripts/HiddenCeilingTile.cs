using UnityEngine;
using System.Collections;

public class HiddenCeilingTile : BaseVisibilityObject
{
    public GameObject Child;

    public override void OnItemEnterRadius(Player player, Player.PlayerType type)
    {
        Child.SetActive(false);
        IsSolid = false;
    }

    public override void OnItemExitRadius(Player player, Player.PlayerType type)
    {
        Child.SetActive(true);
        IsSolid = true;
    }
}
