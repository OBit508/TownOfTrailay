using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace TownOfTrailay.Helpers.Role
{
    public class CustomButtonRolloverHandler : MonoBehaviour
    {
        public void Start()
        {
            PassiveButton component = GetComponent<PassiveButton>();
            component.OnMouseOver.AddListener(new UnityAction(DoMouseOver));
            component.OnMouseOut.AddListener(new UnityAction(DoMouseOut));
            Target.material.SetFloat("_Outline", 1);
            SetOutline(OutColor);
        }
        public void DoMouseOver()
        {
            if (Target != null)
            {
                SetOutline(OverColor);
            }
            if (HoverSound)
            {
                SoundManager.Instance.PlaySound(HoverSound, false, 1f, false);
            }
        }
        public void DoMouseOut()
        {
            if (Target != null)
            {
                SetOutline(OutColor);
            }
        }
        public void SetOutline(Color color)
        {
            Target.material.SetColor("_OutlineColor", color);
        }
        public SpriteRenderer Target;
        public Color OverColor = Color.green;
        public Color OutColor = Color.white;
        public AudioClip HoverSound;
    }

}
