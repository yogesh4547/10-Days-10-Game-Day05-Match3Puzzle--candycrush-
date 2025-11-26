using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    public int column;
    public int row;
    public int type;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 10f;

    private Vector2 targetPosition;
    private bool isMoving = false;

    public void Initialize(int col, int r, int gemType)
    {
        column = col;
        row = r;
        type = gemType;

        targetPosition = transform.position;
    }

    public void SetPosition(int col, int r)
    {
        column = col;
        row = r;

        if (Board.Instance != null)
        {
            targetPosition = Board.Instance.GetWorldPosition(col, r);
            isMoving = true;
        }
    }

    void Update()
    {
        if (isMoving)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition;
                isMoving = false;
            }
        }
    }

    public bool IsMoving()
    {
        return isMoving;
    }
}
