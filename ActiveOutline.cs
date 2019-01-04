using UnityEngine;

namespace cakeslice
{
    public class ActiveOutline : MonoBehaviour
    {
        [HideInInspector] public bool EnMain;
        [HideInInspector] public Rigidbody RbObjet;
        [HideInInspector] public GameObject objet;
        [HideInInspector] public Color DebugRayColor = Color.red;
        public GameObject centerHand;

        public float Distance = 4.0F;

        void Update()
        {
            //si main vide
            if (!EnMain)
            {
                //créer un rayon de raycast
                Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0F));
                Debug.DrawRay(ray.origin, ray.direction * Distance, DebugRayColor);

                RaycastHit hit;
                //si le Hit est avéré et l'objet a le tag "pickable"
                if (Physics.Raycast(ray, out hit, Distance) && hit.collider.tag == "Pickable")
                {
                    //appliquer le outline
                    Outline ObjOut = hit.rigidbody.gameObject.GetComponent<Outline>();
                    ObjOut.eraseRenderer = true;

                    if (Input.GetMouseButtonUp(0))
                    {
                        //récupération d'nfos sur l'objet cliqué
                        objet = hit.transform.gameObject;
                        RbObjet = objet.GetComponent<Rigidbody>();
                        
                        //placer le child de la main au centre de l'objet (pour faciliter la rotation)
                        //on appliquera une rotation sur cet axe et non sur l'objet lui même (trop complexe)
                        centerHand.transform.position = objet.transform.position;
                        //attrapper l'objet pour qu'il suive la main
                        objet.transform.SetParent(centerHand.transform);
                        
                        //modifier les attribus de l'objet
                        RbObjet.useGravity = false;
                        RbObjet.isKinematic = true;
                        Debug.Log("<color=GREEN><b> Attrapé </b> </color>");
                        EnMain = true;
                        return; //quitter la boucle
                    }
                }
            }
            
            else
            {
                //lacher l'objet
                if (Input.GetMouseButtonUp(0))
                {
                    Debug.Log("<color=yellow><b> Laché </b> </color>");
                    Rigidbody rb = transform.GetComponentInChildren<Rigidbody>();
                    rb.transform.parent = null;
                    //reset des attributs de l'objet
                    RbObjet.useGravity = true;
                    RbObjet.isKinematic = false;
                    EnMain = false;
                }

                //rotation du child de la main (et par conséquent de l'objet)
                if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
                {
                    centerHand.transform.Rotate(new Vector3(1, 0, 0));
                }

                else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
                {
                    centerHand.transform.Rotate(new Vector3(-1, 0, 0));
                }
            }
        }
    }
}
