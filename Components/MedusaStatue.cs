using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace TownOfTrailay.Components
{
    public class MedusaStatue : MonoBehaviour
    {
        public static Color Gray = Color.gray;
        public static Color Gray2 = new Color(0.180f, 0.180f, 0.180f);
        public static Color Gray3 = new Color(0.7f, 0.7f, 0.7f);
        public float AnimationTimer = 2;
        public SpriteRenderer BodyRender;
        public int colorId;
        public System.Collections.IEnumerator Start()
        {
            for (float t = 0f; t < AnimationTimer; t += Time.deltaTime)
            {
                BodyRender.material.SetColor("_BackColor", Color.Lerp(Palette.ShadowColors[colorId], Gray2, t / AnimationTimer));
                BodyRender.material.SetColor("_BodyColor", Color.Lerp(Palette.PlayerColors[colorId], Color.gray, t / AnimationTimer));
                BodyRender.material.SetColor("_VisorColor", Color.Lerp(Palette.VisorColor, Gray3, t / AnimationTimer));
                yield return null;
            }
        }
    }
}
