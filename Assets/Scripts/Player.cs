using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public enum Direction
    {
        None, Up, Down, Left, Right
    }

    public enum PlayerType
    {
        Ghost, Human
    }

    public PlayerType Type;
    public float MoveSpeed = 5f;
    public float CellSnapAmount = 0.1f;
    public int DiscoveryRadius = 3;

    private Direction _curDir;
    private tk2dSpriteAnimator _animator;
    private bool _isMoving;
    private Vector3 _destPosition;
    private tk2dTileMap _tileMap;

    void Start()
    {
        _animator = (tk2dSpriteAnimator) GetComponentInChildren(typeof(tk2dSpriteAnimator));
        _tileMap = FindObjectOfType<tk2dTileMap>();
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, DiscoveryRadius);
    }
    
    public void Update()
    {
        if (!_isMoving)
        {
            //Poll the player for the direction to move in
            float horzInput = 0f;
            float vertInput = 0f;
            switch (Type)
            {
                case PlayerType.Ghost:
                    horzInput = Input.GetAxis("Player1 Horizontal");
                    vertInput = Input.GetAxis("Player1 Vertical");
                    break;
                case PlayerType.Human:
                    horzInput = Input.GetAxis("Player2 Horizontal");
                    vertInput = Input.GetAxis("Player2 Vertical");
                    break;
            }

            _curDir = Direction.None;
            if (horzInput < -0.1f)
                _curDir = Direction.Left;
            else if (horzInput > 0.1f)
                _curDir = Direction.Right;
            else if (vertInput < -0.1f)
                _curDir = Direction.Down;
            else if (vertInput > 0.1f)
                _curDir = Direction.Up;

            if (_curDir != Direction.None)
            {
                //Check if the space is clear before we move into it
                Vector3 destPos = transform.position + GetOffsetFromMoveDirection();
                bool isSpaceSolid = IsTileSolid(Mathf.RoundToInt(destPos.x), Mathf.RoundToInt(destPos.y));
                if (isSpaceSolid)
                {
                    //Early out if it's a wall
                    return;
                }

                _isMoving = true;
                _destPosition = transform.position + GetOffsetFromMoveDirection();
                UpdateAnimation();
            }
        }
        else
        {
            //We're already moving so we're going to just keep going to our destination.
            Vector3 curPos = transform.position;

            //Direction we're traveling in
            Vector3 moveDir = GetOffsetFromMoveDirection();

            curPos += moveDir * MoveSpeed * Time.deltaTime;

            if (Vector3.Distance(curPos, _destPosition) < CellSnapAmount)
            {
                transform.position = _destPosition;
                _isMoving = false;
                _curDir = Direction.None;

                //We're going to cheat and get every object of type in the map and then
                //check their radius and then fire off a OnOverlap/OnNotOverlap.
                BaseVisibilityObject[] visibilityObjects = (BaseVisibilityObject[])FindObjectsOfType(typeof(BaseVisibilityObject));
                foreach (BaseVisibilityObject item in visibilityObjects)
                {
                    //Check the distance to the object
                    float dist = Vector3.Distance(_destPosition, item.transform.position);
                    if (dist < DiscoveryRadius)
                    {
                        item.ItemEnterRadius(this, Type);
                    }
                }
            }
            else
            {
                transform.position = curPos;
            }
        }
    }

    private bool IsTileSolid(int tileX, int tileY)
    {
        bool bWall = _tileMap.GetTile(tileX, tileY, 0) == 2;
        if (bWall)
            return true;

        //If it's not a wall there could still be a changing visibility object
        //So we'll get a list of all of our BaseVisibilityObject and check if
        //if they're in this position either and if they're solid.
        BaseVisibilityObject[] visibilityObjects = (BaseVisibilityObject[])FindObjectsOfType(typeof(BaseVisibilityObject));
        foreach (BaseVisibilityObject obj in visibilityObjects)
        {
            Vector3 objPos = obj.transform.position;
            int newTileX = Mathf.RoundToInt(objPos.x);
            int newTileY = Mathf.RoundToInt(objPos.y);

            if (newTileX == tileX && newTileY == tileY)
            {
                Debug.Log("objSolid to Friendly: " + obj.NonSolidToFriendly);
                if (obj.NonSolidToFriendly)
                {
                    Debug.Log("obj.Friendly: " + obj.FriendlyType + " Type: " + Type);
                    if(obj.FriendlyType == Type)
                        return false;
                }
                else
                {
                    return obj.IsSolid;
                }
            }
        }

        return false;
    }

    private Vector3 GetOffsetFromMoveDirection()
    {
        Vector3 offset = Vector3.zero;
        switch (_curDir)
        {
            case Direction.Left:
                offset = Vector3.left;
                break;
            case Direction.Right:
                offset = Vector3.right;
                break;
            case Direction.Up:
                offset = Vector3.up;
                break;
            case Direction.Down:
                offset = Vector3.down;
                break;
        }

        return offset;
    }

    private void UpdateAnimation()
    {
        switch (_curDir)
        {
            case Direction.Left:
                _animator.Play("walk_left");
                break;
            case Direction.Right:
                _animator.Play("walk_right");
                break;
            case Direction.Up:
                _animator.Play("walk_up");
                break;
            case Direction.Down:
                _animator.Play("walk_down");
                break;
        }
    }
}
