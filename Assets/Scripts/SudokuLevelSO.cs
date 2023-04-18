using UnityEngine;

namespace HoMa.Sudoku
{
    [CreateAssetMenu(menuName = "HoMa/Sudoku/SudokuLevel")]
    public class SudokuLevelSO : ScriptableObject
    {
        public int[] SudokuSolution;
        public int[] ClueRevealedCells;
    }
}