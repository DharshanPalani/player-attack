using System.Collections.Generic;
using UnityEngine;

public class SwordRangeController : MonoBehaviour
{
    //public SwordRangeTrigger[] swordRangeTriggers;
    public PlayerDirectionCheck playerDirection;
    public List<GameObject> targetObjects = new List<GameObject>();
    public bool inRange;

    [SerializeField] private SwordRangeTrigger East_Trigger;
    [SerializeField] private SwordRangeTrigger West_Trigger;
    [SerializeField] private SwordRangeTrigger North_Trigger;
    [SerializeField] private SwordRangeTrigger South_Trigger;

    public void AddDirections(GameObject obj)
    {
        SwordRangeTrigger direction = obj.GetComponent<SwordRangeTrigger>();

        string directionName = obj.name.ToUpper();

        if (direction != null)
        {
            switch (directionName)
            {
                case "RIGHT":
                    East_Trigger = direction;
                    break;
                case "LEFT":
                    West_Trigger = direction;
                    break;
                case "NORTH":
                    North_Trigger = direction;
                    break;
                case "SOUTH":
                    South_Trigger = direction;
                    break;
                default:
                    Debug.LogWarning("Unknown direction: " + directionName);
                    break;
            }
        }

    }

    public void CheckTrigger()
    {
        if (playerDirection == null)
        {
            Debug.LogError("Player direction check component is not set!");
            return;
        }

        RemoveTriggerEnable(East_Trigger);
        RemoveTriggerEnable(West_Trigger);
        RemoveTriggerEnable(North_Trigger);
        RemoveTriggerEnable(South_Trigger);

        // Get the player facing direction
        string facingDirection = playerDirection.GetFacingDirection();

        // Enable or disable trigger based on the player facing direction
        switch (facingDirection)
        {
            case "NORTH":
                SetTriggerEnabled(North_Trigger);
                break;
            case "SOUTH":
                SetTriggerEnabled(South_Trigger);
                break;
            case "WEST":
                SetTriggerEnabled(West_Trigger);
                break;
            case "EAST":
                SetTriggerEnabled(East_Trigger);
                break;
            default:
                Debug.LogWarning("Unknown direction: " + facingDirection);
                break;
        }
    }


    private void SetTriggerEnabled(SwordRangeTrigger trigger)
    {
        if (trigger != null)
        {
            //Debug.Log("Trigger set for " + trigger);
            trigger.enable = true;
            //targetObjects = trigger.enemyObject;

            SetGameObject(trigger.enemyObjects);

            if (trigger.inRangeTrigger)
            {
                inRange = true;
            }

            else if (!trigger.inRangeTrigger)
            {
                inRange = false;
            }
        }
        else
        {
            Debug.LogError("Sword range trigger is null! (From set trigger)");
        }
    }


    private void RemoveTriggerEnable(SwordRangeTrigger trigger)
    {
        if (trigger != null)
        {
            trigger.enable = false;
            targetObjects.Clear();
        }
        else
        {
            Debug.LogError("Sword range trigger is null! (From remove trigger)");
        }
    }
    private void SetGameObject(List<GameObject> obj)
    {
        if (targetObjects == null)
        {
            Debug.LogError("targetObjects is null!");
            return;
        }
        
        // Copy elements from obj to targetObjects
        foreach (GameObject gameObject in obj)
        {
            // Add a clone of the gameObject to targetObjects

            if (!targetObjects.Contains(gameObject))
            {
                targetObjects.Add(gameObject);
            }
        }
        
        
    }

    public void TriggerEnemyDamage(SwordData sword)
    {
        if (inRange && targetObjects != null)
        {
            foreach (GameObject _object in targetObjects)
            {
                Health enemyHealth = _object.GetComponent<Health>();

                if (enemyHealth != null)
                {
                    enemyHealth.UpdateHealth(-sword.damage);
                }
                else
                {
                    Debug.Log("Something is wrong!");
                }
            }
        }


    }


}
