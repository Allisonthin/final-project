using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    // Start is called before the first frame update
    private void FinishAnim()
    {
        Destroy(gameObject);
    }
}
