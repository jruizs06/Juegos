using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GEnerateLevel : MonoBehaviour
{

    public GameObject[] section;
    public int zPos = 20;
    public bool creatingSection = false;
    public int sectionNumber;
 

    void Update()
    {
         if(creatingSection == false){
            creatingSection = true;
            StartCoroutine(GenerateSection());
        }
    }

    IEnumerator GenerateSection(){
        sectionNumber = Random.Range(0,3);
        Instantiate(section[sectionNumber], new Vector3(0,0,zPos), Quaternion.identity);
        zPos +=40;
        yield return new WaitForSeconds(2);
        creatingSection = false;
    }
}
