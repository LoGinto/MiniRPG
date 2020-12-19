using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BetterInventory : MonoBehaviour
{
    #region Encapsulation
    [SerializeField] KeyCode inventoryKeycode = KeyCode.I;
    [SerializeField] CanvasGroup inventoryPanelCanvasGroup;
    [SerializeField] CanvasGroup weaponsCanvasGroup;
    [SerializeField] CanvasGroup clothCanvasGroup;
    [SerializeField] CanvasGroup consumableCanvasGroup;
    [SerializeField] Button weaponButtonInstance;
    [SerializeField] Button clothButtonInstance;
    [SerializeField] Button consumableThrowableButton;
    [SerializeField] Image currentConsumableImage;
    bool openedInventory = false;
    bool openedWeaponInventory = false;
    bool openedClothInventory = false;
    bool openedConsumableInventory = false;
    [HideInInspector] BetterFighter betterFighter;
    private ClotheObject clothOnHead;
    private ConsumableObject currentEquippedConsumable;
    public List<ClotheObject> clothes = new List<ClotheObject>();
    public List<ConsumableObject> consumables = new List<ConsumableObject>();
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        betterFighter = FindObjectOfType<BetterFighter>();
        if (consumables != null)
        {
            currentEquippedConsumable = consumables[0];
        }
    }
    // Update is called once per frame
    void Update()
    {
        #region CanvasToggling
        if (Input.GetKeyDown(inventoryKeycode))
        {
            openedClothInventory = false;
            openedWeaponInventory = false;
            openedConsumableInventory = false;
            openedInventory = !openedInventory;
        }
        
        ActivateCanvasGroup(inventoryPanelCanvasGroup, openedInventory);
        ActivateCanvasGroup(weaponsCanvasGroup, openedWeaponInventory);
        ActivateCanvasGroup(clothCanvasGroup, openedClothInventory);
        ActivateCanvasGroup(consumableCanvasGroup, openedConsumableInventory);
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ActivateCanvasGroup(weaponsCanvasGroup, false);
            ActivateCanvasGroup(clothCanvasGroup, false);
            ActivateCanvasGroup(consumableCanvasGroup, false);
            openedWeaponInventory = false;
            openedConsumableInventory = false;
            ActivateCanvasGroup(inventoryPanelCanvasGroup, false);
            DestroyInstances(weaponsCanvasGroup.transform);
            DestroyInstances(clothCanvasGroup.transform);
            ActivateCanvasGroup(inventoryPanelCanvasGroup, false);
            ActivateCanvasGroup(consumableCanvasGroup,false);
        }
        #endregion
        if (currentEquippedConsumable != null)
        {
            currentConsumableImage.GetComponent<Image>().sprite = currentEquippedConsumable.icon;
        }
    }
    public void OpenWeaponsInventory()
    {
        openedInventory = false;
        openedWeaponInventory = true;
        openedClothInventory = false;
        openedConsumableInventory = false;
        ActivateCanvasGroup(weaponsCanvasGroup, true);
        int index = 0;
        foreach (WeaponObject weapon in betterFighter.weaponsInBackPack)
        {
            Button instance = Instantiate(weaponButtonInstance, weaponsCanvasGroup.transform);
            instance.image.sprite = weapon.icon;
            instance.GetComponent<WeaponIndexHolder>().weaponToClickIndex = index;
            index++;
        }
    }
    public void OpenConsumableInventoryToPlayer()
    {
        openedInventory = false;
        openedWeaponInventory = false;
        openedClothInventory = false;
        openedConsumableInventory = true;
        ActivateCanvasGroup(consumableCanvasGroup, true);
        int index = 0;
        foreach (ConsumableObject consumable in consumables)     
        {
            Button instance = Instantiate(weaponButtonInstance, weaponsCanvasGroup.transform);
            instance.image.sprite = consumable.icon;
            instance.GetComponent<WeaponIndexHolder>().consumableToClickIndex = index;
            index++;
        }
    }
    public void OpenClothInventor()
    {
        openedInventory = false;
        openedWeaponInventory = false;
        openedConsumableInventory = false;
        openedClothInventory = true;
        ActivateCanvasGroup(clothCanvasGroup, true);
        int index = 0;
        foreach (ClotheObject cloth in clothes)
        {
            Button instance = Instantiate(clothButtonInstance, clothCanvasGroup.transform);
            instance.image.sprite = cloth.icon;
            instance.GetComponent<WeaponIndexHolder>().clothToClickIndex = index;
            index++;
        }
    }
    void DestroyInstances(Transform parent)
    {
        foreach (Transform child in parent.transform)
        {
            Destroy(child.gameObject);
        }
    }
    public bool WeaponsInventoryIsOpen()
    {
        return openedWeaponInventory;
    }
    public bool InventoryIsOpen()
    {
        return openedInventory;
    }
    public bool ClothInventoryIsOpen()
    {
        return openedClothInventory;
    }
    public void EquipClothFromInventory(int indexToEquip)
    {
        if(clothes[indexToEquip].bodyPart == ClotheObject.BodyPart.head)
        {
           if(clothOnHead != null)
            {
                Destroy(clothOnHead.GetClothInstance());
            }
            clothOnHead = clothes[indexToEquip];
            clothOnHead.EquipCloth(this.gameObject.transform);             
        }
        else if(clothes[indexToEquip].bodyPart == ClotheObject.BodyPart.empty)
        {
            //destroy all clothes
            if (clothOnHead != null)
            {
                Destroy(clothOnHead.GetClothInstance());
            }
        }
    }
    public void EquipConsumable(int indexToEquip)
    {
        currentEquippedConsumable = consumables[indexToEquip];
    }
    public ConsumableObject GetCurrentConsumable()
    {
        return currentEquippedConsumable;
    }
    void ActivateCanvasGroup(CanvasGroup canvasGroup,bool isOn)
    {
        if(isOn == true)
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1f;
        }
        else
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0;
        }
    }
}

