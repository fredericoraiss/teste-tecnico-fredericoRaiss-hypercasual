using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyStation : MonoBehaviour
{

    public float sellValue = 10;

    private Coroutine sellCoroutine;

    public GameObject moneyTextPrefab;
    public Transform spawnMoneyTextTransform;

    private IEnumerator SellBodys(PlayerController p)
    {
        if (p.bodysStack.Count <= 0) yield return null;

        for (int i = p.bodysStack.Count - 1; i >= 0; i--)
        {
            MoneyText mText = Instantiate(moneyTextPrefab, spawnMoneyTextTransform, false).GetComponent<MoneyText>();
            mText.transform.localScale = Vector3.one;
            mText.transform.localPosition = Vector3.zero;
            mText.SetText(sellValue.ToString());

            GameController.instance.CurrentBodys--;
            GameController.instance.AddCash(sellValue);
            p.bodysStack[i].GetComponent<BodyCarried>().Selling();
            yield return new WaitForSeconds(0.15f);
        }

        p.bodysStack.Clear();
        sellCoroutine = null;
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            sellCoroutine ??= StartCoroutine(SellBodys(other.GetComponent<PlayerController>()));
        }
    }
}
