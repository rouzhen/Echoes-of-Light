using UnityEngine;

public class KeyInventory : MonoBehaviour
{
    private int keyCount = 0;

    public void AddKey()
    {
        keyCount++;
        Debug.Log($"Keys collected: {keyCount}");
    }

    public bool HasKey()
    {
        return keyCount > 0;
    }

    public void UseKey()
    {
        if (keyCount > 0)
        {
            keyCount--;
            Debug.Log($"Key used. Remaining keys: {keyCount}");
        }
    }

    public int GetKeyCount()
    {
        return keyCount;
    }
    public void ResetKeys()
    {
        keyCount = 0;
    }
}
