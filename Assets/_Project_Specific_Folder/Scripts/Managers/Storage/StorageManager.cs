using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageManager : StorageManagerBase
{
    [Title("Project Specific")]
    [ShowInInspector, PropertyOrder(10)]
    public int ExtendedVariablePersistent { get { return PlayerPrefs.GetInt(nameof(ExtendedVariablePersistent), 0); } set { PlayerPrefs.SetInt(nameof(ExtendedVariablePersistent), value); } }
    /*[ShowInInspector, PropertyOrder(11)]

    public int SelectedHead { get { return PlayerPrefs.GetInt(eItemType.Head.ToString(), 0); } *//*set { PlayerPrefs.SetInt(nameof(SelectedHead), value); }*//* }
    [ShowInInspector, PropertyOrder(12)]
    public int SelectedBody { get { return PlayerPrefs.GetInt(eItemType.Outfit.ToString(), 0); } *//*set { PlayerPrefs.SetInt(nameof(SelectedBody), value); } *//*}
    [ShowInInspector, PropertyOrder(13)]
    public int SelectedWeapon { get { return PlayerPrefs.GetInt(eItemType.Weapon.ToString(), 0); } *//*set { PlayerPrefs.SetInt(nameof(SelectedWeapon), value); } *//*}
*/
}

