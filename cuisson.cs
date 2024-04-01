using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cuisson : MonoBehaviour
{   
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ingredient"))
        {           
            cuisson_ingredient ingredientScript = other.GetComponent<cuisson_ingredient>();
            // Vérifier si le composant existe
            if (ingredientScript != null)
            {
                // Activer la cuisson de l'ingrédient
                ingredientScript.StartCooking();
            }
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ingredient"))
        {
            cuisson_ingredient ingredientScript = other.GetComponent<cuisson_ingredient>();
            // Vérifier si le composant existe
            if (ingredientScript != null)
            {
                // Désactiver la cuisson de l'ingrédient
                ingredientScript.StopCooking();
            }
        }
    }
}
