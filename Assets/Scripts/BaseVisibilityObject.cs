using UnityEngine;
using System.Collections;

public class BaseVisibilityObject : MonoBehaviour
{
    public Player.PlayerType FriendlyType;
    public bool IsSolid;
    public bool NonSolidToFriendly;

    protected Player TrackedPlayer;


    void Update()
    {
        //If we're tracking a player then we need to track when they exit the radius too.
        if(TrackedPlayer != null)
        {
            float dist = Vector3.Distance(transform.position, TrackedPlayer.transform.position);
            if (dist > TrackedPlayer.DiscoveryRadius)
            {
                ItemExitRadius(TrackedPlayer, TrackedPlayer.Type);
                TrackedPlayer = null;
            }
        }
    }

    public void ItemEnterRadius(Player player, Player.PlayerType type)
    {
        if(type == FriendlyType)
        {
            Debug.Log("OnItemEnterRadi");
            TrackedPlayer = player;
            OnItemEnterRadius(player, type);
        }
    }

    public void ItemExitRadius(Player player, Player.PlayerType type)
    {
        if (type == FriendlyType)
        {
            Debug.Log("OnItemExitRadi");
            OnItemExitRadius(player, type);
        }

    }

    public virtual void OnItemEnterRadius(Player player, Player.PlayerType type)
    {

    }

    public virtual void OnItemExitRadius(Player player, Player.PlayerType type)
    {

    }
}
