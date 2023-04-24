using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEngine;

public class BasketballHoop : MonoBehaviour
{
    private float horizontalBound = 4;
    private float verticalBound = 1.5f;
    public int difficultyLevel = 0;
    private Vector3 destinationPosition;
    private Vector3 startingPosition;
    private float speed = 1;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
        destinationPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        switch (difficultyLevel)
        {
            case 1:
                if(Vector3.Distance(transform.position, destinationPosition) < .05f)
                {
                    destinationPosition = new Vector3(horizontalBound * RandomSign(), startingPosition.y, startingPosition.z);
                } else
                {
                    MoveHoop(destinationPosition);
                }
                break;
            case 2:
                if (speed < 2)
                {
                    speed = 2;
                }
                if (Vector3.Distance(transform.position, destinationPosition) < .05f)
                {
                    destinationPosition = new Vector3(horizontalBound * RandomSign(), startingPosition.y, startingPosition.z);
                } else
                {
                    MoveHoop(destinationPosition);
                }
                break;
            case 3:
                if (speed > 1)
                {
                    speed = 1;
                }
                if (Vector3.Distance(transform.position, destinationPosition) < .05f)
                {
                    destinationPosition = new Vector3(Random.Range(-horizontalBound, horizontalBound), Random.Range(-verticalBound, verticalBound) + startingPosition.y, startingPosition.z);
                } else
                {
                    MoveHoop(destinationPosition);
                }
                break;
        }
    }

    private int RandomSign()
    {
        return Random.Range(0, 2) * 2 - 1;
    }

    public void SetDifficulty(int currentDifficulty)
    {
        difficultyLevel = currentDifficulty;
    }

    private void MoveHoop(Vector3 nextPos)
    {
        transform.position = Vector3.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);
    }
}
