using UnityEngine;

[CreateAssetMenu(fileName = "SmithObject", menuName = "ScriptableObjects/SmithObject")]

public class SmithObjectSO : ScriptableObject
{
    // An exception to makeing fields private, there is no write operations on scriptable objects so they can be public.
    public Transform prefab;
    public Sprite sprite;
    public string objectName;
}
