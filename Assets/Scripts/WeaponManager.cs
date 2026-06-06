using UnityEngine;
using System.Collections;

public class WeaponManager : MonoBehaviour
{
    public GameObject playerCam;

    public float range = 300f;
    public float damage = 25f;

    public Animator playerAnimator;

    public ParticleSystem hitEffect;
    public UnityEngine.UI.Image hitMarker;

    public float shootCooldown = 0.25f;
    private float shootTimer = 0f;

    void Update()
    {
        shootTimer += Time.deltaTime;

        // Reset shooting animation
        if (playerAnimator.GetBool("isShooting"))
        {
            playerAnimator.SetBool("isShooting", false);
        }

        if (Input.GetButtonDown("Fire1") && shootTimer >= shootCooldown)
        {
            Shoot();
            shootTimer = 0f;
        }
    }

    void Shoot()
    {
        playerAnimator.SetBool("isShooting", true);

        RaycastHit hit;

        if (Physics.Raycast(playerCam.transform.position,
                            playerCam.transform.forward,
                            out hit,
                            range))
        {
            EnemyManager enemy = hit.transform.GetComponent<EnemyManager>();

            if (enemy != null)
            {
                float distance = Vector3.Distance(playerCam.transform.position, hit.point);

                float finalDamage = damage;

                // Distance damage reduction
                if (distance > 150f)
                    finalDamage *= 0.7f;

                // Headshot damage
                if (hit.collider.CompareTag("ZombieHead"))
                {
                    finalDamage *= 2f;
                }

                enemy.Hit(finalDamage);

                ShowHitEffect(hit.point);
                StartCoroutine(HitMarkerFlash());
            }
        }
    }

    void ShowHitEffect(Vector3 position)
    {
        if (hitEffect != null)
        {
            ParticleSystem effect = Instantiate(hitEffect, position, Quaternion.identity);
            Destroy(effect.gameObject, 1f);
        }
    }

    IEnumerator HitMarkerFlash()
    {
        if (hitMarker == null) yield break;

        Color c = hitMarker.color;
        c.a = 1f;
        hitMarker.color = c;

        yield return new WaitForSeconds(0.1f);

        c.a = 0f;
        hitMarker.color = c;
    }
}
