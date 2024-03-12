using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

    public Button buyLevelButton;

    public TMP_Text moneyText;
    public TMP_Text bodysText;
    public TMP_Text levelText;
    private TMP_Text buyLevelTextButton;
    public UnityEvent updateText;

    private void OnEnable()
    {
        updateText.AddListener(UpdateTexts);
        buyLevelButton.onClick.AddListener(BuyUpgrade);
    }
    private void OnDisable()
    {
        updateText.RemoveListener(UpdateTexts);
        buyLevelButton.onClick.RemoveListener(BuyUpgrade);
    }

    private void Awake()
    {
        buyLevelTextButton = buyLevelButton.GetComponentInChildren<TMP_Text>();
    }

    private void Start()
    {
        updateText.Invoke();


    }

    public void BuyLevelVisibleControl()
    {
        if (GameController.instance.CurrentCash >= GameController.instance.CostToUpgrade)
        {
            buyLevelButton.interactable = true;
        }
        else
        {
            buyLevelButton.interactable = false;
        }
    }

    private void BuyUpgrade()
    {
        GameController.instance.BuyUpgrade();
    }

    private void UpdateTexts()
    {
        BuyLevelVisibleControl();
        buyLevelTextButton.SetText($"BUY LEVEL ${GameController.instance.CostToUpgrade}");
        moneyText.SetText($"${GameController.instance.CurrentCash.ToString("00")}");
        bodysText.SetText($"Bodys: {GameController.instance.CurrentBodys}/{GameController.instance.MaxBodys}");
        levelText.SetText($"Level: {GameController.instance.CurrentLevel}");
    }


}
