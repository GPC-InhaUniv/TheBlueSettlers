﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectB.Item;
using ProjectB.Inventory;

public class WarehouseUIPresenter : MonoBehaviour
{
    [SerializeField] GameObject warehousePanel;

    public void SwapToWarehouseItem(Slot currentSlot, Slot swapSlot)
    {
        int SlotIndex;

        SlotIndex = currentSlot.transform.GetSiblingIndex();
        currentSlot.transform.SetSiblingIndex(swapSlot.transform.GetSiblingIndex());
        swapSlot.transform.SetSiblingIndex(SlotIndex);
    }

    public void SwapToFromInventorySlotToWarehouseSlot(Item currentItem, Item swapItem)
    {
        int SwapItemCode = currentItem.Code;
        int SwapItemAmount = currentItem.ItemAmount;

        currentItem.SetItem(swapItem.Code);
        currentItem.SetItemAmount(swapItem.ItemAmount);
        swapItem.SetItem(SwapItemCode);
        swapItem.SetItemAmount(SwapItemAmount);

        currentItem.ItemAmountText.text = currentItem.ItemAmount.ToString();
        currentItem.ItemImage.sprite = AssetBundleManager.Instance.LoadSprite(BundleType.Common, currentItem.Image);
        swapItem.ItemAmountText.text = swapItem.ItemAmount.ToString();
        swapItem.ItemImage.sprite = AssetBundleManager.Instance.LoadSprite(BundleType.Common, swapItem.Image);
    }

    public void OnClickWarehouseExitButton()
    {
        warehousePanel.SetActive(false);
    }
}
