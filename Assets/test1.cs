using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test1 : MonoBehaviour
{
    [SerializeField] Material material;


   
    // Start is called before the first frame update
    void Start()
    {
        material.SetFloat("_Effect",1f);
    }
}
