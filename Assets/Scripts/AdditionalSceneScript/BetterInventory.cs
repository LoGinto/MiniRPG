using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BetterInventory : MonoBehaviour
{
    [SerializeField] KeyCode inventoryKeycode = KeyCode.I;
    [SerializeField] CanvasGroup inventoryPanelCanvasGroup;
    [SerializeField] CanvasGroup weaponsCanvasGroup;
    [SerializeField] CanvasGroup clothCanvasGroup;
    [SerializeField] Button weaponButtonInstance;
    [SerializeField] Button clothButton;
    bool openedInventory = false;
    bool openedWeaponInventory = false;
    bool openedClothInventory = false;
    [HideInInspector] BetterFighter betterFighter;
    private ClotheObject clothOnHead;
    public List<ClotheObject> clothes = new List<ClotheObject>();
    // Start is called before the first frame update
    void Start()
    {
        betterFighter = FindObjectOfType<BetterFighter>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(inventoryKeycode))
        {
            openedClothInventory = false;
            openedWeaponInventory = false;
            openedInventory = !openedInventory;
        }
        ActivateCanvasGroup(inventoryPanelCanvasGroup, openedInventory);
        ActivateCanvasGroup(weaponsCanvasGroup, openedWeaponInventory);
        ActivateCanvasGroup(clothCanvasGroup, openedClothInventory);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ActivateCanvasGroup(weaponsCanvasGroup, false);
            ActivateCanvasGroup(clothCanvasGroup, false);
            openedWeaponInventory = false;
            DestroyInstances(weaponsCanvasGroup.transform);
            DestroyInstances(clothCanvasGroup.transform);
            ActivateCanvasGroup(inventoryPanelCanvasGroup, false);
        }
    }
    public void OpenWeaponsInventory()
    {
        openedInventory = false;
        openedWeaponInventory = true;
        openedClothInventory = false;
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
    public void OpenClothInventor()
    {
        openedInventory = false;
        openedWeaponInventory = false;
        openedClothInventory = true;
        ActivateCanvasGroup(clothCanvasGroup, true);
        int index = 0;
        foreach (ClotheObject cloth in clothes)
        {
            Button instance = Instantiate(clothButton, clothCanvasGroup.transform);
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
