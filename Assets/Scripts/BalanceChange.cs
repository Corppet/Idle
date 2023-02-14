using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(CanvasGroup), typeof(TMP_Text))]
public class BalanceChange : MonoBehaviour
{
    [Range(0f, 100f)]
    [SerializeField] private float fadeSpeed = 5f;
    [Range(0f, 100f)]
    [SerializeField] private float moveSpeed = 5f;

    private CanvasGroup canvasGroup;
    private TMP_Text balanceText;

    public void SetText(string text)
    {
        balanceText.text = text;
    }

    public void SetText(string text, Color color)
    {
        balanceText.text = text;
        balanceText.color = color;
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        balanceText = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;
        canvasGroup.alpha -= fadeSpeed * Time.deltaTime;
        if (canvasGroup.alpha <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
