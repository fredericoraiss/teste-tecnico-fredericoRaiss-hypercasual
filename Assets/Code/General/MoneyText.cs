using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MoneyText : MonoBehaviour
{
    TMP_Text text;

    public float moveSpeed = 5;
    public float timeToDestroy = 1f;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
    }
    private void Start()
    {
        Destroy(this.gameObject, timeToDestroy);
    }

    private void Update()
    {
        transform.Translate(new(0, 1 * moveSpeed * Time.deltaTime, 0));
    }

    public void SetText(string value)
    {
        text.SetText($"+${value}");
    }
}
