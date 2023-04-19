using HoMa.Sudoku.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace HoMa.Sudoku
{
    /// <summary>
    /// Manages sudoku grid updates and visuals.
    /// </summary>
    public class SudokuView : MonoBehaviour
    {
        private bool m_FullHighlightApplied = true;
        private bool m_LightColorScheme = false;

        private int[] m_SudokuGridCellValues; //Current values for each cell as integers

        [Header("ColorSchemes")]
        [SerializeField] private ColorSchemeSO m_Light_ColorScheme;
        [SerializeField] private ColorSchemeSO m_Dark_ColorScheme;

        [Header("References")]
        [SerializeField] private SudokuCell[] m_SudokuGridCells;
        [Space]
        [SerializeField] private Animator m_SudokuPanelAnimator;
        [Space]
        [SerializeField] private Button m_BackToMenuButton;
        [SerializeField] private Button m_ColorSchemeButton;
        [SerializeField] private Button m_ClearCellButton;
        [SerializeField] private Button m_ClearSudokuButton;
        [SerializeField] private Button m_ExtraHighlightToggleButton;
        [Space]
        [SerializeField] private Image m_Background;
        [SerializeField] private Image m_SudokuBackground;

        public int[] SudokuArrayCellValues { get { return m_SudokuGridCellValues; } }

        internal bool AllCellValuesSet //Returns true if all values in the internal sudoku array are not 0 (sudoku is fully filled in), else returns 0.
        {
            get
            {
                foreach (int value in m_SudokuGridCellValues)
                {
                    if (value == 0)
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        private void Awake()
        {
            m_SudokuGridCellValues = new int[81];

            m_BackToMenuButton.onClick.AddListener(() => SceneLoadingManager.Instance.LoadScene(0, 0.15f));
            m_ColorSchemeButton.onClick.AddListener(() => ToggleColorScheme());
            m_ClearCellButton.onClick.AddListener(() => ClearCell(SudokuGameManager.Instance.SelectedCell));
            m_ClearSudokuButton.onClick.AddListener(ClearAllCells);
            m_ExtraHighlightToggleButton.onClick.AddListener(ToggleHighlights);
        }

    #region Sudoku visual controls
        /// <summary>
        /// Sets the internal and visual value for a specific Mutable Sudoku cell.
        /// </summary>
        /// <param name="cellIndex">The index for the cell who's value will be set, starts at 0 for the first cell, top left of the Sudoku grid.</param>
        /// <param name="value">The new value for the cell.</param>
        internal void SetCellValue(int cellIndex, int value)
        {
            if (m_SudokuGridCells[cellIndex].MutableValue)
            {
                m_SudokuGridCellValues[cellIndex] = value;
                m_SudokuGridCells[cellIndex].SetValue(value);
            }
        }

        /// <summary>
        /// Clears the internal (reset to 0) and visual (empty string: "") value for a specific Mutable Sudoku cell.
        /// </summary>
        /// <param name="cellIndex">The index for the cell who's value will be set to an empty string.</param>
        internal void ClearCell(int cellIndex)
        {
            if (m_SudokuGridCells[cellIndex].MutableValue) 
            {
                m_SudokuGridCellValues[cellIndex] = 0;
                m_SudokuGridCells[cellIndex].ClearValue();
            }
        }
        /// <summary>
        /// Clears the internal (reset to 0) and visual (empty string: "") value for al Mutable Sudoku cells.
        /// </summary>
        internal void ClearAllCells()
        {
            for (int i = 0; i < m_SudokuGridCellValues.Length; i++)
            {
                if (m_SudokuGridCells[i].MutableValue)
                {
                    m_SudokuGridCellValues[i] = 0;
                    m_SudokuGridCells[i].ClearValue();
                }
            }
        }

        /// <summary>
        /// Resets and unlocks all cells in the Sudoku grid.
        /// </summary>
        internal void ResetAllCells()
        {
            for(int i = 0; i < m_SudokuGridCellValues.Length; i++) 
            {
                m_SudokuGridCellValues[i] = 0;
                m_SudokuGridCells[i].ClearValue();
                m_SudokuGridCells[i].MutableValue = true;
            }
        }

        /// <summary>
        /// Sets the sudoku's clues for this level.
        /// </summary>
        internal void SetClues(SudokuLevelSO level)
        {
            for (int i = 0; i < level.ClueRevealedCells.Length; i++)
            {
                int clueIndex = level.ClueRevealedCells[i];
                int clueValue = level.SudokuSolution[clueIndex];

                SetCellValue(clueIndex, clueValue);
                m_SudokuGridCells[clueIndex].MutableValue = false;
            }
        }
    #endregion

    #region Cell highlighting system
        /// <summary>
        /// Toggles highlight on or off.
        /// </summary>
        internal void ToggleHighlights() 
        {
            m_FullHighlightApplied = !m_FullHighlightApplied;

            UpdateHighlights();
        }

        /// <summary>
        /// Updates sudoku grid highlight status by resetting cell colors first and then applying right colors.
        /// </summary>
        internal void UpdateHighlights() 
        {
            ClearAllHighlights();
            ApplyHighlights();
        }

        internal void ClearAllHighlights()
        {
            for (int i = 0; i < m_SudokuGridCellValues.Length; i++)
            {
                m_SudokuGridCells[i].RemoveHighlight();
            }
        }

        internal void ApplyHighlights()
        {
            int selectedCellIndex = SudokuGameManager.Instance.SelectedCell;

            if (m_FullHighlightApplied) FullHighlight(selectedCellIndex);
            else m_SudokuGridCells[selectedCellIndex].SelectionHighlight();
        }

        private void FullHighlight(int selectedCellIndex) 
        {
            SudokuCell selectedCell = m_SudokuGridCells[selectedCellIndex];

            for (int i = 0; i < m_SudokuGridCells.Length; i++)
            {
                if(m_SudokuGridCells[i].Group == selectedCell.Group || 
                    m_SudokuGridCells[i].Column == selectedCell.Column || 
                    m_SudokuGridCells[i].Row == selectedCell.Row) 
                {
                    m_SudokuGridCells[i].RelevanceHighglight();
                }
            }

            selectedCell.SelectionHighlight();
        }
        #endregion

    #region Color schemes
        /// <summary>
        /// Toggle Sudoku scene light/dark themes.
        /// </summary>
        private void ToggleColorScheme()
        {
            if (m_LightColorScheme) 
            {
                m_Background.color = m_Dark_ColorScheme.Background;
                m_SudokuBackground.color = m_Dark_ColorScheme.SudokuBackground;
                ChangeGridCellColors
                    (
                        m_Dark_ColorScheme.SudokuCell,
                        m_Dark_ColorScheme.SudokuCellText,
                        m_Dark_ColorScheme.SelectedCell,
                        m_Dark_ColorScheme.SelectedNonMutableCell,
                        m_Dark_ColorScheme.RelevantCell
                    );

                m_LightColorScheme = false;
            }
            else 
            {
                m_Background.color = m_Light_ColorScheme.Background;
                m_SudokuBackground.color = m_Light_ColorScheme.SudokuBackground;
                ChangeGridCellColors
                    (
                        m_Light_ColorScheme.SudokuCell,
                        m_Light_ColorScheme.SudokuCellText,
                        m_Light_ColorScheme.SelectedCell,
                        m_Light_ColorScheme.SelectedNonMutableCell,
                        m_Light_ColorScheme.RelevantCell
                    );

                m_LightColorScheme = true;
            }

            UpdateHighlights();
        }

        /// <summary>
        /// Changes all of the grid cell's colors by iterating their references
        /// Alternative having used an event based system would have also done the job in this case, but this solution is straight forward and works.
        /// </summary>
        /// <param name="color"></param>
        private void ChangeGridCellColors(Color bgColor, Color textColor, Color selectedColor, Color selectedNonMutableCellColor, Color relevantColor)
        {
            for(int i = 0; i < m_SudokuGridCells.Length; i++) 
            {
                m_SudokuGridCells[i].SetColor(bgColor, textColor, selectedColor, selectedNonMutableCellColor, relevantColor);
            }
        }
        #endregion

    #region Animations
        /// <summary>
        /// Plays cell wave animation.
        /// </summary>
        internal void CellsAnimation()
        {
            m_SudokuPanelAnimator.SetTrigger("CellWave");
        }
    #endregion
    }
}