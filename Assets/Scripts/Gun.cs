using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    [Header("Gun Stats")]
    public int magazineSize = 30;
    public int usableBullets = 270;
    public float fireRate = 0.1f;

    //Gun Flashes
    public Image flashImage;
    public Sprite[] gunFlash; 

    public Transform shootPoint;
    [SerializeField] private GameObject[] m_bulletHoleSprites;

    //gun variables
    int m_bulletCount;
    int m_bulletsRemain;
    bool m_canFire;

    private void Start()
    {
        m_bulletCount = magazineSize;
        m_bulletsRemain = usableBullets;
        m_canFire = true;
    }

    private void Update()
    {
        if(Input.GetButton("Fire1") && m_canFire && m_bulletCount > 0)
        {
            StartCoroutine(FiringGun());
        }
        else if(Input.GetKeyDown(KeyCode.R) && m_bulletsRemain > 0 && m_bulletCount < magazineSize)
        {
            Reload();
        }
    }

    private void Reload()
    {
        int reloadAmount = magazineSize - m_bulletCount;
        if(reloadAmount >= m_bulletsRemain) //checking if the reload amount is more than the remaining amount of bullets
        {
            m_bulletCount += m_bulletsRemain; //reload the gun equal to the remaining amount of bullets
            m_bulletsRemain -= reloadAmount;
        }
        else 
        {
            m_bulletCount = magazineSize;
            m_bulletsRemain -= reloadAmount;
        }
    }

    IEnumerator FiringGun()
    {
        Debug.Log("Started firing");

        StartCoroutine(GunFlashing());

        RaycastHit hitInfo;
        if (Physics.Raycast(shootPoint.position, shootPoint.forward, out hitInfo)) //checking for colliders with raycast
        {
             Debug.Log("hit: " + hitInfo.collider.name);

            if(hitInfo.collider.tag == "Wall")
            {
                Debug.Log(m_bulletHoleSprites[Random.Range(0, m_bulletHoleSprites.Length)]);
                GameObject selectedBulletHole = m_bulletHoleSprites[Random.Range(0, m_bulletHoleSprites.Length)];

                Vector3 Adjust = new Vector3(hitInfo.point.x + 0.01f, hitInfo.point.y + 0.01f, hitInfo.point.z + 0.01f);
                Instantiate(selectedBulletHole, Adjust, Quaternion.LookRotation(hitInfo.normal));
            }
        }
            m_canFire = false;
        m_bulletCount--;

        yield return new WaitForSeconds(fireRate);
        m_canFire = true;
        Debug.Log("Ended firing");
    }

    IEnumerator GunFlashing()
    {
        flashImage.sprite = gunFlash[Random.Range(0, gunFlash.Length)]; //randomly choose a sprite
        flashImage.color = Color.white;
        yield return new WaitForSeconds(0.05f);
        flashImage.sprite = null;
        flashImage.color = new Color(0, 0, 0, 0);
    }
}
