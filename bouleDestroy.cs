using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bouleDestroy : MonoBehaviour
{
    [SerializeField] private float dureeVisible = 100f;
    
    void Update()
    {
        if (dureeVisible >0)
        {
            dureeVisible -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
