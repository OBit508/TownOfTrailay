using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace TownOfTrailay.Components
{
    public class MedusaStatue : MonoBehaviour
    {
        public float AnimationTimer = 2;
        public List<SpriteRenderer> renderers = new List<SpriteRenderer>();
        public System.Collections.IEnumerator Start()
        {
            for (float t = 0f; t < AnimationTimer; t += Time.deltaTime)
            {
                foreach (SpriteRenderer rend in renderers)
                {
                    rend.color = Color.Lerp(Color.white, Color.black, t / AnimationTimer);
                }
                yield return null;
            }
        }
    }
}
