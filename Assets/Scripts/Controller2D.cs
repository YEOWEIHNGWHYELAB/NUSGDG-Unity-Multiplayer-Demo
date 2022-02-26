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

    float horizontalRaySpacing;
    float verticalRaySpacing;

    BoxCollider2D collider;
    RaycaseOrigins raycastOrigins;

    void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        CalculateRaySpacing();
    }

    public void Move(Vector3 velocity)
    {
        UpdateRaycaseOrigins();
        //  Code to handle collisions and modify velocity
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
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin,
                Vector2.right * directionX,
                rayLength, collisionMask);
            Debug.DrawRay(
                rayOrigin,
                Vector2.right * directionX * rayLength,
                Color.red);

            if (hit)
            {
                velocity.x = (hit.distance - skinWidth) * directionX;
                rayLength = hit.distance;
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
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, 
                Vector2.up * directionY,
                rayLength, collisionMask);
            Debug.DrawRay(
                rayOrigin,
                Vector2.up * directionY * rayLength,
                Color.red);

            if (hit)
            {
                velocity.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;
            }
        }
    }
    void UpdateRaycaseOrigins()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    void CalculateRaySpacing()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        // Make sure there is at least 2 rays firing horizontally
        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);

        // Make sure there is at least 2 rays firing vertically
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.y / (verticalRayCount - 1);


    }

    struct RaycaseOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }
}
