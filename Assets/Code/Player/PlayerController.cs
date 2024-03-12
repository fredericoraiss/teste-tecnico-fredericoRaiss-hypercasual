using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    [Header("---GENERAL")]
    public float moveSpeed = 10;

    public GameObject playerObject;
    public Material bodyMaterial;
    private Rigidbody rb;
    private Animator anim;
    Vector2 moveInputValues;

    [Header("---PUNCH")]
    public GameObject punchObject;
    [SerializeField] private bool isPunching = false;

    [Header("---BODYS")]
    public Transform bodyLocal;
    public GameObject bodyPrefab;
    public List<GameObject> bodysStack = new();
    private Vector3 smoothBodyVelovity = Vector3.zero;

    [Header("---TEXT")]
    public TMP_Text levelUpText;
    private Coroutine levelUpTextCoroutine = null;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        isPunching = false;
        levelUpText.gameObject.SetActive(false);
    }
    void Start()
    {
        //DisablePunchCollider();
        bodyMaterial.color = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        if (bodysStack.Count > 0)
        {
            InertiaBodysEffect();
        }
    }

    public void InputMoveAction(InputAction.CallbackContext ctx)
    {
        moveInputValues = ctx.ReadValue<Vector2>();
    }

    private void Move()
    {
        Vector3 movement = new(moveInputValues.x, 0, moveInputValues.y);
        Vector3 rotation = new(moveInputValues.x * moveSpeed, rb.velocity.y, moveInputValues.y * moveSpeed);

        Vector3 normalSpeed = new(movement.x * moveSpeed, rb.velocity.y, movement.z * moveSpeed);
        Vector3 punchSpeed = normalSpeed / 2;

        rb.velocity = isPunching ? punchSpeed : normalSpeed;

        if (movement.x != 0 || movement.z != 0)
        {
            transform.rotation = Quaternion.LookRotation(rotation);
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }
    }

    public void NormalPunch()
    {
        isPunching = true;
        anim.SetTrigger("Punch");
    }

    public void ChangeColorToRandonValue()
    {
        bodyMaterial.color = new Color(UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(0, 1f), UnityEngine.Random.Range(0, 1f));
    }

    public void FinalizePunch() { isPunching = false; }

    public void AddBodyToBack()
    {
        if (bodysStack.Count >= GameController.instance.MaxBodys) return; //TODO: adicionar visual negação

        GameObject body = Instantiate(bodyPrefab);
        bodysStack.Add(body);
        GameController.instance.CurrentBodys = bodysStack.Count;

        body.transform.position = bodyLocal.position;

        GameController.instance.uiController.updateText.Invoke();
    }

    public void CallLevelUpText()
    {
        levelUpTextCoroutine ??= StartCoroutine(LevelUpTextCoroutine());
    }

    private IEnumerator LevelUpTextCoroutine()
    {
        levelUpText.transform.LookAt(levelUpText.transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
        levelUpText.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.65f);
        levelUpText.gameObject.SetActive(false);

        levelUpTextCoroutine = null;
    }

    private void InertiaBodysEffect()
    {
        for (int i = 0; i < bodysStack.Count; i++)
        {
            if (bodysStack[i].GetComponent<BodyCarried>().inSelling) return;

            if (i == 0) bodysStack[i].transform.position = bodyLocal.position;

            if (i > 0)
            {
                Transform futurePos = bodysStack[i - 1].GetComponent<BodyCarried>().upperBody;
                Vector3 newPos = Vector3.SmoothDamp(bodysStack[i].transform.position, futurePos.position, ref smoothBodyVelovity, 0.026f);
                bodysStack[i].transform.position = newPos;

            }
        }
    }
}
