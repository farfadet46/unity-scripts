using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attrap : MonoBehaviour
{
    public bool mainOQP = false;
    [HideInInspector] public Rigidbody RbObjet;
    [HideInInspector] public GameObject objet;
    [HideInInspector] public Color DebugRayColor = Color.red;
    public GameObject centerHand;

    [SerializeField] private float Distance = 4.0F;

    void Update()
    {
        //si main vide
        if (!mainOQP)
        {
            //créer un rayon de raycast
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0F));
            Debug.DrawRay(ray.origin, ray.direction * Distance, DebugRayColor);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Distance, LayerMask.GetMask("Default")) )
            {
                if (hit.collider.CompareTag("door"))
                {
                    if (Input.GetMouseButtonUp(0))
                    {
                        objet = hit.transform.gameObject;
                        doorOpen DoorScript = objet.GetComponent<doorOpen>();
                        DoorScript.ChangeEtat();
                    }
                }
                
                else if (hit.collider.CompareTag("plate") )
                {
                    if (Input.GetMouseButtonUp(0))
                    {
                        //récupération d'nfos sur l'objet cliqué
                        objet = hit.transform.gameObject;
                        RbObjet = objet.GetComponent<Rigidbody>();
                        platecolider PlateColider = objet.GetComponent<platecolider>();
                        
                        if ( PlateColider!=null)
                        {
                            PlateColider.attacheItem();
                            Debug.Log("les enfants sont en kinematic");
                        }
                        centerHand.transform.position = objet.transform.position;
                        //attrapper l'objet pour qu'il suive la main
                        objet.transform.SetParent(centerHand.transform);
                        
                        //modifier les attribus de l'objet
                        RbObjet.useGravity = false;
                        RbObjet.isKinematic = true;
                        Debug.Log("<color=GREEN><b> plate carrée Attrapée </b> </color>");
                        mainOQP = true;
                    }
                }   
                
                else if (hit.collider.CompareTag("tool") )
                {
                    if (Input.GetMouseButtonUp(0))
                    {
                        //récupération d'nfos sur l'objet cliqué
                        objet = hit.transform.gameObject;
                        RbObjet = objet.GetComponent<Rigidbody>();
                        
                        centerHand.transform.position = objet.transform.position;
                        //attrapper l'objet pour qu'il suive la main
                        objet.transform.SetParent(centerHand.transform);
                        
                        //modifier les attribus de l'objet
                        RbObjet.useGravity = false;
                        RbObjet.isKinematic = true;
                        Debug.Log("<color=GREEN><b> Tool Attrapé </b> </color>");
                        mainOQP = true;
                    }
                }
                
                else if (hit.collider.CompareTag("cuillerGlace"))
                {
                    if (Input.GetMouseButtonUp(0))
                    {
                        Debug.Log("mode glace activé !");
                        //récupération d'nfos sur l'objet cliqué
                        objet = hit.transform.gameObject;
                        RbObjet = objet.GetComponent<Rigidbody>();
                        
                        centerHand.transform.position = objet.transform.position;
                        //attrapper l'objet pour qu'il suive la main
                        objet.transform.SetParent(centerHand.transform);
                        
                        //modifier les attribus de l'objet
                        RbObjet.useGravity = false;
                        RbObjet.isKinematic = true;
                        Debug.Log("<color=GREEN><b> Cuillere a glace Attrapée </b> </color>");
                        mainOQP = true;
                    }
                }
                
                else if (hit.collider.CompareTag("ingredient"))
                {
                    if (Input.GetMouseButtonUp(0))
                    {
                        objet = hit.transform.gameObject;
                        RbObjet = objet.GetComponent<Rigidbody>();
                        //on récupère le script (pour savoir s'il est déjà attaché)
                        cuisson_ingredient ingredientScript = objet.GetComponent<cuisson_ingredient>();
                        //s'il est déjà sur une assiette, on peut l'enlever de l'assiette
                        if (ingredientScript != null && ingredientScript.attached)
                        {
                            Debug.Log("on le détache de l'assiette");
                            RbObjet.transform.SetParent(null); //s'il est déjà attaché, on le détache
                            ingredientScript.attached = false;
                        }
                        
                        Debug.Log("on le place sur la main");
                        //placer le child de la main au centre de l'objet (pour faciliter la rotation)
                        //on appliquera une rotation sur cet axe et non sur l'objet lui même (trop complexe)
                        centerHand.transform.position = objet.transform.position;
                        //attrapper l'objet pour qu'il suive la main
                        objet.transform.SetParent(centerHand.transform);
                        
                        //modifier les attribus de l'objet
                        RbObjet.useGravity = false;
                        RbObjet.isKinematic = true;
                        Debug.Log("<color=GREEN><b> Ingredient Attrapé </b> </color>");
                        mainOQP = true;
                        ingredientScript.inHand = true;
                    }
                }
            }
        }
        
        else //on a quelque chose en main
        {
            //lacher l'objet
            if (Input.GetMouseButtonUp(0) && mainOQP && objet != null)
            {
                Debug.Log("<color=yellow><b> Laché </b> </color>");
                // Détacher l'objet de son parent 
                objet.transform.SetParent(null);
               
               if (objet.GetComponent<Collider>().CompareTag("ingredient"))
                {
                    cuisson_ingredient ingredientScript = objet.GetComponent<cuisson_ingredient>();
                    ingredientScript.inHand = false;
                }
                // Réinitialiser les attributs de l'objet
                RbObjet.useGravity = true;
                RbObjet.isKinematic = false;
                // Réinitialiser la référence à l'objet et son Rigidbody
                objet = null;
                RbObjet = null;
                mainOQP = false;
            }
            
            if (Input.GetMouseButtonUp(1) && mainOQP && objet != null)
            {
                if (objet.GetComponent<Collider>().CompareTag("cuillerGlace"))
                {
                    //on recupere le script de la boule
                    remplirCuiller remplirCuillerScript = objet.GetComponent<remplirCuiller>();
                    if (remplirCuillerScript.rempli == true)
                    {
                        Debug.Log("on vide la cuiller");
                        remplirCuillerScript.videCuiller();
                    }
                }
            }
            
            //rotation de l'objet avec la molette de la souris
            if (objet != null)
            {
                float scroll = Input.GetAxis("Mouse ScrollWheel");
                if (scroll > 0f)
                {
                    centerHand.transform.Rotate(new Vector3(2, 0, 0));
                }
                else if (scroll < 0f)
                {
                    centerHand.transform.Rotate(new Vector3(-2, 0, 0));
                }
            }
        }
    }
}