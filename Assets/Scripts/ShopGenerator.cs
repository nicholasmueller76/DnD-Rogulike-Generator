using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class AdventurerShop
{
    public int highFreqItems;
    public int commonSale;
    public int uncommonSale;
    public int rareSale;
    public int mundane;
    public int common;
    public int uncommon;
    public int rare;
    //public int veryRare;
    //public int legendary;
};

[System.Serializable]
public class BlacksmithShop
{
    public int highFreqItems;
    public int commonSale;
    public int uncommonSale;
    public int rareSale;
    public int mundane;
    public int common;
    public int uncommon;
    public int rare;
    //public int veryRare;
    //public int legendary;
};
[System.Serializable]
public class AlchemistShop
{
    public int highFreqItems;
    public int commonSale;
    public int uncommonSale;
    public int rareSale;
    public int mundane;
    public int common;
    public int uncommon;
    public int rare;
    //public int veryRare;
    //public int legendary;
};
[System.Serializable]
public class WizardShop
{
    public int highFreqItems;
    public int commonSale;
    public int uncommonSale;
    public int rareSale;
    public int mundane;
    public int common;
    public int uncommon;
    public int rare;
    //public int veryRare;
    //public int legendary;
};


public class ShopGenerator : MonoBehaviour
{
    public TextAsset adv_HighFreqFile;
    public TextAsset adv_MundaneFile;
    public TextAsset adv_CommonFile;
    public TextAsset adv_UncommonFile;
    public TextAsset adv_RareFile;

    private string[] adv_HighFreqItems;
    private string[] adv_MundaneItems;
    private string[] adv_CommonItems;
    private string[] adv_UncommonItems;
    private string[] adv_RareItems;

    public TextAsset blk_HighFreqFile;
    public TextAsset blk_MundaneFile;
    public TextAsset blk_CommonFile;
    public TextAsset blk_UncommonFile;
    public TextAsset blk_RareFile;

    private string[] blk_HighFreqItems;
    private string[] blk_MundaneItems;
    private string[] blk_CommonItems;
    private string[] blk_UncommonItems;
    private string[] blk_RareItems;

    public TextAsset alc_HighFreqFile;
    public TextAsset alc_MundaneFile;
    public TextAsset alc_CommonFile;
    public TextAsset alc_UncommonFile;
    public TextAsset alc_RareFile;

    private string[] alc_HighFreqItems;
    private string[] alc_MundaneItems;
    private string[] alc_CommonItems;
    private string[] alc_UncommonItems;
    private string[] alc_RareItems;

    public TextAsset wiz_HighFreqFile;
    public TextAsset wiz_MundaneFile;
    public TextAsset wiz_CommonFile;
    public TextAsset wiz_UncommonFile;
    public TextAsset wiz_RareFile;

    private string[] wiz_HighFreqItems;
    private string[] wiz_MundaneItems;
    private string[] wiz_CommonItems;
    private string[] wiz_UncommonItems;
    private string[] wiz_RareItems;

    public AdventurerShop[] adventurerShops;

    public BlacksmithShop[] blacksmithShops;

    public AlchemistShop[] alchemistShops;

    public WizardShop[] wizardShops;

    public Text output;
    public Dropdown merchantSelector;
    public Dropdown floorSelector;

    // Start is called before the first frame update
    void Start()
    {
        adv_HighFreqItems = adv_HighFreqFile.ToString().Split('\n');
        adv_MundaneItems = adv_MundaneFile.ToString().Split('\n');
        adv_CommonItems = adv_CommonFile.ToString().Split('\n');
        adv_UncommonItems = adv_UncommonFile.ToString().Split('\n');
        adv_RareItems = adv_RareFile.ToString().Split('\n');

        blk_HighFreqItems = blk_HighFreqFile.ToString().Split('\n');
        blk_MundaneItems = blk_MundaneFile.ToString().Split('\n');
        blk_CommonItems = blk_CommonFile.ToString().Split('\n');
        blk_UncommonItems = blk_UncommonFile.ToString().Split('\n');
        blk_RareItems = blk_RareFile.ToString().Split('\n');

        alc_HighFreqItems = alc_HighFreqFile.ToString().Split('\n');
        alc_MundaneItems = alc_MundaneFile.ToString().Split('\n');
        alc_CommonItems = alc_CommonFile.ToString().Split('\n');
        alc_UncommonItems = alc_UncommonFile.ToString().Split('\n');
        alc_RareItems = alc_RareFile.ToString().Split('\n');

        wiz_HighFreqItems = wiz_HighFreqFile.ToString().Split('\n');
        wiz_MundaneItems = wiz_MundaneFile.ToString().Split('\n');
        wiz_CommonItems = wiz_CommonFile.ToString().Split('\n');
        wiz_UncommonItems = wiz_UncommonFile.ToString().Split('\n');
        wiz_RareItems = wiz_RareFile.ToString().Split('\n');
    }

    public void ShowHide()
    {
        this.gameObject.SetActive(!gameObject.activeInHierarchy);
    }

    public void Generate()
    {
        if (merchantSelector.value == 0) output.text = GenerateAdventurer(floorSelector.value);
        else if (merchantSelector.value == 1) output.text = GenerateBlacksmith(floorSelector.value);
        else if (merchantSelector.value == 2) output.text = GenerateAlchemist(floorSelector.value);
        else if (merchantSelector.value == 3) output.text = GenerateWizard(floorSelector.value);
    }

    string GenerateAdventurer(int floor)
    {
        string output = "";

        for (int i = 0; i < adventurerShops[floor].highFreqItems; i++)
        {
            output += Generate(adv_HighFreqItems) + " - SALE!\n";
        }

        for (int i = 0; i < adventurerShops[floor].commonSale; i++)
        {
            output += Generate(adv_CommonItems) + ", 25 gp - Common - SALE!\n";
        }

        for (int i = 0; i < adventurerShops[floor].uncommonSale; i++)
        {
            output += Generate(adv_UncommonItems) + ", 120 gp - Uncommon - SALE!\n";
        }

        for (int i = 0; i < adventurerShops[floor].rareSale; i++)
        {
            output += Generate(adv_RareItems) + ", 750 gp - Rare - SALE!\n";
        }

        for (int i = 0; i < adventurerShops[floor].mundane; i++)
        {
            output += Generate(adv_MundaneItems)  + " - Mundane\n";
        }

        for (int i = 0; i < adventurerShops[floor].common; i++)
        {
            output += Generate(adv_CommonItems) + ", 50 gp - Common\n";
        }

        for (int i = 0; i < adventurerShops[floor].uncommon; i++)
        {
            output += Generate(adv_UncommonItems) + ", 200 gp - Uncommon\n";
        }

        for (int i = 0; i < adventurerShops[floor].rare; i++)
        {
            output += Generate(adv_RareItems) + ", 1000 gp - Rare\n";
        }

        return output;
    }

    string GenerateBlacksmith(int floor)
    {
        string output = "";

        for (int i = 0; i < blacksmithShops[floor].highFreqItems; i++)
        {
            output += Generate(blk_HighFreqItems) + " - SALE!\n";
        }

        for (int i = 0; i < blacksmithShops[floor].commonSale; i++)
        {
            output += Generate(blk_CommonItems) + ", 30 gp - Common - SALE!\n";
        }

        for (int i = 0; i < blacksmithShops[floor].uncommonSale; i++)
        {
            output += Generate(blk_UncommonItems) + ", 150 gp - Uncommon - SALE!\n";
        }

        for (int i = 0; i < blacksmithShops[floor].rareSale; i++)
        {
            output += Generate(blk_RareItems) + ", 800 gp - Rare - SALE!\n";
        }

        for (int i = 0; i < blacksmithShops[floor].mundane; i++)
        {
            output += Generate(blk_MundaneItems) + " - Mundane\n";
        }

        for (int i = 0; i < blacksmithShops[floor].common; i++)
        {
            output += Generate(blk_CommonItems) + ", 50 gp - Common\n";
        }

        for (int i = 0; i < blacksmithShops[floor].uncommon; i++)
        {
            output += Generate(blk_UncommonItems) + ", 200 gp - Uncommon\n";
        }

        for (int i = 0; i < blacksmithShops[floor].rare; i++)
        {
            output += Generate(blk_RareItems) + ", 1000 gp - Rare\n";
        }

        return output;
    }

    string GenerateAlchemist(int floor)
    {
        string output = "";

        for (int i = 0; i < alchemistShops[floor].highFreqItems; i++)
        {
            output += Generate(alc_HighFreqItems) + " - SALE!\n";
        }

        for (int i = 0; i < alchemistShops[floor].commonSale; i++)
        {
            output += Generate(alc_CommonItems) + ", 25 gp - Common - SALE!\n";
        }

        for (int i = 0; i < alchemistShops[floor].uncommonSale; i++)
        {
            output += Generate(alc_UncommonItems) + ", 120 gp - Uncommon - SALE!\n";
        }

        for (int i = 0; i < alchemistShops[floor].rareSale; i++)
        {
            output += Generate(alc_RareItems) + ", 750 gp - Rare - SALE!\n";
        }

        for (int i = 0; i < alchemistShops[floor].mundane; i++)
        {
            output += Generate(alc_MundaneItems) + " - Mundane\n";
        }

        for (int i = 0; i < alchemistShops[floor].common; i++)
        {
            output += Generate(alc_CommonItems) + ", 50 gp - Common\n";
        }

        for (int i = 0; i < alchemistShops[floor].uncommon; i++)
        {
            output += Generate(alc_UncommonItems) + ", 200 gp - Uncommon\n";
        }

        for (int i = 0; i < alchemistShops[floor].rare; i++)
        {
            output += Generate(alc_RareItems) + ", 1000 gp - Rare\n";
        }

        return output;
    }

    string GenerateWizard(int floor)
    {
        string output = "";

        for (int i = 0; i < wizardShops[floor].highFreqItems; i++)
        {
            output += Generate(wiz_HighFreqItems) + " - SALE!\n";
        }

        for (int i = 0; i < wizardShops[floor].commonSale; i++)
        {
            output += Generate(wiz_CommonItems) + ", 30 gp - Common - SALE!\n";
        }

        for (int i = 0; i < wizardShops[floor].uncommonSale; i++)
        {
            output += Generate(wiz_UncommonItems) + ", 150 gp - Uncommon - SALE!\n";
        }

        for (int i = 0; i < wizardShops[floor].rareSale; i++)
        {
            output += Generate(wiz_RareItems) + ", 800 gp - Rare - SALE!\n";
        }

        for (int i = 0; i < wizardShops[floor].mundane; i++)
        {
            output += Generate(wiz_MundaneItems) + " - Mundane\n";
        }

        for (int i = 0; i < wizardShops[floor].common; i++)
        {
            output += Generate(wiz_CommonItems) + ", 50 gp - Common\n";
        }

        for (int i = 0; i < wizardShops[floor].uncommon; i++)
        {
            output += Generate(wiz_UncommonItems) + ", 200 gp - Uncommon\n";
        }

        for (int i = 0; i < wizardShops[floor].rare; i++)
        {
            output += Generate(wiz_RareItems) + ", 1000 gp - Rare\n";
        }

        return output;
    }

    public string Generate(string[] items)
    {
        return items[Random.Range(0, items.Length-1)];
    }

    public int FivePercentRandom(int price)
    {
        return price + ((Random.Range(-5, 5) / 100) * price);
    }
}
