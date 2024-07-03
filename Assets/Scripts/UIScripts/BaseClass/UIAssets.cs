using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "UIAssets", menuName = "UI/AssetsList")]
public class UIAssets : ScriptableObject
{
    private Dictionary<string, GameObject> _uiAssets = new Dictionary<string, GameObject>();
    [SerializeField]
    private GameObject[] _uiList = null;
    private void OnEnable()
    {
        if (_uiList == null)
            return;
        foreach (var item in _uiList)
        {
            if (item == null)
            {
                Debug.LogWarning($"UIAssets is null");
                continue;
            }
            var uiBase = item.GetComponent<UIBase>();
            if (uiBase == null)
            {
                Debug.LogWarning($"UI <{item}> UIBase is null");
                continue;
            }
            _uiAssets.Add(uiBase.UIName, item);
        }
    }
    public GameObject GetUIObj(string uIType)
    {
        if (!_uiAssets.TryGetValue(uIType, out var getValue))
        {
            Debug.LogError($"No found {uIType}");
            return null;
        }
        return getValue;
    }
}
