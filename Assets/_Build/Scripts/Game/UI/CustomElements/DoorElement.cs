using UnityEngine;
using UnityEngine.UIElements;

namespace LostKaiju.Game.UI.CustomElements
{
    // [UxmlElement]
    public partial class DoorElement : VisualElement
    {
        public DoorElement()
        {
            generateVisualContent += OnGenerateVisualContent;
        }

        private void OnGenerateVisualContent(MeshGenerationContext ctx)
        {
            var painter = ctx.painter2D;

            var rect = contentRect;

            float doorWidth = rect.width * 0.7f;
            float doorHeightLeft = rect.height * 0.1f;

            painter.BeginPath();

            painter.MoveTo(new Vector2(0, 0));
            painter.LineTo(new Vector2(doorWidth, doorHeightLeft));
            painter.LineTo(new Vector2(doorWidth, rect.height - doorHeightLeft * 0.9f));
            painter.LineTo(new Vector2(0, rect.height));
            painter.ClosePath();

            painter.fillColor = Color.black;
            painter.Fill();

            painter.strokeColor = Color.white;
            painter.lineWidth = 2;
            painter.Stroke();
        }
    }
}