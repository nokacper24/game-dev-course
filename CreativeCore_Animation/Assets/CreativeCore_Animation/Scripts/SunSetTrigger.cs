using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunSetTrigger : MonoBehaviour
{
    public Animator anim;

    private void OnTriggerEnter(Collider other)
    {
        anim.SetBool("inSunSetArea", true);
    }

    private void OnTriggerExit(Collider other)
    {
       anim.SetBool("inSunSetArea", false);
    }
}
