using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Piece : MonoBehaviour
{
    public bool inRightPos = false;
    public bool locked = false;
    public Vector2 rightPos;
    public Vector2 messyPos;
    public Vector3 originalScale;
    public AudioSource puzzleSound;
    public Sprite question = null;
    public string answer;

    private void OnEnable()
    {
        rightPos = transform.position;
        transform.localPosition = new Vector3(Random.Range(-6.0f, -2.0f), Random.Range(2.0f, 5.0f), 0.0f);
        messyPos = transform.position;
        originalScale = transform.localScale;
        puzzleSound = GameObject.Find("PuzzlePlaceSound").GetComponent<AudioSource>();
        gameObject.GetComponent<SpriteRenderer>().color = new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), 255);
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, rightPos) < 0.25f && !inRightPos)
        {
            transform.position = rightPos;
            inRightPos = true;
            puzzleSound.Play();
        }
    }
}
