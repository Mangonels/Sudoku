using HoMa.Sudoku.Framework;
using System.Collections;
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

        private bool m_SudokuLocked = false;

        private const int CELEB_EFFECTS_AMOUNT = 3;
        private const float CELEB_EFFECTS_DELAYS_SECONDS = 0.75f;
        private const float CELEB_FINISH_DELAY_SECONDS = 2f;
        private const float GAME_SET_UNLOCK_SUDOKU_SECONDS = 1f;

        private const float CELL_ANIM_WAIT_SECONDS = 2f;

        private const float CELEB_EFFECTS_MAX_X = 1.5f;
        private const float CELEB_EFFECTS_MIN_X = -1.5f;
        private const float CELEB_EFFECTS_MAX_Y = 0f;
        private const float CELEB_EFFECTS_MIN_Y = 4f;
        private const float CELEB_EFFECTS_Z = 88f;

        [Header("References")]
        [SerializeField] private SudokuView m_SudokuViewRef;
        [SerializeField] private List<SudokuLevelSO> m_Levels;

        [Header("Prefab references")]
        [SerializeField] private GameObject m_ConfettiEffect;

        internal int SelectedCell { get { return m_SelectedCell; } }

        internal bool SudokuLocked
        {
            get { return m_SudokuLocked; }
            set { m_SudokuLocked = value; }
        }

        private void Awake()
        {
            base.Awake(false);
        }

        private void Start()
        {
            m_SudokuViewRef.SetClues(m_Levels[m_Level]);

            SetSelectedSudokuCell(0);
        }

        internal void SetSelectedSudokuCell(int index) 
        {
            if (m_SudokuLocked) return;

            m_SelectedCell = index;

            m_SudokuViewRef.UpdateHighlights();
        }

        internal void SetSelectedSudokuCellValue(int value)
        {
            if (m_SudokuLocked) return;

            m_SudokuViewRef.SetCellValue(m_SelectedCell, value);

            if (m_SudokuViewRef.AllCellValuesSet && ValidateCurrentSudokuSolution()) StartCoroutine(SudokuLevelFinishedTransition());
        }

        private IEnumerator SudokuLevelFinishedTransition() 
        {
            m_SudokuLocked = true;

            yield return StartCoroutine(LevelFinishedCelebrationEffects());

            StartCoroutine(SetupNextLevelSudoku());
        }

        /// <summary>
        /// Current Sudoku state validator for finishing sudoku successfully.
        /// </summary>
        /// <returns>true: If the sudoku was completed with the correct numbers, else false.</returns>
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
        
        private IEnumerator SetupNextLevelSudoku()
        {
            m_SudokuViewRef.CellsAnimation(); //Play "reset cells" animation

            yield return new WaitForSeconds(CELL_ANIM_WAIT_SECONDS);

            m_Level = (m_Level + 1) % m_Levels.Count; //Loop levels

            m_SudokuViewRef.UpdateLevel(m_Level + 1);

            m_SudokuViewRef.ResetAllCells();
            m_SudokuViewRef.SetClues(m_Levels[m_Level]);

            yield return new WaitForSeconds(GAME_SET_UNLOCK_SUDOKU_SECONDS);

            m_SudokuLocked = false;

            SetSelectedSudokuCell(0);
        }

        private IEnumerator LevelFinishedCelebrationEffects()
        {
            for(int i = 0; i < CELEB_EFFECTS_AMOUNT; i++)
            {
                yield return new WaitForSeconds(CELEB_EFFECTS_DELAYS_SECONDS);

                Vector3 spawnPosition = new Vector3(Random.Range(CELEB_EFFECTS_MIN_X, CELEB_EFFECTS_MAX_X), Random.Range(CELEB_EFFECTS_MIN_Y, CELEB_EFFECTS_MAX_Y), CELEB_EFFECTS_Z);
                GameObject.Instantiate(m_ConfettiEffect, spawnPosition, Quaternion.identity);
            }

            yield return new WaitForSeconds(CELEB_FINISH_DELAY_SECONDS);
        }
    }
}