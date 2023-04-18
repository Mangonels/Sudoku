using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace HoMa.Sudoku.Framework
{
    /// <summary>
    /// A sinking button on click, simply moves the part of the button's graphic downwards on click, through DOTween system.
    /// </summary>
    public class SinkClickButton : Button
    {
        public Image TargetGraphic;
        [SerializeField] private float m_SinkAmount = -20f;

        protected override void Awake()
        {
            base.Awake();

            onClick.AddListener(DoSink);
        }

        internal virtual void DoSink() 
        {
            TargetGraphic.rectTransform.DOLocalMoveY(m_SinkAmount, 0.05f).OnComplete(
                ()=> { TargetGraphic.rectTransform.DOLocalMoveY(0, 0.05f); }
            );
        }
    }
}