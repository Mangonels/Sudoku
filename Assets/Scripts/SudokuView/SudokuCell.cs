using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HoMa.Sudoku
{
    /// <summary>
    /// Sudoku cell which manages stuff such as it's displayed value and highlighting.
    /// </summary>
    public class SudokuCell : MonoBehaviour
    {
        private bool m_Mutable = true;
        private Color m_BackgroundColor;

        [Header("Coloring")]
        [SerializeField] private Color m_RelevantColor;
        [SerializeField] private Color m_SelectedNonMutableColor;
        [SerializeField] private Color m_SelectedMutableColor;

        [Header("Data")]
        [SerializeField] private int m_CellIndex;
        [SerializeField] private int m_Column;
        [SerializeField] private int m_Row;
        [SerializeField] private int m_Group;

        [Header("References")]
        [SerializeField] private Button m_ButtonComponent;
        [SerializeField] private Image m_ImageComponent;
        [SerializeField] private TMP_Text m_ValueDisplay;

        internal int CellIndex { get { return m_CellIndex; } }
        internal int Column { get { return m_Column; } }
        internal int Row { get { return m_Row; } }
        internal int Group { get { return m_Group; } }

        /// <summary>
        /// Specifies if this cell's value is locked "true" or not "false".
        /// </summary>
        internal bool MutableValue
        {
            get { return m_Mutable; }
            set { m_Mutable = value; }
        }

        private void Awake()
        {
            m_BackgroundColor = m_ImageComponent.color;

            m_ButtonComponent.onClick.AddListener(NotifySudokuCellClick);
        }

        internal void SetValue(int value) 
        {
            m_ValueDisplay.text = value.ToString();
        }
        internal void ClearValue()
        {
            m_ValueDisplay.text = "";
        }

        internal void SetColor(Color bgColor, Color textColor, Color selectedColor, Color selectedNonMutableCellColor, Color relevantColor) 
        {
            m_BackgroundColor = bgColor;
            m_ImageComponent.color = bgColor;
            m_ValueDisplay.color = textColor;
            m_SelectedMutableColor = selectedColor;
            m_SelectedNonMutableColor = selectedNonMutableCellColor;
            m_RelevantColor = relevantColor;
        }

        internal void SelectionHighlight()
        {
            if(MutableValue) m_ImageComponent.color = m_SelectedMutableColor;
            else m_ImageComponent.color = m_SelectedNonMutableColor;
        }

        internal void RelevanceHighglight()
        {
            m_ImageComponent.color = m_RelevantColor;
        }

        internal void RemoveHighlight()
        {
            m_ImageComponent.color = m_BackgroundColor;
        }

        private void NotifySudokuCellClick() 
        {
            SudokuGameManager.Instance.SetSelectedSudokuCell(m_CellIndex);
        }
    }
}