using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Inventory : MonoBehaviour, ISaveManager
{
    public static Inventory instance;

    public List<ItemData> startingItems;

    public List<InventoryItem> equipment;
    public Dictionary<ItemData_Equament, InventoryItem> equipmentDictionary;

    public List<InventoryItem> inventory;
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;

    public List <InventoryItem> stash;
    public Dictionary<ItemData, InventoryItem> stashDictionary;

    [Header("Inventory UI")]
    [SerializeField] private Transform stashSlotParent;
    [SerializeField] private Transform inventorySloParent;
    [SerializeField] private Transform equipmentSlotParent;
    [SerializeField] private Transform statsSlotParent;

    private UI_Itemslot[] inventoryitemslots;
    private UI_Itemslot[] stashItemSlot;
    private UI_EquipmentSlot[] equipmentSlot;
    private UI_StatsSlot[] statsslot;

    [Header("Items cooldown")]
    private float lastTimeUsedFlask;
    private float lastTimeUsedArmor;

    public float flaskCooldown {  get; private set; }
    private float armorCooldown;

    [Header("Data base")]
    public List<ItemData_Equament> loadeEquipment; 
    public List<InventoryItem> loadedItems;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        inventory = new List<InventoryItem>();
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();

        stash = new List<InventoryItem>();
        stashDictionary = new Dictionary<ItemData, InventoryItem>();

        equipment = new List<InventoryItem>();
        equipmentDictionary = new Dictionary<ItemData_Equament, InventoryItem>();

        inventoryitemslots = inventorySloParent.GetComponentsInChildren<UI_Itemslot>();
        stashItemSlot = stashSlotParent.GetComponentsInChildren<UI_Itemslot>();
        equipmentSlot = equipmentSlotParent.GetComponentsInChildren<UI_EquipmentSlot>();
        statsslot = statsSlotParent.GetComponentsInChildren<UI_StatsSlot>();

        AddStartingItem();
    }

    private void AddStartingItem()
    {
        foreach(ItemData_Equament item in loadeEquipment)
        {
            EquipItem(item);
        }



        if(loadedItems.Count > 0)
        {
          foreach(InventoryItem item in loadedItems)
            {
                for(int i = 0;i<item.stackSize; i++)
                {
                    AddItem(item.data);
                }
            }
            return;
        }


        for (int i = 0; i < startingItems.Count; i++)
        {
            if(startingItems[i] != null)
               AddItem(startingItems[i]);
        }
    }

    public void EquipItem(ItemData _item)
    {
        ItemData_Equament newEquipment = _item as ItemData_Equament;
        InventoryItem newItem = new InventoryItem(newEquipment);

        ItemData_Equament oldEquipment = null;
        foreach (KeyValuePair<ItemData_Equament, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == newEquipment.equipmentType)
            {
                oldEquipment = item.Key;
            }
        }
        if (oldEquipment != null)
        {
            Unequipment(oldEquipment);
            AddItem(oldEquipment);
        }
        equipment.Add(newItem);
        equipmentDictionary.Add(newEquipment, newItem);
        newEquipment.AddModifiers();
        RemoveItem(_item);

        UpdateSlotUI();
    }

    public  void Unequipment(ItemData_Equament itemToRemove)
    {
        if (equipmentDictionary.TryGetValue(itemToRemove, out InventoryItem value))
        {
            equipment.Remove(value);
            equipmentDictionary.Remove(itemToRemove);
            itemToRemove.RemoveModifiers();
        }
    }

    private void UpdateSlotUI()
    {

        for (int i = 0; i < equipmentSlot.Length; i++)
        {
            foreach (KeyValuePair<ItemData_Equament, InventoryItem> item in equipmentDictionary)
            {
                if (item.Key.equipmentType == equipmentSlot[i].slotType)
                {
                    equipmentSlot[i].UpdateSlot(item.Value);
                }
            }
        }

        for (int i = 0; i < inventoryitemslots.Length; i++)
        {
            inventoryitemslots[i].CleanUpSlot();

        }
        for (int i = 0; i < stashItemSlot.Length; i++)
        {
            stashItemSlot[i].CleanUpSlot();
        }


        for (int i = 0; i < inventory.Count; i++)
        {
            inventoryitemslots[i].UpdateSlot(inventory[i]);
        }

        for (int i = 0; i < stash.Count; i++)
        {
            stashItemSlot[i].UpdateSlot(stash[i]);
        }

        UpdateStatsUI();
    }

    public void UpdateStatsUI()
    {
        for (int i = 0; i < statsslot.Length; i++)
        {
            statsslot[i].UpdateStatValueUI();
        }
    }

    public void AddItem(ItemData _item)
    {
        if (_item.itemType == ItemType.Equipment&&CanAddItem())
        {
          AddToInventory(_item);
        }
        else if (_item.itemType == ItemType.Material)
        {
            AddToStash(_item);
        }
        UpdateSlotUI();
    }

    private void AddToStash(ItemData _item)
    {
        if (stashDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            stash.Add(newItem);
            stashDictionary.Add(_item, newItem);
        }
    }

    private void AddToInventory(ItemData _item)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            inventory.Add(newItem);
            inventoryDictionary.Add(_item, newItem);
        }
    }

    public void RemoveItem(ItemData _item)
    {
        if(inventoryDictionary.TryGetValue(_item,out InventoryItem value))
        {
            if (value.stackSize <= 1)
            {
                inventory.Remove(value);
                inventoryDictionary.Remove(_item);
            }
            else
                value.RemoveStack();
        }

        if(stashDictionary.TryGetValue(_item,out InventoryItem stashValue))
        {
            if(stashValue.stackSize <= 1)
            {
                stash.Remove(stashValue);
                stashDictionary.Remove(_item);
            }
            else
            {
                stashValue.RemoveStack();
            }
        }

        UpdateSlotUI() ;
    }

    public bool CanAddItem()
    {
        if(inventory.Count>=inventoryitemslots.Length)
        {
            Debug.Log("没有更多空间了");
            return false;
        }
        return true;
    }
    public bool CanCraft(ItemData_Equament _itemToCraft,List<InventoryItem> _requireMaterials)
    {
        List<InventoryItem> materialsToRemove = new List<InventoryItem>();

        for (int i = 0; i < _requireMaterials.Count; i++)
        {
            if (stashDictionary.TryGetValue(_requireMaterials[i].data,out InventoryItem stashValue))
            {
                if (stashValue.stackSize < _requireMaterials[i].stackSize)
                {
                    Debug.Log("not enough materials");
                    return false;
                }
                else
                {
                    materialsToRemove.Add(stashValue);
                }
            }
            else
            {
                Debug.Log("not enough materials");
                return false;
            }
        }

        for (int i = 0; i < materialsToRemove.Count; i++)
        {
            RemoveItem(materialsToRemove[i].data);
        }
        AddItem(_itemToCraft);
        Debug.Log("Here is your item" + _itemToCraft.name);
        return true;
    }

    public List<InventoryItem> GetEquipmentList() => equipment;

    public List<InventoryItem>GetStashList()=> stash;


    public ItemData_Equament GetEquipment(EquipmentType _type)
    {
        ItemData_Equament equipedItem = null;

        foreach (KeyValuePair<ItemData_Equament, InventoryItem> item in equipmentDictionary)//遍历装备字典
        {
            if (item.Key.equipmentType == _type)//如果装备类型相同
                equipedItem = item.Key;//删除该装备
        }

        return equipedItem;

    }

    public void UseFlask()
    {
        ItemData_Equament currentFlask = GetEquipment(EquipmentType.Flask);

        if (currentFlask == null)
        {
            return;
        }

        bool canUseFlask = Time.time > lastTimeUsedFlask + flaskCooldown;


        if (canUseFlask)
        {
            flaskCooldown = currentFlask.itemCooldown;
            currentFlask.Effect(null); 
            lastTimeUsedFlask= Time.time;
        }
        else
        {
            Debug.Log("Flask on cooldown");
        }
    }

    public bool CanUseArmor()
    {
        ItemData_Equament currentArmor = GetEquipment(EquipmentType.Armor);
        if(Time.time> lastTimeUsedArmor+armorCooldown)
        {
            armorCooldown = currentArmor.itemCooldown;
            lastTimeUsedArmor= Time.time;
            return true;
        }
        Debug.Log("Armor on cooldown");
        return false;

    }

    public void LoadData(GameData _data)
    {
        foreach(KeyValuePair<string,int>pair in _data.inventory)
        {
            foreach(var item in GetItemDataBase())
            {
                if(item!=null && item.itemId == pair.Key)
                {
                    InventoryItem itemToLoad=new InventoryItem(item);
                    itemToLoad.stackSize = pair.Value;

                    loadedItems.Add(itemToLoad);
                }
            }
        }

        foreach (string loadedItemId in _data.equimentId)
        {
            foreach (var item in GetItemDataBase())
            {
                if (item != null && loadedItemId== item.itemId)
                {
                    loadeEquipment.Add(item as ItemData_Equament);
                }
            }
        }

    }

    public void SaveData(ref GameData _data)
    {
       _data.inventory.Clear();
        _data.equimentId.Clear();

        foreach (KeyValuePair<ItemData, InventoryItem> pair in inventoryDictionary)
        {
            _data.inventory.Add(pair.Key.itemId,pair.Value.stackSize);
        }
        foreach (KeyValuePair<ItemData, InventoryItem> pair in stashDictionary)
        {
            _data.inventory.Add(pair.Key.itemId, pair.Value.stackSize);
        }
        foreach (KeyValuePair<ItemData_Equament, InventoryItem> pair in equipmentDictionary)
        {
            _data.equimentId.Add(pair.Key.itemId);
        }
    }


    private List<ItemData> GetItemDataBase()
    {
        List<ItemData> itemDataBase = new List<ItemData>();
        string[] assrtNames = AssetDatabase.FindAssets("", new[] {"Assets/Data/Items"});

        foreach(string SOName in assrtNames)
        {
            var SOpath = AssetDatabase.GUIDToAssetPath(SOName);
            var itemData = AssetDatabase.LoadAssetAtPath<ItemData>(SOpath);
            itemDataBase.Add(itemData);
        }
        return itemDataBase;
    }
}
