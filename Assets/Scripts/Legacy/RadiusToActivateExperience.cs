using System.Collections;
using UnityEngine;

public class RadiusToActivateExperience : MonoBehaviour
{
    //[SerializeField] GameObject xp;

    //bool isPlayerNear;


    //private void Awake()
    //{
    //    isPlayerNear = false;
    //}

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player"))
    //    {
    //        isPlayerNear = true;
    //        StartCoroutine(WaitForShape());
    //        takeExperienceScript.experienceAnim.SetTrigger("WhiteBlack");
    //        takeExperienceScript.experienceAnim.ResetTrigger("BlackWhite");
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player"))
    //    {
    //        isPlayerNear = false;
    //        takeExperienceScript.experienceAnim.SetTrigger("BlackWhite");
    //        takeExperienceScript.experienceAnim.ResetTrigger("WhiteBlack");
    //        takeExperienceScript.isBlack = false;
    //    }
    //}

    //private IEnumerator WaitForShape()
    //{
    //    yield return new WaitForSeconds(0.8f);
    //    if (isPlayerNear) takeExperienceScript.isBlack = true;
    //    else takeExperienceScript.isBlack = false;
    //}
}