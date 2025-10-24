using UnityEngine;

public class ShopItem : MonoBehaviour
{
    [SerializeField] private string itemName;
    [SerializeField] [TextArea(3, 10)] private string itemDesc;
    [SerializeField] private int itemPrice;

    [SerializeField] private TMPro.TextMeshProUGUI m_ItemNameText;
    [SerializeField] private TMPro.TextMeshProUGUI m_ItemDescText;
    [SerializeField] private TMPro.TextMeshProUGUI m_ItemPriceText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //void Start() { }

    private void OnEnable()
    {
        SetShopItem();
    }

    // Update is called once per frame
    //void Update() { }

    [ContextMenu("SetShopItem")]
    private void SetShopItem()
    {
        m_ItemNameText.SetText(itemName);
        m_ItemDescText.SetText(itemDesc);
        m_ItemPriceText.SetText("{0} G", itemPrice);
    }
}
