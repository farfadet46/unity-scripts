using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cuisson_ingredient : MonoBehaviour
{
    public float timeNeeded = 100.0f; // temps neccessaire pour cuire cet ingrédient
    public CookingState cookingState = CookingState.Raw;
    public float timeCooked; //timer de cuisson effectuée sur cet ingredient
    public bool onFire; //Cet ingrédient est sur le feu
    public bool inHand; //cet ingredient est dans la main
    public bool attached; //cet ingredient est sur une assiette
    
    [SerializeField] private Material rawMaterial;
    [SerializeField] private Material wellDoneMaterial;
    [SerializeField] private Material overCookedMaterial;
    
    //couleur des particules...
    Color RawColor = new Color(1.0f, 0.96f, 0.88f, 1.0f);
    Color CookedColor = new Color(0.94f, 0.76f, 0.41f, 1.0f);
    Color OvercookedColor = new Color(0.2f, 0.2f, 0.2f, 1.0f);
    
    private AudioSource sound;
    
    public enum CookingState
    {
        Raw,
        Cooked,
        Overcooked,
        Charcoal
    }
    
    public delegate void CookingStateChangedHandler(CookingState newState);
    public event CookingStateChangedHandler CookingStateChanged;
    
    [SerializeField] private ParticleSystem smokeParticles;
    
    // Start is called before the first frame update
    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        
        if (!smokeParticles)
        {
            Transform child = transform.Find("Fume");
            if (child!= null)
            {
                smokeParticles = child.GetComponent<ParticleSystem>();
                
                if (smokeParticles != null)
                {
                    // Les particules ont été trouvées avec succès
                    Debug.Log("Les particules ont été trouvées.");
                }
                else
                {
                    // Aucun composant ParticleSystem trouvé sur l'enfant
                    Debug.LogWarning("Aucun composant ParticleSystem trouvé sur l'enfant.");
                }
            }
            else
            {
                // Aucun enfant trouvé avec le nom "Particules"
                Debug.LogWarning("Aucun enfant trouvé avec le nom 'Particules'.");
            }
        }
        smokeParticles.Stop();
        sound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!attached && transform.position.y < -1)
        {
            //si l'ingrédient trombe sous la map.
            transform.position = new Vector3(transform.position.x, 1, transform.position.z);
        }
        
        //attached = sur une assiete
        if (onFire)
        {
            if ( inHand == false && attached == false )
                //si n'est pas dans la main ni attaché à une assiette
            {            
                timeCooked += Time.deltaTime;
                if (timeCooked > timeNeeded)
                {
                    SetCookingState(CookingState.Overcooked);
                    Debug.Log("Cet ingrédient est trop cuit !");
                }
                else if (timeCooked < timeNeeded / 3)
                {
                    SetCookingState(CookingState.Raw);
                }
                else if (timeCooked > timeNeeded / 3 && timeCooked < (timeNeeded / 3) * 2)
                {
                    SetCookingState(CookingState.Cooked);
                }
                else if (timeCooked > timeNeeded + (timeNeeded / 3))
                {
                    SetCookingState(CookingState.Charcoal);
                }
            }
        }
        else
        {
            StopCooking();
        }
        /*
        if (inHand || attached )
        {
            
            StopCooking();
        }
        */
    }
    
    void SetCookingState(CookingState newState)
    {
        cookingState = newState;
        UpdateMaterial();
        if (CookingStateChanged != null)
        {
            CookingStateChanged(cookingState);
        }
    }
    
    public void StartCooking()
    {
        if ( !attached && !inHand)
        {
        onFire = true;
        smokeParticles.Play();
        sound.Play();
        }
    }
    
    public void StopCooking()
    {
        onFire = false;
        smokeParticles.Stop();
        sound.Stop();
    }
    
    void UpdateMaterial()
    {
        // Obtenez le composant Renderer attaché à cet objet
        

        if (GetComponent<Renderer>() != null) // Vérifiez si le composant Renderer est trouvé
        {
            // Vérifiez l'état de cuisson actuel pour décider quel matériau utiliser
            Material newMaterial = null; // Le nouveau matériau à utiliser
            var mainModule = smokeParticles.main;
            switch (cookingState)
            {
                case CookingState.Raw:
                    newMaterial = rawMaterial; // Utilisez le matériau cru
                    //SetParticleColor(1);
                    mainModule.startColor = RawColor;
                    break;
                case CookingState.Cooked:
                    newMaterial = wellDoneMaterial; // Utilisez le matériau cuit
                    //SetParticleColor(1);
                    mainModule.startColor = CookedColor;
                    break;
                case CookingState.Overcooked:
                    newMaterial = overCookedMaterial; // Utilisez le matériau trop cuit
                    //SetParticleColor(2);
                    mainModule.startColor = OvercookedColor;
                    break;
            }

            // Assurez-vous qu'un matériau approprié a été défini
            if (newMaterial != null)
            {
                // Changez le matériau de l'objet
                GetComponent<Renderer>().material = newMaterial;
            }
            else
            {
                Debug.LogError("Aucun matériau défini pour l'état de cuisson actuel !");
            }
        }
        else
        {
            Debug.LogError("Aucun composant Renderer trouvé sur cet objet !");
        }
    }
    
}