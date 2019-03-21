using GlobalOutline;
using UnityEngine;

public class OutlineTests : MonoBehaviour
{
    public GameObject[] ObjectsToApply;

    private void Start()
    {
        foreach (var gameObject in ObjectsToApply)
        {
            OutlineManager.Instance.AddGameObject(gameObject);
        }
    }
}
