using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowActiveWeaponsUI : MonoBehaviour
{
    [SerializeField] GameObject[] weaponsUI;
    private void Update()
    {
        if(weaponsUI!= null && weaponsUI.Length > 0) 
        {
            UpdateActiveWeaponsUI();
        }  
    }
    public void UpdateActiveWeaponsUI()
    {
        foreach(GameObject weaponUI in weaponsUI)
        {
            if(weaponUI != null && weaponUI.GetComponent<ArsenalDisplay>()!= null)
            {
                if (weaponUI.GetComponent<ArsenalDisplay>().weaponToDisplay.Level > 0)
                {
                    weaponUI.SetActive(true);
                }
                else
                {
                    weaponUI.SetActive(false);

                }
            }
            
        }
    }
    
}
