using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealerScript : MonoBehaviour
{
    // Parameters
    private Vector3[] chipsSlot = new Vector3[10];
    
    // Chips that dealer owns
    public Stack dealerChips = new Stack();
    
    // Start is called before the first frame update
    void Start()
    {
        // Initialize the chipsSlot
        for (int i = 0; i < 5; i++)
        {
            chipsSlot[i] = new Vector3(-8+i, 4, 0);
            chipsSlot[i+5] = new Vector3(-8+i, 3.5f, 0);
        } 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Position a chip
    public void putChip(GameObject chip)
    {
        chip.transform.position = chipsSlot[dealerChips.Count];
    }
}
