using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeHead : BodyPart
{
    Vector2 movement;

    private BodyPart tail = null;

    const float TimeToAddBodyPart = 0.1f;
    float addTimer = TimeToAddBodyPart;

    public int partsToAdd = 0;

    void Start()
    {
        SwipeControls.OnSwipe += SwipeDetection;
    }

    override public void Update()
    {
        base.Update();

        SetMovement(movement);
        UpdateDirection();
        UpdatePosition();

        if (partsToAdd > 0)
        {
            addTimer -= Time.deltaTime;
            if (addTimer <= 0)
            {
                addTimer = TimeToAddBodyPart;
                AddBodyPart();
                partsToAdd--;
            }
        }
    }

    void AddBodyPart()
    {
        if (tail == null)
        {
            Vector3 newPosition = transform.position;
            newPosition.z += 0.01f;

            BodyPart newPart = Instantiate(GameController.instance.bodyPrefab, newPosition, Quaternion.identity);
            newPart.following = this;
            tail = newPart;
            newPart.TurnIntoTail();
        }
        else
        {
            Vector3 newPosition = tail.transform.position;
            newPosition.z += 0.01f;

            BodyPart newPart = Instantiate(GameController.instance.bodyPrefab, newPosition, Quaternion.identity);
            newPart.following = tail;
            newPart.TurnIntoTail();
            tail.TurnIntoBodyPart();
            tail = newPart;
        }
    }

    void SwipeDetection(SwipeControls.SwipeDirection direction)
    {
        switch (direction)
        {
            case SwipeControls.SwipeDirection.Up:
                MoveUp();
                break;
            case SwipeControls.SwipeDirection.Down:
                MoveDown();
                break;
            case SwipeControls.SwipeDirection.Left:
                MoveLeft();
                break;
            case SwipeControls.SwipeDirection.Right:
                MoveRight();
                break;
        }
    }

    void MoveUp()
    {
        movement = GameController.instance.snakeSpeed * Time.deltaTime * Vector2.up;
    }

    void MoveDown()
    {
        movement = GameController.instance.snakeSpeed * Time.deltaTime * Vector2.down;
    }

    void MoveLeft()
    {
        movement = GameController.instance.snakeSpeed * Time.deltaTime * Vector2.left;
    }

    void MoveRight()
    {
        movement = GameController.instance.snakeSpeed * Time.deltaTime * Vector2.right;
    }

    public void ResetSnake()
    {
        tail = null;

        MoveUp();

        partsToAdd = 5;
        addTimer = TimeToAddBodyPart;
    }
}
