using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunHandler : MonoBehaviour
{
    public GameObject gun;
    public LineRenderer gunLr;
    void Update()
    {
        gunLr.SetPosition(0, gun.transform.position);
        gunLr.SetPosition(1, gun.transform.position + gun.transform.forward * 10);
    }
}
