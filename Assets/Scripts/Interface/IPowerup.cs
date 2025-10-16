using UnityEngine;

public interface IPowerup
{
    void DestroyPowerup();
    //void SpawnPowerup();
    void ApplyPowerup(MonoBehaviour i);

    PowerupType powerupType
    {
        get;
    }

    /*bool hasSpawned
    {
        get;
    }*/
}


public enum PowerupType
{
    Small = 1,
    MagicMushroom = 2,
    FireFlower = 3,
    StarMan = 4
}

public interface IPowerupApplicable
{
    public void RequestPowerupEffect(IPowerup i);
}