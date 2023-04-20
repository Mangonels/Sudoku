using UnityEngine;
using UnityEngine.UI;
#if UNITY_ANDROID && !UNITY_EDITOR
using UnityEngine.EventSystems;
#endif

namespace HoMa.Sudoku
{
    /// <summary>
    /// Notifies of value selection.
    /// </summary>
    public class SelectableValue : MonoBehaviour
#if UNITY_ANDROID && !UNITY_EDITOR
        , IPointerEnterHandler
#endif
    {
        [SerializeField] private int m_Value;
        [SerializeField] private Button m_ButtonComponent;

#if UNITY_EDITOR
        private void Awake()
        {
            m_ButtonComponent.onClick.AddListener(() => SudokuGameManager.Instance.SetSelectedSudokuCellValue(m_Value));
        }
#endif

#if UNITY_ANDROID && !UNITY_EDITOR
        public void OnPointerEnter(PointerEventData eventData)
        {
            SudokuGameManager.Instance.SetSelectedSudokuCellValue(m_Value);
        }
#endif
    }
}