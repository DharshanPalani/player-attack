using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDirectionCheck : MonoBehaviour
{
    public PlayerMovement playerMovement;

    public Directions currentDirections;

    private Vector2 direction;

    public enum Directions { 
            EAST,
            WEST,
            NORTH,
            SOUTH
    }

    void Update()
    {
        direction = playerMovement.GetCurrentDirection();
        /*string facingDirection = GetFacingDirection(direction);
        Debug.Log("Player is facing: " + facingDirection);*/
    }

    public string GetFacingDirection()
    {
        // Check the sign of the x and y components to determine the facing direction
        // Don't ask me how I made this up. I know basic maths.
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            // More horizontal movement, so it check x component
            if (direction.x > 0)
            {
                currentDirections = Directions.EAST;
            }
            else
            {
                currentDirections = Directions.WEST;
            }
        }
        else
        {
            // More vertical movement, so it check y component
            if (direction.y > 0)
            {
                currentDirections = Directions.NORTH;
            }
            else
            {
                currentDirections = Directions.SOUTH;
            }
        }

        return currentDirections.ToString();
    }
}
