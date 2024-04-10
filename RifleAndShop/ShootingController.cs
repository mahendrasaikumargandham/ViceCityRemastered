using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class ShootingController : MonoBehaviour
{
    public Transform firePoint;
    public float fireRate = 0.5f;
    public float reloadTime = 2f;
    public int magazineCapacity = 30;
    public float rotationSpeed = 300f;
    public int maxAmmo = 30000;
    public float fireRange = 100f;
    public float giveDamageOf = 5f;

    private float nextFireTime;
    private int currentMagazine;
    private int currentAmmo;
    private bool isReloading;

    InputManager inputManager;
    GameManager gameManager;

    [Header("Rifle effects and sounds")]
    public ParticleSystem muzzleFlash;
    public GameObject bloodEffect;
    public GameObject justEffect;

    void Start() {
        inputManager = FindObjectOfType<InputManager>();
        gameManager = FindObjectOfType<GameManager>();
        currentMagazine = magazineCapacity;
        currentAmmo = maxAmmo;
    }

    void Update() {
        if(gameManager.useMobileInputs == true) {
            if(CrossPlatformInputManager.GetButton("Fire1") && Time.time >= nextFireTime && currentMagazine > 0 && !isReloading) {
                Shoot();
                nextFireTime = Time.time + 1f / fireRate;
            }

            if(CrossPlatformInputManager.GetButton("Reload") && currentMagazine < magazineCapacity && currentAmmo > 0 && !isReloading) {
                Debug.Log("Inside reload");
                StartCoroutine(Reload());
            }
        }
        else {
            if(inputManager.shootInput == true && Time.time >= nextFireTime && currentMagazine > 0 && !isReloading) {
                Shoot();
                nextFireTime = Time.time + 1f / fireRate;
            }

            if(inputManager.reloadInput == true && currentMagazine < magazineCapacity && currentAmmo > 0 && !isReloading) {
                Debug.Log("Inside reload");
                StartCoroutine(Reload());
            }
        }
    }

    void Shoot() {
        muzzleFlash.Play();
        RaycastHit hit;
        // Debug.Log("Inside Shoot Method");

        Vector3 cameraDirection = firePoint.transform.forward;
        cameraDirection.y = 0f; // Optional: Keep rotation only on the horizontal plane

        // Convert direction to rotation
        Quaternion targetRotation = Quaternion.LookRotation(cameraDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        if(Physics.Raycast(firePoint.position, firePoint.forward, out hit, fireRange)) {
            Debug.Log("Hitted: " + hit.transform.position);

            CharacterNavigatorScript characterNavigatorScript = hit.transform.GetComponent<CharacterNavigatorScript>();

            if(characterNavigatorScript != null) {
                characterNavigatorScript.CharacterHitDamage(giveDamageOf);
                GameObject bloodEffectGo = Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(bloodEffectGo, 4f);
            }

            Character character = hit.transform.GetComponent<Character>();

            if(character != null) {
                character.CharacterHitDamage(giveDamageOf);
                GameObject bloodEffectGo = Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(bloodEffectGo, 4f);
            }

            Guard guard = hit.transform.GetComponent<Guard>();

            if(guard != null) {
                guard.CharacterHitDamage(giveDamageOf);
                GameObject bloodEffectGo = Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(bloodEffectGo, 4f);
            }

            Diaz diaz = hit.transform.GetComponent<Diaz>();

            if(diaz != null) {
                diaz.CharacterHitDamage(giveDamageOf);
                GameObject bloodEffectGo = Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(bloodEffectGo, 4f);
            }

            Lance lance = hit.transform.GetComponent<Lance>();

            if(lance != null) {
                lance.CharacterHitDamage(giveDamageOf);
                GameObject bloodEffectGo = Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(bloodEffectGo, 4f);
            }

            GameObject justEffectGo = Instantiate(justEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(justEffectGo, 4f);
        }
        currentMagazine--;
    }

    IEnumerator Reload() {
        isReloading = true;
        int ammoToReload = Mathf.Min(magazineCapacity - currentMagazine, currentAmmo);
        yield return new WaitForSeconds(reloadTime);
        currentMagazine += ammoToReload;
        currentAmmo -= ammoToReload;

        if(currentAmmo < maxAmmo - magazineCapacity) {
            maxAmmo = currentAmmo + magazineCapacity;
        }

        isReloading = false;
    }
}
