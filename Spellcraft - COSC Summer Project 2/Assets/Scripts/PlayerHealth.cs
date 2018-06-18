using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private int MaxHealth;
    private int CharacterHealth;

    //TIP: Add a slider for enchanced UX.

    public Slider healthBar;

    // Use this for initialization
    void Start()
    {
        MaxHealth = 100;
        CharacterHealth = MaxHealth;

        healthBar.value = computeHealth();
    }

    // Update is called once per frame
    void Update()
    {
        //if damage is dealt by the player decarese the health.
        if (Input.GetKeyDown(KeyCode.A)) //example: if keyboard 'A' is pressed the health of the player decareases.
            damagePlayerHealth(8); //this method decrease the player health by 8 units.
    }

    void damagePlayerHealth(int damageLevel)
    {
        if (CharacterHealth > 0) { 
        CharacterHealth -= damageLevel;
        healthBar.value = computeHealth();
    }
    else {
            KillPlayer();
    }
    }

    int computeHealth(){
        //this method computes health percentage.
            return CharacterHealth / MaxHealth;

    }

    void KillPlayer(){
        Debug.Log("You Died. Try again.");
    }
}
