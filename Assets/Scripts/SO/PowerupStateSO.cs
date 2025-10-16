using UnityEngine;

[CreateAssetMenu(fileName = "PowerupState", menuName = "ScriptableObjects/PowerupState", order = 3)]
public class PowerupStateSO : ScriptableObject {
    public PowerupType previousHighestPowerup;
    protected PowerupType _value = PowerupType.Small;


    public PowerupType Value
    //read or set powerup state from other scripts
    {
        get { return _value; }
        set { SetValue(value); }
    }

     public virtual void SetValue(PowerupType value) {
        //sets the current powerup state
        // if new is "higher" than previous, update previous
        if ((int)value > (int)previousHighestPowerup) previousHighestPowerup = value;
        _value = value;
    }

    // Overload for setting from another SO
    public void SetValue(PowerupStateSO other) {
        SetValue(other.Value);
    }

    // Reset to default
    public void ResetHighestPowerup() {
        previousHighestPowerup = PowerupType.Small;
        SetValue(PowerupType.Small);
    }   
}