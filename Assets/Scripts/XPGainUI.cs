using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPGainUI : MonoBehaviour
{
    //variables to contain the prefab '+1 xP!', '+10 xp!' and 'Level Up!' objects
    [SerializeField] private GameObject prefabOneXP;
    [SerializeField] private GameObject prefabTenXP;
    [SerializeField] private GameObject prefabLevelUp;

    void Update()
    {
        //rotate the canvas to always face the main camera
        transform.forward = Camera.main.transform.forward;
    }

    public void OneXP()
    {
        StartCoroutine(ShowOneXP());
    }

    public void TenXP()
    {
        StartCoroutine(ShowTenXP());
    }

    public void LevelUp()
    {
        StartCoroutine(ShowLevelUp());
    }

    private IEnumerator ShowOneXP()
    {
        GameObject oneXP = Instantiate(prefabOneXP, transform.position, transform.rotation, gameObject.transform);

        yield return new WaitForSeconds(1.5f);

        Destroy(oneXP);
    }

    private IEnumerator ShowTenXP()
    {
        GameObject tenXP = Instantiate(prefabTenXP, transform.position, transform.rotation, gameObject.transform);

        yield return new WaitForSeconds(1.5f);

        Destroy(tenXP);
    }

    private IEnumerator ShowLevelUp()
    {
        GameObject oneXP = Instantiate(prefabLevelUp, transform.position, transform.rotation, gameObject.transform);

        yield return new WaitForSeconds(1.5f);

        Destroy(oneXP);
    }
}
