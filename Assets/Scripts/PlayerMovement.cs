using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

[RequireComponent(typeof(LineRenderer))]
public class PlayerMovement : MonoBehaviour
{
    public float baseSpeed = 1f;
    public float degreesPerSecond = 90f;
    public bool doubleSpeed = false;
    public float trailSpacing = 0.1f;
    public GameObject trailPrefab;
    private Transform trailParent;
    private Vector3 lastPos;
    private List<Vector3> points = new List<Vector3>();
    private LineRenderer lineRenderer;
    private Rigidbody2D rb;



    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        
        GameObject go = new GameObject("TrailRoot");
        trailParent = go.transform;

        lineRenderer = GetComponent<LineRenderer>();
        AddPoint(transform.position);
        lastPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        updateInputStates();
    }


    private void updateInputStates()
    {
        // x = r * sin(angle), y = r * cos(angle)
        if (Keyboard.current.aKey.isPressed)
        {
            this.calculateNextPosition(-degreesPerSecond);
        }
        else if (Keyboard.current.dKey.isPressed)
        {
            this.calculateNextPosition(degreesPerSecond);
        }
        else
        {
            this.calculateNextPosition(0);
        }
    }

    private void calculateNextPosition(float angle)
    {
        if (Vector2.Distance(lastPos, transform.position) > trailSpacing)
        {
            GameObject trail = Instantiate(trailPrefab, lastPos, Quaternion.identity, trailParent);
            AddPoint(transform.position);

            lastPos = transform.position;
        }

        this.RotateAndMoveForward(angle);
        if (this.doubleSpeed)
        {
            this.RotateAndMoveForward(angle);
        }
    }



    private void RotateAndMoveForward(float angle)
    {
        transform.Rotate(0, 0, angle * Time.deltaTime);
        transform.position += baseSpeed * Time.deltaTime * transform.up;
    }

    void AddPoint(Vector3 point)
    {
        points.Add(point);
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPosition(points.Count - 1, point);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collided with Pos: " + other.gameObject.transform.position + ", lastTrailPos: " + lastPos);
    }
}
