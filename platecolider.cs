using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platecolider : MonoBehaviour
{
    public List<Rigidbody> objectsInTrigger = new List<Rigidbody>();
    
    void OnTriggerEnter(Collider other)
    {
        // Récupère le Rigidbody de l'objet entrant
        Rigidbody rb = other.GetComponent<Rigidbody>();

        // Vérifie si l'objet entrant a un Rigidbody
        if (rb != null && !objectsInTrigger.Contains(rb) && rb.gameObject.CompareTag("ingredient"))
        {
            cuisson_ingredient ingredientScript = rb.gameObject.GetComponent<cuisson_ingredient>();
            if ( !ingredientScript.attached )
            {
            // Ajoute l'objet à la liste
            objectsInTrigger.Add(rb);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Récupère le Rigidbody de l'objet sortant
        Rigidbody rb = other.GetComponent<Rigidbody>();

        // Vérifie si l'objet sortant est dans la liste
        if (rb != null && objectsInTrigger.Contains(rb))
        {
            // Supprime l'objet de la liste
            objectsInTrigger.Remove(rb);
        }
    }

    public void attacheItem()
    {        
        foreach (Rigidbody rb in objectsInTrigger)
        {
            // Enlève la gravité et passe en kinematic
            rb.useGravity = false;
            rb.isKinematic = true;
            rb.gameObject.transform.SetParent(this.gameObject.transform);
            
            cuisson_ingredient ingredientScript = rb.gameObject.GetComponent<cuisson_ingredient>();
            ingredientScript.attached = true;
            // mieux vaux prévoir ...
            ingredientScript.inHand=false;
            ingredientScript.onFire=false;
            ingredientScript.StopCooking();
        }
        
    }
}
