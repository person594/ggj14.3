using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
	public float MoveSpeed;
	public tk2dTileMap map;
	private bool ismoving = false;
	private int x, y, destX, destY;
	private float movementProgress;
	private Direction facing = Direction.NORTH;

	public enum Direction{
		NORTH, EAST, SOUTH, WEST, NONE
	}

	//private float destx, desty;

	//public enum Direction {left, right, up, down, none};
	//public Direction direction = Direction.none;

	// Use this for initialization
	void Start () {
//		controller = GetComponent<CharacterController> ();
//		Debug.Log (controller);
		x = Mathf.RoundToInt(transform.position.x);
		y = Mathf.RoundToInt(transform.position.y);
		destX = x;
		destY = y;



	}
	
	// Update is called once per frame
	void Update ()
	{
		MoveSpeed = 5 * Time.deltaTime;

		if (ismoving) {
			movementProgress += MoveSpeed;
			if(movementProgress >= 1){
				x = destX;
				y = destY;
				if (getJoystickDir() != Direction.NONE){
					movementProgress -= 1;
					move(getJoystickDir());
				} else {
					movementProgress = 0;
					ismoving = false;
				}

			}
		}else{
			move(getJoystickDir());/*
			if (Mathf.Abs(verticalMovement)> Mathf.Abs(horizontalMovement) ){
				if (verticalMovement > 0){
					move(Direction.NORTH);
				} else move(Direction.SOUTH);
			} else if (Mathf.Abs(verticalMovement) < Mathf.Abs(horizontalMovement) ){
				if (horizontalMovement > 0){
					move(Direction.EAST);
				} else move(Direction.WEST);
			}*/
		}

		float xPos = ((float) destX) * movementProgress + ((float) x) * (1 - movementProgress);
		float yPos = ((float) destY) * movementProgress + ((float) y) * (1 - movementProgress);
		transform.position = new Vector3(xPos, yPos, 0);

		Debug.Log(movementProgress);

	}

	void move(Direction d){
		facing = d;
		switch(d){
		case Direction.NORTH : destY = y + 1; break;
		case Direction.EAST : destX = x + 1; break;
		case Direction.SOUTH : destY = y - 1; break;
		case Direction.WEST : destX = x - 1; break;
		}
		movementProgress = MoveSpeed;
		ismoving = d != Direction.NONE;
	}

	Direction getJoystickDir(){
		float horizontalMovement = Input.GetAxis ("Horizontal");
		float verticalMovement = Input.GetAxis ("Vertical");

		if (horizontalMovement == 0 && verticalMovement == 0) return Direction.NONE;

		if (Mathf.Abs(verticalMovement) > Mathf.Abs(horizontalMovement)){
			if (verticalMovement > 0){
				return Direction.NORTH;
			} else {
				return Direction.SOUTH;
			}
		} else {
			if (horizontalMovement > 0){
				return Direction.EAST;
			} else {
				return Direction.WEST;
			}
		}
	}

	bool isWall(float x, float y){
		if (map.GetTile (Mathf.RoundToInt(x), Mathf.RoundToInt(y), 0) == 2) {
			return true;
		} else {
			return false;	
		}
	}
}
