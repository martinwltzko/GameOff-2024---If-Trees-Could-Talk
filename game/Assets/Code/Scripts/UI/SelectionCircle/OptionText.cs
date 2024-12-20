﻿using System;
using TMPro;
using UnityEngine;

namespace Code.Scripts.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class OptionText : MonoBehaviour
    {
        [SerializeField] private RectTransform rt;
        [SerializeField] private TextMeshProUGUI label;
        
        public Vector2 Position => rt.anchoredPosition;

        public void Set(string text, Vector2 localPosition, Color color)
        {
            SetText(text);
            SetPosition(localPosition);
            SetColor(color);
            
            var width = label.preferredWidth;
            var height = label.preferredHeight;
            rt.sizeDelta = new Vector2(width, height);
        }

        public void SetText(string text) => label.SetText(text);

        public void SetColor(Color color) => label.color = color;

        public void SetPosition(Vector2 localPosition) => rt.anchoredPosition = localPosition;

        public void Hide(bool hide) => gameObject.SetActive(!hide);
    }
}