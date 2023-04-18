using DG.Tweening;
using UnityEngine;

namespace HoMa.Sudoku.Framework
{
    /// <summary>
    /// A sinking button on click, with the added functionality of changing it's target graphic's color, through DOTween system.
    /// </summary>
    public class SinkToggleColorClickButton : SinkClickButton
    {
        private bool BaseColorApplied;

        [SerializeField] private Color m_BaseColor;
        [SerializeField] private Color m_ToggledColor;

        protected override void Awake()
        {
            base.Awake();

            m_BaseColor = TargetGraphic.color;
            BaseColorApplied = true;
        }

        internal override void DoSink()
        {
            base.DoSink();

            if (BaseColorApplied)
            {
                TargetGraphic.DOColor(m_ToggledColor, 0.05f);
                BaseColorApplied = false;
            }
            else
            {
                TargetGraphic.DOColor(m_BaseColor, 0.05f);
                BaseColorApplied = true;
            }
        }
    }
}