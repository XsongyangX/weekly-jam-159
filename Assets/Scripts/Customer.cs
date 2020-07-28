﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script dictating how a customer interacts
/// </summary>
public class Customer : Person
{
    
    /// <summary>
    /// Reference to the customer behavior state machine
    /// </summary>
    public TileBase CashRegister;

    float themeRTPC = 80;

    /// <summary>
    /// Assigns references
    /// </summary>
    /// <param name="other"></param>
    /// 

    private void Start() {
        AkSoundEngine.PostEvent("ThemePlay", gameObject);
        AkSoundEngine.SetRTPCValue("Theme_RTPC", themeRTPC);
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
   
        

        switch (other.gameObject.name)
        {
            // colliding with something in the tables tilemap
            case "Tables":
                CollideTable(other);
                break;
        }
    }
    
    /// <summary>
    /// Sits at the table, influences the behavior machine
    /// </summary>
    /// <param name="other">Unity's detected collision</param>
    private void CollideTable(Collision2D other)
    {
        
        var tablesTilemap = other.gameObject.GetComponent<Tilemap>();
        var contact = other.GetContact(0); // first contact point
        
        // To get the tile, we need to get a world position inside a cell.
        // But the collision position is not inside the cell all the time.
        // So we'll lengthen the path taken by the customer by a step to get a
        // position inside the cell.
        
        // Find the position that is slightly ahead in the customer
        // Use the opposite direction of the normal of the surface of collision 
        var oppositeNormal = contact.normal.normalized * -1;

        var simulatedPosition = oppositeNormal * CollisionStep + contact.point;
        var cell = tablesTilemap.WorldToCell(simulatedPosition);


        // Change the touched tile to another tile
        tablesTilemap.SetTile(cell, CashRegister);
        float themeRTPC = 40;
        AkSoundEngine.SetRTPCValue("Theme_RTPC", themeRTPC);
        AkSoundEngine.PostEvent("CoinPay", gameObject);
    }
}
