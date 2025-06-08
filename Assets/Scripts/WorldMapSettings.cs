using UnityEngine;

public class WorldMapSettings : MonoBehaviour
{
    [Header("World Map Settings:")]
    public Vector2 WorldMapSize = new Vector2(160, 160);
    public Vector3 WorldMapCenter = Vector3.zero;
    public Vector2 ObjectPlacementZone;
    public int ObjectDistance;

    [Header("World Payouts:")]
    public int DeliveryPayout;
    public int LootDeliveryPayout;
    public int FishPayout;
    public int LootRandomPayout;

    [Header("World Events:")]
    public int DeliveryPortCount;
    public int FishPortCount;
    public int MaxRandomEventCount;
    public int MinRandomEventCountOnStart;
}