using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SwordRangeTrigger : MonoBehaviour
{
    // Public variables
    public List<GameObject> enemyObjects = new List<GameObject>();
    [SerializeField] private Vector2 _detectionSize = new Vector2(2f, 2f); // Set the size of your detection area
    public LayerMask enemyLayer; // Set this to the layer where your enemies are located
    public UnityEvent startEvent;
    public UnityEvent triggerEvent;
    public Color[] colorArray = { Color.red, Color.green };

    public bool enable;
    public bool inRangeTrigger;

    // Private variables
    private Color _majorColor;
    private Color _visualizationColor;

    private void Awake()
    {
        _majorColor = colorArray[0];
        _visualizationColor = _majorColor;
    }

    private void Start()
    {
        // Invoke start event
        startEvent.Invoke();
    }

    private void FixedUpdate()
    {
        _majorColor = colorArray[1];

        // Detect enemies within the detection area
        Collider2D[] colliders = null;

        // Adjust detection area based on the orientation of the object
        Vector2 offset = Vector2.zero;
        Vector2 size = _detectionSize;

        // Determine the orientation of the object
        float angle = transform.eulerAngles.z;
        if (angle < 45f || angle > 315f) // Facing to the right (East)
        {
            offset = transform.right * (_detectionSize.x / 2f);
            size = new Vector2(_detectionSize.x / 2f, _detectionSize.y);
        }
        else if (angle > 135f && angle < 225f) // Facing to the left (West)
        {
            offset = -transform.right * (_detectionSize.x / 2f);
            size = new Vector2(_detectionSize.x / 2f, _detectionSize.y);
        }
        else if (angle > 45f && angle < 135f) // Facing upwards (North)
        {
            offset = transform.up * (_detectionSize.y / 2f);
            size = new Vector2(_detectionSize.x, _detectionSize.y / 2f);
        }
        else if (angle > 225f && angle < 315f) // Facing downwards (South)
        {
            offset = -transform.up * (_detectionSize.y / 2f);
            size = new Vector2(_detectionSize.x, _detectionSize.y / 2f);
        }

        colliders = Physics2D.OverlapBoxAll((Vector2)transform.position + offset, size, 0f, enemyLayer);

        // Clear the list of detected enemies
        enemyObjects.Clear();

        // Add detected enemies to the list
        foreach (Collider2D col in colliders)
        {
            if (col.CompareTag("Enemy"))
            {
                AppendGameObject(col.gameObject);
            }
        }

        if (enemyObjects.Count > 0)
        {
            inRangeTrigger = true;
        }
        else
        {
            inRangeTrigger = false;
        }

        _visualizationColor = enable ? _majorColor : colorArray[0];

        // Invoke trigger event
        triggerEvent.Invoke();
    }

    // Method to add a game object to the list of enemy objects
    private void AppendGameObject(GameObject obj)
    {
        if (!enemyObjects.Contains(obj))
        {
            enemyObjects.Add(obj);
        }
    }

    // Visualize the detection area in the Unity editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = _visualizationColor;
        Gizmos.DrawWireCube(transform.position, _detectionSize);
    }
}
