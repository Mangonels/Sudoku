using UnityEngine;

namespace HoMa.Sudoku
{
    [CreateAssetMenu(menuName = "HoMa/Sudoku/ColorScheme")]
    public class ColorSchemeSO : ScriptableObject
    {
        [SerializeField] public Color Background;
        [SerializeField] public Color SudokuCell;
        [SerializeField] public Color SudokuBackground;
        [SerializeField] public Color SudokuCellText;
        [SerializeField] public Color SelectedCell;
        [SerializeField] public Color SelectedNonMutableCell;
        [SerializeField] public Color RelevantCell;
    }
}