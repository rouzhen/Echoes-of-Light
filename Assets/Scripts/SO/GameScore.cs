using UnityEngine;

[CreateAssetMenu(fileName = "GameScore", menuName = "ScriptableObjects/GameScore", order = 2)]
// GAME SCORE
public class GameScore : Variable<int>
{

    public int previousHighestValue;
    public override void SetValue(int value)
    {
        if (value > previousHighestValue) previousHighestValue = value;

        _value = value;
    }

    // overload
    public void SetValue(GameScore value)
    {
        SetValue(value.Value);
    }

    public void ApplyChange(int amount)
    {
        // this.Value += amount;
        SetValue(Value + amount);
    }

    public void ApplyChange(GameScore amount)
    {
        ApplyChange(amount.Value);
    }

    public void ResetHighestValue()
    {
        previousHighestValue = 0;
    }

}