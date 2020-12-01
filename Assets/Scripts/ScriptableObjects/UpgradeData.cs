using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Upgrade", menuName = "Upgrade")]
public class UpgradeData : ScriptableObject {

    [SerializeField] public UpgradeData[] requirements;
    public Sprite sprite;
    public Effect effect;
    [TextArea] public string dex;
    
    public bool unlocked = false;
  
}
