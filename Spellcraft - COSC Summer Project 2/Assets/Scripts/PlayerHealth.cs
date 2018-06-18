using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private int MaxHealth;
    private int CharacterHealth;

    //TIP: Add a slider for enchanced UX.

    public RectTransform healthBar;

    // Use this for initialization
    void Start()
    {
        MaxHealth = 100;
        CharacterHealth = MaxHealth;

        healthBar.localScale = new Vector3(computeHealth(), 1f, 1f);
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
            CharacterHealth = Mathf.Clamp(CharacterHealth, 0, MaxHealth);
            healthBar.localScale = new Vector3 (computeHealth(),1f,1f);
    }
    else {
            KillPlayer();
    }
    }

    float computeHealth(){
        //this method computes health percentage.
        return (float)CharacterHealth / MaxHealth;
    }

    void KillPlayer(){
        Debug.Log("You Died. Try again.");
    }
}
