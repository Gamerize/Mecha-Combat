using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Gun Stats")]
    public int magazineSize = 30;
    public int usableBullets = 270;
    public float fireRate = 0.1f;

    public Transform shootPoint;
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
        if(Input.GetMouseButton(0) && m_canFire && m_bulletCount > 0)
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
        if(reloadAmount >= m_bulletsRemain)
        {
            m_bulletCount += m_bulletsRemain;
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

        RaycastHit hitInfo;
        if (Physics.Raycast(shootPoint.position, shootPoint.forward, out hitInfo))
        {
             Debug.Log("hit: " + hitInfo.collider.name);
        }
            m_canFire = false;
        m_bulletCount--;

        yield return new WaitForSeconds(fireRate);
        m_canFire = true;
        Debug.Log("Ended firing");
    }
}
