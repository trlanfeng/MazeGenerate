using UnityEngine;
using System.Collections;

public class MoveBy8Dir : MonoBehaviour
{

    //enum Direction
    //{
    //    None,
    //    UpLeft,
    //    Up,
    //    UpRight,
    //    Left,
    //    Right,
    //    DownLeft,
    //    Down,
    //    DownRight
    //}
    //Direction dir;

    float xOffset;
    float yOffset;
    SpriteFrames SF;
    Rigidbody2D R2D;

    void Start()
    {
        SF = GetComponent<SpriteFrames>();
        R2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        xOffset = Input.GetAxis("Horizontal");
        yOffset = Input.GetAxis("Vertical");
        //transform.position += (Vector3)checkDirection() * Time.deltaTime * 200 ;
        //debugMove();
    }

    void FixedUpdate()
    {
        fixPlayerPosition();
    }

    void LateUpdate()
    {
        followCamera();
    }

    void fixPlayerPosition()
    {
        R2D.velocity = checkDirection() * Time.deltaTime * 150;
        Vector3 pos = R2D.position;
        if (pos.y > 20.2f)
        {
            pos.y = 20.2f;
        }
        else if (pos.y < 0.2f)
        {
            pos.y = 0.2f;
        }
        if (pos.x < -0.35f)
        {
            pos.x = -0.35f;
        }
        else if (pos.x > 20.35f)
        {
            pos.x = 20.35f;
        }
        R2D.position = pos;
    }

    void followCamera()
    {
        Vector3 pos = transform.position - new Vector3(0, 0, 10);
        if (pos.y > MazeGenerate2D.cameraMaxY)
        {
            pos.y = MazeGenerate2D.cameraMaxY;
        }
        else if (pos.y < MazeGenerate2D.cameraMinY)
        {
            pos.y = MazeGenerate2D.cameraMinY;
        }
        if (pos.x < MazeGenerate2D.cameraMinX)
        {
            pos.x = MazeGenerate2D.cameraMinX;
        }
        else if (pos.x > MazeGenerate2D.cameraMaxX)
        {
            pos.x = MazeGenerate2D.cameraMaxX;
        }
        Camera.main.transform.position = pos;
    }

    Vector2 checkDirection()
    {
        if (yOffset > 0 && xOffset < 0)
        {
            //dir = Direction.UpLeft;
            SF.curClip = 6;
        }
        else if (yOffset > 0 && xOffset == 0)
        {
            //dir = Direction.Up;
            SF.curClip = 3;
        }
        else if (yOffset > 0 && xOffset > 0)
        {
            //dir = Direction.UpRight;
            SF.curClip = 7;
        }
        else if (xOffset < 0 && yOffset == 0)
        {
            //dir = Direction.Left;
            SF.curClip = 1;
        }
        else if (xOffset == 0 && yOffset == 0)
        {
            //dir = Direction.None;
            SF.isPlay = false;
            return Vector2.zero;
        }
        else if (xOffset > 0 && yOffset == 0)
        {
            //dir = Direction.Right;
            SF.curClip = 2;
        }
        else if (yOffset < 0 && xOffset < 0)
        {
            //dir = Direction.DownLeft;
            SF.curClip = 4;
        }
        else if (yOffset < 0 && xOffset == 0)
        {
            //dir = Direction.Down;
            SF.curClip = 0;
        }
        else if (yOffset < 0 && xOffset > 0)
        {
            //dir = Direction.DownRight;
            SF.curClip = 5;
        }
        SF.isPlay = true;
        Vector2 moveOffset = new Vector2(Mathf.RoundToInt(xOffset), Mathf.RoundToInt(yOffset));
        return moveOffset;
    }


    void debugMove()
    {
        Debug.Log("x:" + xOffset + ",y:" + yOffset);
    }
}
