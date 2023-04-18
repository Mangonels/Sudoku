using HoMa.Sudoku.Framework;
using UnityEngine;
using UnityEngine.UI;

namespace HoMa.Sudoku
{
    /// <summary>
    /// Manages menu commands.
    /// </summary>
    public class MainMenuView : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private Button m_DoSudokuButton;

        protected void Awake()
        {
            m_DoSudokuButton.onClick.AddListener(() => SceneLoadingManager.Instance.LoadScene(1, 0.15f));
        }
    }
}