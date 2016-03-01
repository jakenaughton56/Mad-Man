using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class Controller2D : MonoBehaviour {

	public LayerMask collisionMask;
	public LayerMask bottomLevel;

	const float skinWidth = .015f; // how deep into the player object the rays start
	const int minRays = 2;

	public int horizontalRayCount = 4;
	public int verticalRayCount = 4;

	float horizontalRaySpacing; // spacing between rays
	float verticalRaySpacing;

	BoxCollider2D collider;
	RaycastOrigins raycastOrigins;
	public CollisionInfo collisions;


	void Start(){
		collider = GetComponent<BoxCollider2D> ();
		CalculateRaySpacing ();
	}

	// called by player to move player object
	public void Move(Vector3 velocity, bool downbutton){
		// Update corner ray positions
		UpdateRaycastOrigins ();

		// reset collisions to none
		collisions.Reset ();

		// check for collisions (only if player is moving in that direction)
		if (velocity.x != 0) {
			HorizontalCollisions (ref velocity);
		}
		if (velocity.y != 0) {
			VerticalCollisions (ref velocity,downbutton);
		}

		// move player
		transform.Translate (velocity);
	}


	// checks for horizontal collisions with game level
	void HorizontalCollisions(ref Vector3 velocity){
		float directionX = Mathf.Sign (velocity.x); // check if player is moving up or down
		float rayLength = Mathf.Abs (velocity.x) + skinWidth; // distance player is to move in the given frame
		
		// run a loop through all rays to check for a collision
		for(int i = 0; i<horizontalRayCount; i++){
			// check if we are moving up or down
			Vector2 rayOrigin = (directionX == -1)?raycastOrigins.bottomLeft:raycastOrigins.bottomRight;
			rayOrigin += Vector2.up * horizontalRaySpacing * i; // calculate new ray origin
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right*directionX, rayLength, collisionMask);
			// display rays on screen
			// Debug.DrawRay(rayOrigin, Vector2.right*directionX*rayLength, Color.red);
			if(hit){
				velocity.x = (hit.distance - skinWidth) * directionX; // the length from player object to level object;
				rayLength = hit.distance;

				// update collision structure
				collisions.left = directionX == -1;
				collisions.right = directionX == 1;
			}
		}
	}



	// checks for vertical collisions with game level
	void VerticalCollisions(ref Vector3 velocity,bool downbutton){
		float directionY = Mathf.Sign (velocity.y); // check if player is moving up or down
		float rayLength = Mathf.Abs (velocity.y) + skinWidth; // distance player is to move in the given frame

		// run a loop through all rays to check for a collision
		for(int i = 0; i<verticalRayCount; i++){
			// check if we are moving up or down
			Vector2 rayOrigin = (directionY == -1)?raycastOrigins.bottomLeft:raycastOrigins.topLeft;
			rayOrigin += Vector2.right*(verticalRaySpacing*i + velocity.x); // calculate new ray origin
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up*directionY, rayLength, collisionMask);
			RaycastHit2D hit2 = Physics2D.Raycast(rayOrigin, Vector2.up*directionY, rayLength, bottomLevel);

			// display rays on screen
			//Debug.DrawRay(rayOrigin, Vector2.up*directionY*rayLength, Color.red);

			if(hit && (directionY == -1) && !downbutton){
				velocity.y = (hit.distance - skinWidth) * directionY; // the length from player object to level object;
				rayLength = hit.distance;

				// update collision structure
				collisions.below = true;
				//collisions.above = directionY == 1;
			}
			if(hit2 && (directionY == -1)){
				velocity.y = (hit2.distance - skinWidth) * directionY; // the length from player object to level object;
				rayLength = hit2.distance;
				
				// update collision structure
				collisions.below = true;
				//collisions.above = directionY == 1;
			}
		}
	}


	// update positions of rays
	void UpdateRaycastOrigins(){
		Bounds bounds = collider.bounds;
		bounds.Expand (skinWidth * -2); // we wish the rays to start inside the box.


		//set positions of the rays in the four corners
		raycastOrigins.bottomLeft = new Vector2 (bounds.min.x, bounds.min.y);
		raycastOrigins.bottomRight = new Vector2 (bounds.max.x, bounds.min.y);
		raycastOrigins.topLeft = new Vector2 (bounds.min.x, bounds.max.y);
		raycastOrigins.topRight = new Vector2 (bounds.max.x, bounds.max.y);
	}

	void CalculateRaySpacing(){
		Bounds bounds = collider.bounds;
		bounds.Expand (skinWidth * -2);

		// makes sure the number of rays is greater than two
		horizontalRayCount = Mathf.Clamp (horizontalRayCount, minRays, int.MaxValue);
		verticalRayCount = Mathf.Clamp (verticalRayCount, minRays, int.MaxValue);

		// calculate the space between each ray, that is the length of the bounds side
		// divided by the number of rays.
		horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
		verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
	}

	// Structure to hold the four corner rays
	struct RaycastOrigins{
		public Vector2 topLeft, topRight;
		public Vector2 bottomLeft, bottomRight;
	}

	public struct CollisionInfo{
		public bool above, below;
		public bool left, right;

		public void Reset(){
			above = below = false;
			left = right = false;
		}
	}
	
}
