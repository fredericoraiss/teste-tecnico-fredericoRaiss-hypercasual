using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance { get; private set; }
    public float CurrentCash { get => currentCash; set => currentCash = value; }
    public float CostToUpgrade { get => costToUpgrade; set => costToUpgrade = value; }
    public int CurrentBodys { get => currentBodys; set => currentBodys = value; }
    public int MaxBodys { get => maxBodys; set => maxBodys = value; }
    public int CurrentLevel { get => currentLevel; set => currentLevel = value; }

    [SerializeField] private int currentBodys;
    [SerializeField] private int maxBodys = 5;

    [SerializeField] private float currentCash;
    [SerializeField] private float costToUpgrade;

    [SerializeField] private int currentLevel = 0;

    public UIController uiController;
    public GameObject playerObj;
    private PlayerController playerController;

    public List<GameObject> enemiesOnScene = new();

    private void Awake()
    {
        if (instance == null)
            instance = this;

        playerController = playerObj.GetComponent<PlayerController>();
    }

    void Start()
    {
        InitialConfigure();
    }


    private void InitialConfigure()
    {
        CurrentLevel = 0;
        currentCash = 0;
        CurrentBodys = 0;
        MaxBodys = 5;
        CostToUpgrade = 35;
    }


    private void Upgrade()
    {
        CurrentLevel += 1;
        MaxBodys += 3;
        CostToUpgrade = (CostToUpgrade * 0.2f) + CostToUpgrade;
        playerController.CallLevelUpText();
        playerController.ChangeColorToRandonValue();
        uiController.updateText.Invoke();
    }

    public void AddCash(float value) { CurrentCash += value; uiController.updateText.Invoke(); }
    public void RemoveCash(float value) { CurrentCash -= value; uiController.updateText.Invoke(); }

    public void BuyUpgrade()
    {
        if (CurrentCash >= CostToUpgrade)
        {
            RemoveCash(CostToUpgrade);
            Upgrade();
        }
    }

    public float DistanceBetweenObjects(Vector3 a, Vector3 b)
    {
        return (b - a).sqrMagnitude;
    }
}
