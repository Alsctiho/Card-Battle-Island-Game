using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI attackText;
    [SerializeField] private TextMeshProUGUI healthText;

    private Image image;

    private void Start()
    {
        image = GetComponentInChildren<Image>();
    }

    public void UpdateSprite(Sprite sprite)
    {
        image.sprite = sprite;
    }

    public void UpdateAttack(int attack)
    {
        attackText.text = attack.ToString();
    }

    public void UpdateHealth(int health)
    {
        healthText.text = health.ToString();
    }
}
