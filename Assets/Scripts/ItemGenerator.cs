using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemGenerator : MonoBehaviour
{
    public TextAsset commonItemsFile;
    public TextAsset uncommonItemsFile;
    public TextAsset rareItemsFile;
    public TextAsset veryRareItemsFile;
    public TextAsset legendaryItemsFile;

    public TextAsset cheapItemsFile;
    public TextAsset decentItemsFile;
    public TextAsset goodItemsFile;
    public TextAsset expensiveItemsFile;

    //[SerializeField]
    private string[] commonItems;
    private string[] uncommonItems;
    private string[] rareItems;
    private string[] veryRareItems;
    private string[] legendaryItems;

    private string[] cheapItems;
    private string[] decentItems;
    private string[] goodItems;
    private string[] expensiveItems;

    public Text output;
    public Dropdown raritySelector;
    public Dropdown costSelector;
    public Dropdown typeSelector;

    public void Start()
    {
        commonItems = commonItemsFile.ToString().Split('\n');
        uncommonItems = uncommonItemsFile.ToString().Split('\n');
        rareItems = rareItemsFile.ToString().Split('\n');
        veryRareItems = veryRareItemsFile.ToString().Split('\n');
        legendaryItems = legendaryItemsFile.ToString().Split('\n');

        cheapItems = cheapItemsFile.ToString().Split('\n');
        decentItems = decentItemsFile.ToString().Split('\n');
        goodItems = goodItemsFile.ToString().Split('\n');
        expensiveItems = expensiveItemsFile.ToString().Split('\n');
    }

    public void ChangeType()
    {
        if (typeSelector.value == 0)
        {
            raritySelector.gameObject.SetActive(true);
            costSelector.gameObject.SetActive(false);
        }
        else if (typeSelector.value == 1)
        {
            raritySelector.gameObject.SetActive(false);
            costSelector.gameObject.SetActive(true);
        }
    }

    public void ShowHide()
    {
        this.gameObject.SetActive(!gameObject.activeInHierarchy);
    }

    public void Generate()
    {
        if (typeSelector.value == 0) GenerateMagicItem();
        else if (typeSelector.value == 1) GenerateMundaneItem();
    }

    public void GenerateMagicItem()
    {
        if (raritySelector.value == 0) output.text = Generate(commonItems);
        else if (raritySelector.value == 1) output.text = Generate(uncommonItems);
        else if (raritySelector.value == 2) output.text = Generate(rareItems);
        else if (raritySelector.value == 3) output.text = Generate(veryRareItems);
        else if (raritySelector.value == 4) output.text = Generate(legendaryItems);
    }

    public void GenerateMundaneItem()
    {
        if (costSelector.value == 0) output.text = Generate(cheapItems);
        else if (costSelector.value == 1) output.text = Generate(decentItems);
        else if (costSelector.value == 2) output.text = Generate(goodItems);
        else if (costSelector.value == 3) output.text = Generate(expensiveItems);
    }

    public string Generate(string[] items)
    {
        return items[Random.Range(0, items.Length-1)];
    }
}
