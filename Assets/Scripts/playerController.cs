using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    [Header("-----Components---------")]
    [SerializeField] CharacterController controller;

    [Header("-----Player Stats---------")]
    [Range(0, 10)][SerializeField] float playerSpeed ;
    [SerializeField] float jumpHeight ;
    [SerializeField] float gravityValue ;
    [SerializeField] int jumpsMax;

    [Header("------Gun stats-------")]
    [SerializeField] float shootRate;
    [SerializeField] int shootDist;
    [SerializeField] int shootDamage;
    [SerializeField] GameObject gunModel;
    [SerializeField] List<gunStats> gunStat = new List<gunStats> ();

    private Vector3 playerVelocity;
    private int timesJumped;
    bool isShooting;
    int selectGun;

    private void Start()
    {

    }

    void Update()
    {
        MoveMent();
        StartCoroutine(shoot());
        changeGun();
    }

    void MoveMent()
    {
        if (controller.isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
            timesJumped = 0;
        }

        Vector3 move = (transform.right * Input.GetAxis("Horizontal")) +
                       (transform.forward * Input.GetAxis("Vertical"));

        controller.Move(move * Time.deltaTime * playerSpeed);

        if (Input.GetButtonDown("Jump") && timesJumped < jumpsMax)
        {
            timesJumped++;
            playerVelocity.y = jumpHeight;
        }

        playerVelocity.y -= gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    IEnumerator shoot()
    {
        if (gunStat.Count > 0 && Input.GetButton("Shoot") && !isShooting)
        {
            isShooting = true;
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)), out hit, shootDist))
            {
                if (hit.collider.GetComponent<IDamage>() != null)
                    hit.collider.GetComponent<IDamage>().takeDamage(shootDamage);
            }
            yield return new WaitForSeconds(shootRate);
            isShooting = false;
        }
    }

    public void gunPickup(gunStats stats)
    {
        shootRate = stats.shootRate;
        shootDist = stats.shootDist;
        shootDamage = stats.shootDamage;
        gunModel.GetComponent<MeshFilter>().sharedMesh = stats.Model.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = stats.Model.GetComponent<MeshRenderer>().sharedMaterial;

        gunStat.Add(stats);
    }

    void changeGun()
    {
        if (gunStat.Count > 1)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectGun < gunStat.Count - 1)
            {
                ++selectGun;
                shootRate = gunStat[selectGun].shootRate;
                shootDist = gunStat[selectGun].shootDist;
                shootDamage = gunStat[selectGun].shootDamage;
                gunModel.GetComponent<MeshFilter>().sharedMesh = gunStat[selectGun].Model.GetComponent<MeshFilter>().sharedMesh;
                gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunStat[selectGun].Model.GetComponent<MeshRenderer>().sharedMaterial;
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0 && selectGun > 0)
            {
                --selectGun;
                shootRate = gunStat[selectGun].shootRate;
                shootDist = gunStat[selectGun].shootDist;
                shootDamage = gunStat[selectGun].shootDamage;
                gunModel.GetComponent<MeshFilter>().sharedMesh = gunStat[selectGun].Model.GetComponent<MeshFilter>().sharedMesh;
                gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunStat[selectGun].Model.GetComponent<MeshRenderer>().sharedMaterial;
            }
        }
    }

}
