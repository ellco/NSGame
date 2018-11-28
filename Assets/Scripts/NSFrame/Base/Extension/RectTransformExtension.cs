using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NSFrame
{
    public static class RectTransformExtension
    {
        public static void AnchorToCorners(this RectTransform transform)
        {
            if (transform == null)
                throw new ArgumentNullException("transform");

            if (transform.parent == null)
                return;
            transform.localScale = Vector3.one;
            transform.localPosition = Vector3.zero;

            Vector2 newAnchorsMin = new Vector2(0.0f, 0.0f);
            Vector2 newAnchorsMax = new Vector2(1.0f, 1.0f);

            transform.anchorMin = newAnchorsMin;
            transform.anchorMax = newAnchorsMax;
            transform.offsetMin = transform.offsetMax = new Vector2(0, 0);

        }
    }
}
