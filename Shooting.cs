using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Shooting : MonoBehaviour
{
    public enum WeaponType { Pistol, Shotgun, SMG, Minigun }
    public WeaponType currentWeapon = WeaponType.Pistol;

    [Header("References")]
    [Tooltip("The child object that rotates to face the mouse.")]
    public Transform aimTransform;
    [Tooltip("The point where projectiles will spawn from.")]
    public Transform firePoint;
    
    [Header("Standard Projectile")]
    public GameObject projectilePrefab; 

    [Header("Minigun Settings")]
    public GameObject minigunPrefab; 
    public float minigunFireRate = 0.05f; 
    public float minigunDuration = 40f;   

    [Header("Weapon Stats")]
    public float pistolFireRate = 0.5f;
    public float shotgunFireRate = 1f;
    public int shotgunPelletCount = 5;
    public float shotgunSpreadAngle = 45f;
    public float shotgunPowerupDuration = 15f;

    public float smgFireRate = 0.1f;
    public float SMGPowerupDuration = 10f;
 
    private float nextFireTime = 0f;
    private Camera mainCamera;
    
 
    private Coroutine shotgunCoroutine;
    private Coroutine smgCoroutine;
    private Coroutine minigunCoroutine; 
    
    private Animator animator;

    void Start()
    {
        mainCamera = Camera.main;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Aim();
        HandleShooting();
    }

    public void ResetToDefaultWeapon()
    {
        if (shotgunCoroutine != null) { StopCoroutine(shotgunCoroutine); shotgunCoroutine = null; }
        if (smgCoroutine != null) { StopCoroutine(smgCoroutine); smgCoroutine = null; }
        if (minigunCoroutine != null) { StopCoroutine(minigunCoroutine); minigunCoroutine = null; } // Added

        currentWeapon = WeaponType.Pistol;
    }

    private void Aim()
    {
        if (aimTransform == null || mainCamera == null) return;
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        Vector2 direction = (mousePosition - aimTransform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        aimTransform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void HandleShooting()
    {
        if (Time.timeScale == 0f || EventSystem.current.IsPointerOverGameObject()) return;

        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            if (currentWeapon == WeaponType.Pistol)
            {
                FirePistol();
                nextFireTime = Time.time + pistolFireRate;
            }
            else if (currentWeapon == WeaponType.Shotgun)
            {
                FireShotgun();
                nextFireTime = Time.time + shotgunFireRate;
            }
            else if (currentWeapon == WeaponType.SMG)
            {
                Firesmg();
                nextFireTime = Time.time + smgFireRate;
            }
            else if (currentWeapon == WeaponType.Minigun) 
            {
                FireMinigun();
                nextFireTime = Time.time + minigunFireRate;
            }
        }
    }

    private void FirePistol()
    {
        if (animator != null) animator.SetTrigger("Shoot");
        InstantiateProjectile(aimTransform.rotation);
        AudioManager.instance.PlaySFX(AudioManager.instance.pistol);
    }

    private void FireShotgun()
    {
        if (animator != null) animator.SetTrigger("Shoot");
        for (int i = 0; i < shotgunPelletCount; i++)
        {
            float spread = Random.Range(-shotgunSpreadAngle / 2, shotgunSpreadAngle / 2);
            Quaternion spreadRotation = Quaternion.Euler(0, 0, aimTransform.eulerAngles.z + spread);
            InstantiateProjectile(spreadRotation);
        }
        AudioManager.instance.PlaySFX(AudioManager.instance.shotgun);
    }

    public void Firesmg()
    {
        if (animator != null) animator.SetTrigger("Shoot");
        InstantiateProjectile(aimTransform.rotation);
        AudioManager.instance.PlaySFX(AudioManager.instance.smg);
    }

    private void FireMinigun()
    {
        if (animator != null) animator.SetTrigger("Shoot");

        if (minigunPrefab != null && firePoint != null)
        {
            Instantiate(minigunPrefab, firePoint.position, aimTransform.rotation);
        }
        AudioManager.instance.PlaySFX(AudioManager.instance.Minigun); 
    }

    private void InstantiateProjectile(Quaternion rotation)
    {
        if (projectilePrefab == null || firePoint == null) return;

        GameObject projectileObj = Instantiate(projectilePrefab, firePoint.position, rotation);
        Projectile projectile = projectileObj.GetComponent<Projectile>();
        if (projectile != null)
        {
            projectile.SetVelocity(rotation * Vector2.right);
        }
    }


    public void ActivateShotgun()
    {
        StopAllPowerups();
        shotgunCoroutine = StartCoroutine(ShotgunPowerupRoutine());
    }

    public void ActivateSMG()
    {
        StopAllPowerups();
        smgCoroutine = StartCoroutine(smgPowerupRoutine());
    }

    public void ActivateMinigun() 
    {
        StopAllPowerups();
        minigunCoroutine = StartCoroutine(MinigunPowerupRoutine());
    }

    private void StopAllPowerups()
    {
        if (shotgunCoroutine != null) StopCoroutine(shotgunCoroutine);
        if (smgCoroutine != null) StopCoroutine(smgCoroutine);
        if (minigunCoroutine != null) StopCoroutine(minigunCoroutine);
    }
    
    private IEnumerator ShotgunPowerupRoutine()
    {
        currentWeapon = WeaponType.Shotgun;
        yield return new WaitForSeconds(shotgunPowerupDuration);
        currentWeapon = WeaponType.Pistol;
        shotgunCoroutine = null;
    }

     private IEnumerator smgPowerupRoutine()
    {
        currentWeapon = WeaponType.SMG;
        yield return new WaitForSeconds(SMGPowerupDuration);
        currentWeapon = WeaponType.Pistol;
        smgCoroutine = null;
    }

    private IEnumerator MinigunPowerupRoutine()
    {
        currentWeapon = WeaponType.Minigun;
        yield return new WaitForSeconds(minigunDuration); 
        currentWeapon = WeaponType.Pistol;
        minigunCoroutine = null;
    }
}