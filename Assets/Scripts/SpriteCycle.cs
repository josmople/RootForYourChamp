using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteCycle : MonoBehaviour
{
    
    public Sprite[] sprites;
    public float intervalSeconds = 0.4f;

    private float current;
    private int spriteIndex;

    public Image image;

    void Start() {
        if (image == null)
            image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        current += Time.deltaTime;

        while (current > intervalSeconds) {
            current -= intervalSeconds;

            var displayedSprite = sprites[spriteIndex++ % sprites.Length];
            image.sprite = displayedSprite;
        }
    }
}
