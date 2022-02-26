using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (BoxCollider2D))]
public class Controller2D : MonoBehaviour
{
    public LayerMask collisionMask;

    const float skinWidth = 0.015f;
    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;

    float maxClimbAngle = 75;
    float maxDescendAngle = 75;

    float horizontalRaySpacing;
    float verticalRaySpacing;

    BoxCollider2D boxCollider;
    RaycastOrigins raycastOrigins;
    public CollisionInfo collisions;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        CalculateRaySpacing();
    }

    public void Move(Vector3 velocity)
    {
        UpdateRaycaseOrigins();
        collisions.Reset();
        collisions.velocityOld = velocity;

        //  Code to handle collisions and modify velocity
        if (velocity.y < 0)
        {
            DescendSlope(ref velocity);
        }
        if (velocity.x != 0)
        {
            HorizontalCollisions(ref velocity);
        }
        if (velocity.y != 0)
        {
            VerticalCollisions(ref velocity);
        }

        transform.Translate(velocity);
    }
    void HorizontalCollisions(ref Vector3 velocity)
    {
        // Down = -1, Up = 1
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;
        for (int i = 0; i < horizontalRayCount; i++)
        {
            // If moving Left, ray bottom left
            // If moving Right, ray top left
            Vector2 rayOrigin = (directionX == -1)
                ? raycastOrigins.bottomLeft
                : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRayCount * i);
            RaycastHit2D hit = Physics2D.Raycast(
                rayOrigin,
                Vector2.right * directionX,
                rayLength, 
                collisionMask);
            Debug.DrawRay(
                rayOrigin,
                Vector2.right * directionX * rayLength,
                Color.red);

            if (hit)
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                if (i == 0 && slopeAngle <= maxClimbAngle)
                {
                    if (collisions.descendingSlope)
                    {
                        collisions.descendingSlope = false;
                        velocity = collisions.velocityOld;
                    }
                    float distanceToSlopStart = 0;
                    if (slopeAngle != collisions.slopeAngleOld)
                    {
                        distanceToSlopStart = hit.distance - skinWidth;
                        velocity.x -= distanceToSlopStart * directionX;
                    }
                    ClimbSlope(ref velocity, slopeAngle);
                    velocity.x += distanceToSlopStart * directionX;
                }


                if (!collisions.climbingSlope || slopeAngle > maxClimbAngle)
                {
                    // Deal with x on slope
                    /*velocity.x = Mathf.Min(Mathf.Abs(velocity.x), (hit.distance - skinWidth)) * directionX;
                    rayLength = Mathf.Min(Mathf.Abs(velocity.x) + skinWidth, hit.distance);*/
                    velocity.x = Mathf.Abs(hit.distance - skinWidth) * directionX;
                    rayLength = hit.distance;

                    // Prevent jittering when climbing slope and collide wall
                    // by calculating velocity y
                    if (collisions.climbingSlope)
                    {
                        velocity.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);
                    }

                    // If hit something and going left
                    collisions.left = directionX == -1;
                    // If hit something and going right
                    collisions.right = directionX == 1;
                }
            }
        }
    }

    void VerticalCollisions(ref Vector3 velocity)
    {
        // Down = -1, Up = 1
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;
        for (int i = 0; i < verticalRayCount; i++)
        {
            // If moving Down, ray bottom left
            // If moving Up, ray top left
            Vector2 rayOrigin = (directionY == -1)
                ? raycastOrigins.bottomLeft
                : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(
                rayOrigin, 
                Vector2.up * directionY,
                rayLength,
                collisionMask);
            Debug.DrawRay(
                rayOrigin,
                Vector2.up * directionY * rayLength,
                Color.red);

            if (hit)
            {
                velocity.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;

                if (collisions.climbingSlope)
                {
                    velocity.x = velocity.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);
                }

                // If hit something and going down
                collisions.below = directionY == -1;
                // If hit something and going up
                collisions.above = directionY == 1;
            }
        }

        // Check for new slope when climbing slope
        if (collisions.climbingSlope)
        {
            float directionX = Mathf.Sign(velocity.x);
            rayLength = Mathf.Abs(velocity.x) + skinWidth;
            Vector2 rayOrigin = ((directionX == -1)
                ? raycastOrigins.bottomLeft
                : raycastOrigins.bottomRight)
                + Vector2.up * velocity.y;
            RaycastHit2D hit = Physics2D.Raycast(
                rayOrigin,
                Vector2.right * directionX,
                rayLength,
                collisionMask);

            if (hit)
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                // If collide with new slope
                if (slopeAngle != collisions.slopeAngle)
                {
                    velocity.x = (hit.distance - skinWidth) * directionX;
                    collisions.slopeAngle = slopeAngle;
                }
            }
        }
    }
    
    void ClimbSlope(ref Vector3 velocity, float slopeAngle)
    {
        float moveDistance = Mathf.Abs(velocity.x);
        float climbVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

        if (velocity.y <= climbVelocityY) { 
            velocity.y = climbVelocityY;
            velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
            collisions.below = true;
            collisions.climbingSlope = true;
            collisions.slopeAngle = slopeAngle;
        }
    }

    void DescendSlope(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        Vector2 rayOrigin = (directionX == -1)
            ? raycastOrigins.bottomRight
            : raycastOrigins.bottomLeft;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, collisionMask);

        if (hit)
        {
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            if (slopeAngle != 0 && slopeAngle <= maxDescendAngle)
            {
                if (Mathf.Sign(hit.normal.x) == directionX)
                {
                    if (hit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x))
                    {
                        float moveDistance = Mathf.Abs(velocity.x);
                        float descendVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                        velocity.x = Mathf.Cos(slopeAngle * Mathf.Rad2Deg) * moveDistance * Mathf.Sign(velocity.x);
                        velocity.y -= descendVelocityY;

                        collisions.slopeAngle = slopeAngle;
                        collisions.descendingSlope = true;
                        collisions.below = true;
                    }
                }
            }
        }
    }

    void UpdateRaycaseOrigins()
    {
        Bounds bounds = boxCollider.bounds;
        bounds.Expand(skinWidth * -2);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    void CalculateRaySpacing()
    {
        Bounds bounds = boxCollider.bounds;
        bounds.Expand(skinWidth * -2);

        // Make sure there is at least 2 rays firing horizontally
        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);

        // Make sure there is at least 2 rays firing vertically
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.y / (verticalRayCount - 1);


    }

    struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }

    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;

        public bool climbingSlope;
        public bool descendingSlope;
        public float slopeAngle, slopeAngleOld;
        public Vector3 velocityOld;

        public void Reset()
        {
            above = below = false;
            left = right = false;
            climbingSlope = false;
            descendingSlope = false;

            slopeAngleOld = slopeAngle;
            slopeAngle = 0;
        }
    }
}
