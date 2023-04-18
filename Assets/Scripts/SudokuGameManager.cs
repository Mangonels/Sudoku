using HoMa.Sudoku.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace HoMa.Sudoku
{
    /// <summary>
    /// The SudokuGameManager class currently takes care of most of the Sudoku game's logic, and level progression.
    /// With further development, certain responsabilities could be moved to more specific subclasses.
    /// </summary>
    public class SudokuGameManager : Singleton<SudokuGameManager>
    {
        private int m_Level = 0;
        private int m_SelectedCell;

        private bool m_MoveValueHolderToSelection = false;

        [SerializeField] private SudokuView m_SudokuViewRef;
        [SerializeField] private List<SudokuLevelSO> m_Levels;

        internal int Level { get { return m_Level; } }
        internal int SelectedCell { get { return m_SelectedCell; } }

        internal bool MoveValueHolderToSelection
        {
            get { return m_MoveValueHolderToSelection; }
            set { m_MoveValueHolderToSelection = value; }
        }

        private void Awake()
        {
            base.Awake(false);
        }

        private void Start()
        {
            SetSelectedSudokuCell(0);

            m_SudokuViewRef.SetClues(m_Levels[m_Level]);
        }

        internal void SetSelectedSudokuCell(int index) 
        {
            m_SelectedCell = index;

            m_SudokuViewRef.UpdateHighlights();
        }

        internal void SetSelectedSudokuCellValue(int value) 
        {
            m_SudokuViewRef.SetCellValue(m_SelectedCell, value);

            if (m_SudokuViewRef.AllCellValuesSet && ValidateCurrentSudokuSolution()) SetupNextLevelSudoku();
        }

        private bool ValidateCurrentSudokuSolution() 
        {
            int[] solution = m_Levels[m_Level].SudokuSolution;
            int[] currentState = m_SudokuViewRef.SudokuArrayCellValues;

            for (int i = 0; i < solution.Length; i++)
            {
                if (solution[i] != currentState[i])
                {
                    return false;
                }
            }

            return true;
        }

        private void SetupNextLevelSudoku()
        {
            m_Level = (m_Level + 1) % m_Levels.Count; //Loop levels

            m_SudokuViewRef.ResetAllCells();
            m_SudokuViewRef.SetClues(m_Levels[m_Level]);
        }
    }
}