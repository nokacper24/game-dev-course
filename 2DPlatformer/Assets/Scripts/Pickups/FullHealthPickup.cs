using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullHealthPickup : Pickup
{
    public override void DoOnPickup(Collider2D collision) {
        if(collision.tag=="Player" && collision.gameObject.GetComponent<Health>()!=null) {
            Health playerHaalth = collision.gameObject.GetComponent<Health>();
            playerHaalth.ReceiveHealing(playerHaalth.maximumHealth);
            base.DoOnPickup(collision);
        }
    }

}
