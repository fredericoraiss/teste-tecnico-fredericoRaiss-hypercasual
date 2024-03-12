using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyCarried : MonoBehaviour
{
    public Transform upperBody;
    public GameObject bodyObj;
    Coroutine sellingCoroutine;
    public bool inSelling = false;

    public void Selling()
    {
        sellingCoroutine ??= StartCoroutine(Sell());
    }
    private IEnumerator Sell()
    {
        inSelling = true;
        bodyObj.SetActive(false);
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }

}
