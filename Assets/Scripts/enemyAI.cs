using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour , IDamage
{

    [Header("----- Components -----")]
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer model;

    [Header("----- Enemy stats -----")]
    [SerializeField] int HP;
    [SerializeField] int SpeedChase;
    [SerializeField] int facePlayerSpeed;
    [SerializeField] int sightDist;
    [SerializeField] int roamDist;
    [SerializeField] GameObject headPos;
    [SerializeField] int viewAngle;

    [Header("----- Gun stats -----")]
    [SerializeField] float shootRate;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject shootPos;

    bool isShooting;
    bool playerInRange;
    Vector3 playerDir;
    float stoppingDistOrig;
    Vector3 stratingPos;
    float angle;
    float speedPatrol;

    // Start is called before the first frame update
    void Start()
    {
       // gameManager.instance.enemyNumber++;
        //stoppingDistOrig = agent.stoppingDistance;
        //stratingPos = transform.position;
        //roam();
        //speedPatrol = agent.speed;
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(gameManager.instance.player.transform.position);
        //if (agent.enabled)
        //{
        //    if (playerInRange)
        //    {
        //        playerDir = gameManager.instance.player.transform.position - transform.position;
        //        angle = Vector3.Angle(playerDir, transform.forward);
        //        // canSeePlayer();
        //    }
        //}
        //else if (agent.remainingDistance < 0.1f && agent.destination != gameManager.instance.player.transform.position)
        //    roam();
    }

    public void takeDamage(int dmg)
    {
        HP -= dmg;
       // agent.SetDestination(gameManager.instance.player.transform.position);

        StartCoroutine(flashDamage());
        if (HP <= 0)
        {
           // gameManager.instance.checkEnemyTotal();
            Destroy(gameObject);
        }
    }

    void roam()
    {
        agent.stoppingDistance = 0;
        agent.speed = speedPatrol;

        Vector3 randomDir = Random.insideUnitSphere * roamDist;
        randomDir += stratingPos;

        NavMeshHit hit;
        NavMesh.SamplePosition(randomDir, out hit, 1, 1);
        NavMeshPath path = new NavMeshPath();

        agent.CalculatePath(hit.position, path);
        agent.SetPath(path);
    }

    //void canSeePlayer()
    //{

    //    RaycastHit hit;
    //    if (Physics.Raycast(headPos.transform.position, playerDir, out hit, sightDist))
    //    {
    //        Debug.DrawRay(headPos.transform.position, playerDir);
    //        if (hit.collider.CompareTag("Player") && angle <= viewAngle)
    //        {
    //            agent.speed = SpeedChase;
    //            agent.stoppingDistance = stoppingDistOrig;
    //            agent.SetDestination(gameManager.instance.player.transform.position);

    //            if (agent.remainingDistance < agent.stoppingDistance)
    //                facePlayer();

    //            if (!isShooting)
    //                StartCoroutine(shoot());
    //        }
    //    }
    //}


    //void facePlayer()
    //{
    //    playerDir.y = 0;
    //    Quaternion rotation = Quaternion.LookRotation(playerDir);
    //    transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * facePlayerSpeed);
    //}

    IEnumerator shoot()
    {
        isShooting = true;
        Instantiate(bullet, shootPos.transform.position, transform.rotation);

        yield return new WaitForSeconds(shootRate);
        isShooting = false;

    }

    IEnumerator flashDamage()
    {
        model.material.color = Color.red;
        //agent.speed = 0; samething underline
        agent.enabled = false;
        yield return new WaitForSeconds(0.25f);
        model.material.color = Color.white;
        agent.enabled = true;

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            agent.stoppingDistance = 0;
        }
    }


}
