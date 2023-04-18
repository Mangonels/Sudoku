using UnityEngine;
using UnityEngine.UI;

namespace HoMa.Sudoku
{
    /// <summary>
    /// Notifies of value selection.
    /// </summary>
    public class SelectableValue : MonoBehaviour
    {
        [SerializeField] private int m_Value;
        [SerializeField] private Button m_ButtonComponent;

        private void Awake()
        {
            m_ButtonComponent.onClick.AddListener(NotifySudokuValueSelection);
        }

        private void NotifySudokuValueSelection()
        {
            SudokuGameManager.Instance.SetSelectedSudokuCellValue(m_Value);
        }
    }
}