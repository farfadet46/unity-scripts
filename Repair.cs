using System;
using UnityEngine;

namespace NWH.VehiclePhysics
{

    public class Repair : MonoBehaviour
    {
        public bool IsInGarage;
        public GameObject vehicle;

        void Start()
        {
            IsInGarage = false;

        }

        public void Update()
        {
            if (Input.GetButtonDown("RepairVehicle"))
            {
                if (IsInGarage)
                {
                    vehicle = GameObject.FindWithTag("Vehicle");

                    VehicleController otherScript = vehicle.GetComponent<VehicleController>();
                    otherScript.damage.Repair();
                }
                else
                {
                    Debug.Log("vous devez entrer dans un garage");
                }
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Vehicle"))
            {
                IsInGarage = true;
            }
            else if (other.CompareTag("Player"))
            {
                Debug.Log("Reviens avec une voiture à réparer !");
            }
        }
        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Vehicle"))
            {
                IsInGarage = false;
            }
        }
        
    }
}