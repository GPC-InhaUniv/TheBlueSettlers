﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ProjectB.Inventory
{
    abstract public class Slot : MonoBehaviour, IPointerClickHandler
    {
        protected enum SlotType
        {
            InventorySlot, CombinationSlot, EquipSlot, WarehouseSlot
        }

        protected Image clickedImage;
        protected bool isClicked;
        public bool IsClicked { get { return isClicked; } }
        protected static Slot beforePressSlot;

        private void OnEnable()
        {
            clickedImage = this.gameObject.GetComponent<Image>();
        }

        abstract public void OnPointerClick(PointerEventData eventData);

        public void InitializeToIsClicked()
        {
            isClicked = false;
        }

        public void InitializeTobeforePressSlot()
        {
            beforePressSlot = null;
        }

        public void InitializeToClickedImage()
        {
            clickedImage.color = new Color(1, 1, 1, 0.392f);
        }
    }
}