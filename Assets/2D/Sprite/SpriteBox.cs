using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
[RequireComponent(typeof(SpriteRenderer))]
public class SpriteBox : MonoBehaviour
{
    SpriteRenderer SR;
    public Vector2 originalSize;
    public Vector2 showSize;
    public float Up;
    public float Down;
    public float Left;
    public float Right;
    public Vector2 size;

    // Use this for initialization
    void Start()
    {
        SR = GetComponent<SpriteRenderer>();
        getSpriteSize();
        getSpriteCorner();
    }

    // Update is called once per frame
    void Update()
    {
        getSpriteSize();
        getSpriteCorner();
    }

    public void getSpriteSize()
    {
        if (SR == null)
        {
            return;
        }
        float x = SR.sprite.bounds.size.x * 100;
        float y = SR.sprite.bounds.size.y * 100;
        originalSize = new Vector2(x, y);
        float rx = SR.bounds.size.x * 100;
        float ry = SR.bounds.size.y * 100;
        showSize = new Vector2(rx, ry);
    }

    public void getSpriteCorner()
    {
        if (SR == null)
        {
            return;
        }
        Up = SR.bounds.size.y / 2f + transform.localPosition.y;
        Down = SR.bounds.size.y / 2f - transform.localPosition.y;
        Left = SR.bounds.size.x / 2f - transform.localPosition.x;
        Right = SR.bounds.size.x / 2f + transform.localPosition.x;
    }

    public void resizeToSet()
    {
        if (size.x > 0 && size.y > 0)
        {
            float sx = size.x / originalSize.x;
            float sy = size.y / originalSize.y;
            transform.localScale = new Vector3(sx, sy, 1);
        }
    }

    public void resizeToOriginal()
    {
        transform.localScale = Vector3.one;
    }
}
