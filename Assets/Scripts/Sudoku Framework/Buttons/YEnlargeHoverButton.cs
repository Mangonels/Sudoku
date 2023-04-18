using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HoMa.Sudoku.Framework
{
    /// <summary>
    /// An enlarging button on hover, simply scales up part of the button's graphic when hovering, and de-scales if not hovering, 
    /// it also changes to a diferent color on click, through DOTween system.
    /// </summary>
    public class YEnlargeHoverButton : Button
    {
        private float m_OriginalScale;

        [SerializeField] private Image m_MainGraphic;
        [SerializeField] private float m_HoverYScale = 1.5f;
        [SerializeField] private Color m_HoverColor = Color.white;
        [SerializeField] private Color m_UnohveredColor = Color.gray;

        protected override void Awake()
        {
            base.Awake();

            m_OriginalScale = m_MainGraphic.rectTransform.localScale.y;
        }

        public override void OnPointerEnter(PointerEventData pointerEventData)
        {
            HoverEffects();
        }

        public override void OnPointerExit(PointerEventData pointerEventData)
        {
            SetOriginalState();
        }

        private void HoverEffects()
        {
            m_MainGraphic.rectTransform.DOScaleY(m_HoverYScale, 0.1f);
            m_MainGraphic.DOColor(m_HoverColor, 0.1f);
        }
        private void SetOriginalState()
        {
            m_MainGraphic.rectTransform.DOScaleY(m_OriginalScale, 0.1f);
            m_MainGraphic.DOColor(m_UnohveredColor, 0.1f);
        }
    }
}