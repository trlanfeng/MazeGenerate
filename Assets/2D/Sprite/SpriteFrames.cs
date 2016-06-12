using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[ExecuteInEditMode]
public class SpriteFrames : MonoBehaviour
{
    public float speed;
    public int curClip;
    public bool isPlay;
    public bool staticFirstFrane;
    [System.Serializable]
    public struct Clips
    {
        public string name;
        public bool repeat;
        public List<Sprite> sprites;
    }
    public Clips[] clips;

    int spriteIndex;
    SpriteRenderer SR;
    void Start()
    {
        SR = GetComponent<SpriteRenderer>();
        spriteIndex = 0;
        if (speed <= 0)
        {
            speed = 1;
        }
        if (curClip <= 0)
        {
            curClip = 0;
        }
    }

    void Update()
    {
        if (clips.Length <= 0)
        {
            return;
        }
        playFrames(curClip);
    }

    float timer = 0;
    void playFrames(int clipIndex)
    {
        if (isPlay)
        {
            timer += Time.deltaTime;
            if (timer > 0.05f / speed)
            {
                spriteIndex++;
                timer = 0;
            }
            if (spriteIndex >= clips[clipIndex].sprites.Count)
            {
                if (clips[clipIndex].repeat)
                {
                    if (staticFirstFrane)
                    {
                        spriteIndex = 1;
                    }
                    else
                    {
                        spriteIndex = 0;
                    }
                }
                else
                {
                    spriteIndex = clips[clipIndex].sprites.Count - 1;
                }
            }
            SR.sprite = clips[clipIndex].sprites[spriteIndex];
        }
        else
        {
            if (staticFirstFrane && spriteIndex != 0)
            {
                spriteIndex = 0;
                SR.sprite = clips[clipIndex].sprites[spriteIndex];
            }
        }
    }
}
