using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


public class EnemyController : MonoBehaviour
{

    public Animator anim;
    public EnemyState enemyState = EnemyState.live;
    public GameObject bones;

    private Rigidbody rb;
    private PlayerController playerController;

    private Coroutine deathCoroutine = null;

    public float distanceCanCollect = 5f;
    public float distanceToPunch = 3.5f;
    public bool canCollect = false;

    private void Awake()
    {
        //anim = GetComponent<Animator>();
        enemyState = EnemyState.live;
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        playerController = GameController.instance.playerObj.GetComponent<PlayerController>();
    }

    void Update()
    {
        if (enemyState == EnemyState.live)
        {
            LookToPlayer();
        }

        if (enemyState == EnemyState.death)
        {
            if (GameController.instance.DistanceBetweenObjects(playerController.transform.position, bones.transform.position) <= distanceCanCollect && canCollect)
            {
                CollectBody();
            }
        }
    }
    public void LookToPlayer()
    {

        Vector3 relativePos = playerController.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos);

        Quaternion current = transform.localRotation;

        transform.localRotation = Quaternion.Slerp(current, rotation, Time.deltaTime * 5);
    }

    public IEnumerator Death()
    {
        anim.enabled = false;
        enemyState = EnemyState.death;
        yield return new WaitForSeconds(0.5f);
        canCollect = true;
    }

    private void DesativeAllColliders()
    {
        List<Collider> colliders = bones.GetComponentsInChildren<Collider>().ToList();
        rb.isKinematic = true;
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        foreach (var col in colliders)
        {
            col.enabled = false;
        }
    }

    private void CollectBody()
    {
        if (GameController.instance.CurrentBodys >= GameController.instance.MaxBodys) return;

        enemyState = EnemyState.collected;
        DesativeAllColliders();
        playerController.AddBodyToBack();

        GameController.instance.enemiesOnScene.Remove(this.gameObject);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PunchCollider"))
        {
            if (enemyState != EnemyState.live) return;

            playerController.NormalPunch();
            deathCoroutine ??= StartCoroutine(Death());
        }
    }

}

public enum EnemyState
{
    live,
    death,
    collected
}
