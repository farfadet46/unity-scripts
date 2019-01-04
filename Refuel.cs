using UnityEngine;
using UnityEngine.UI;

namespace NWH.VehiclePhysics
{
    public class Refuel : MonoBehaviour
    {
        public bool IsInStation;
        public GameObject vehicle;
        public MeshFilter lamp;
        public Text label;
  
        void Start()
        {
            IsInStation = false;
            Recolor(false);
            label.text = "";
        }

        public void Update()
        {
            if (Input.GetButtonDown("RefuelVehicle"))
            {
                if (IsInStation)
                {
                    vehicle = GameObject.FindWithTag("Vehicle");

                    VehicleController otherScript = vehicle.GetComponent<VehicleController>();
                    otherScript.fuel.amount = otherScript.fuel.capacity;
                    label.text = "Le plein est fait. Merci de votre visite.";
                }
                else
                {
                    Debug.Log("vous devez entrer dans une station pour faire le plein");
                }
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Vehicle"))
            {
                IsInStation = true;
                Recolor(true);
                label.text = "Tu peut remplir ta voiture, aujourd'hui c'est gratuit !";
            }
            else if (other.CompareTag("Player"))
            {
                label.text = "Reviens avec une voiture ou un jerrycan !";
            }
        }
        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Vehicle"))
            {
                IsInStation = false;
                Recolor(false);
                label.text = "";
            }
        }
        void Recolor(bool col)
        {
            if (col)
            {
                lamp.gameObject.GetComponent<Renderer>().material.color = Color.red;
            }
            else
            {
                lamp.gameObject.GetComponent<Renderer>().material.color = Color.green;
            }
        }
    }
}