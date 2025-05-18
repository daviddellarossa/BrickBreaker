using System;
using System.Collections.Generic;
using System.Linq;
using DeeDeeR.BrickBreaker.Bricks;
using DeeDeeR.BrickBreaker.Levels;
using DeeDeeR.BrickBreaker.PowerUps;
using Editor.GameDataInitializers;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public delegate void ExecToolbarActionOnCell(int row, int col);

public class LevelEditor : EditorWindow
{
    [FormerlySerializedAs("_defaultNumberOfRows")] [SerializeField] 
    private int defaultNumberOfRows = 14;
    [FormerlySerializedAs("_defaultNumberOfColumns")] [SerializeField] 
    private int defaultNumberOfColumns = 14;
    
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    private VisualElement veLeftPanel;
    private VisualElement veCentrePanel;
    
    private BrickTypeToolbar _brickTypeToolbar;
    private PowerUpToolbar _powerUpToolbar;

    private ListView lvLevels;
    private LevelLayout levelLayout;
    
    private List<Level> _levels = new List<Level>();

    private ExecToolbarActionOnCell _execToolbarActionOnCell;
    
    private Level _selectedLevel = null;
    private (int row, int column)? _selectedCell = null;

    [MenuItem("Window/UI Toolkit/LevelEditor")]
    public static void ShowExample()
    {
        LevelEditor wnd = GetWindow<LevelEditor>();
        wnd.titleContent = new GUIContent("LevelEditor");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;
        
        // Instantiate UXML
        VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();
        root.Add(labelFromUXML);
        
        veLeftPanel = root.Q<VisualElement>("veLeftPanel");
        veCentrePanel = root.Q<VisualElement>("veCentrePanel");
        
        // veLevelLayout = veCentrePanel.Q<VisualElement>("veLevelLayout");
        
        lvLevels = veLeftPanel.Q<ListView>("lvLevels");
        levelLayout = veCentrePanel.Q<LevelLayout>();
        levelLayout.CellSelected += LevelLayoutOnCellSelected;
        
        _brickTypeToolbar = veCentrePanel.Q<BrickTypeToolbar>();
        _powerUpToolbar = veCentrePanel.Q<PowerUpToolbar>();

        BindLevelToolbar();
        BindBrickTypeToolbar();
        BindPowerUpToolbar();
        BindLevels();
    }

    private void LevelLayoutOnCellSelected(object sender, CellSelectedEventArgs e)
    {
        _execToolbarActionOnCell(e.Row, e.Column);;
    }


    private void BindBrickTypeToolbar()
    {
        if (_brickTypeToolbar == null)
        {
            Debug.LogError("BrickTypeToolbar not found");
            return;
        }
        _brickTypeToolbar.BrickTypeSelectionChanged += BrickTypeToolbarOnBrickTypeSelectionChanged;
    }

    private void BrickTypeToolbarOnBrickTypeSelectionChanged(object sender, BrickType e)
    {
        _powerUpToolbar.CancelCurrentSelection();
        _execToolbarActionOnCell = AddBrickTypeToCell;
    }

    private void BindPowerUpToolbar()
    {

        if (_powerUpToolbar == null)
        {
            Debug.LogError("PowerUpToolbar not found");
            return;
        }
        _powerUpToolbar.PowerUpSelectionChanged += PowerUpToolbarOnPowerUpSelectionChanged;

    }

    private void PowerUpToolbarOnPowerUpSelectionChanged(object sender, PowerUp e)
    {
        _brickTypeToolbar.CancelCurrentSelection();

        _execToolbarActionOnCell = AddPowerUpToCell;
    }

    private void BindLevelToolbar()
    {
        var levelToolbar = veLeftPanel.Q<LevelToolbar>("levelToolbar");
        levelToolbar.CreateLevelEvent += LevelToolbarOnCreateLevelEvent;
        levelToolbar.DeleteLevelEvent += LevelToolbarOnDeleteLevelEvent;
        levelToolbar.SortListEvent += LevelToolbarOnSortListEvent;
    }

    private void LevelToolbarOnSortListEvent(object sender, EventArgs e)
    {
        _levels.Sort((x, y) => x.SortingOrder.CompareTo(y.SortingOrder));
    }

    private void LevelToolbarOnDeleteLevelEvent(object sender, EventArgs e)
    {
        var selectedIndex = lvLevels.selectedIndex;
        if (selectedIndex < 0)
        {
            return;
        }
        
        var level = _levels[selectedIndex];
        if (level == null)
        {
            return;
        }
        
        // Show confirmation dialog
        bool confirmDelete = EditorUtility.DisplayDialog(
            "Delete Level",
            $"Are you sure you want to delete '{level.LevelName}'? This action cannot be undone.",
            "Delete",
            "Cancel");
        
        if (!confirmDelete)
        {
            return; // User canceled the deletion
        }

        
        _levels.Remove(level);
        lvLevels.selectedIndex = -1;
        lvLevels.itemsSource = _levels;
        lvLevels.selectedIndex = 0;
        
        AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(level));
        AssetDatabase.Refresh();
    }

    private void LevelToolbarOnCreateLevelEvent(object sender, EventArgs e)
    {
        var sortingOrder = _levels.Count + 1;
        var levelName = $"Level {sortingOrder}";
        var newLevel = ScriptableObjectHelper.CreateScriptableObject<Level>(levelName, PathHelper.LevelsPath);
        
        newLevel.LevelName = levelName;
        newLevel.SortingOrder = sortingOrder;
        newLevel.NumColumns = defaultNumberOfColumns;
        newLevel.NumRows = defaultNumberOfRows;
        
        newLevel.Cells = new List<Cell>(newLevel.NumColumns * newLevel.NumRows);
        for (int i = 0; i < newLevel.NumColumns * newLevel.NumRows; i++)
        {
            newLevel.Cells.Add(new Cell());
        }
        
        EditorUtility.SetDirty(newLevel);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
        _levels.Add(newLevel);
        lvLevels.selectedIndex = _levels.Count - 1;
    }

    private void BindLevels()
    {
        _levels.Clear();
        
        var levels = ScriptableObjectHelper.GetAllScriptableObjects<Level>(PathHelper.LevelsPath);
        _levels.AddRange(levels);
        _levels.Sort((x, y) => x.SortingOrder.CompareTo(y.SortingOrder));
        
        lvLevels.selectionChanged += LvLevelsOnSelectionChanged;

        lvLevels.itemsSource = _levels;

        lvLevels.selectedIndex = 0;
    }

    private void LvLevelsOnSelectionChanged(IEnumerable<object> selectedObjects)
    {
        var selectedObjectsArray = selectedObjects.ToArray();
        if (!selectedObjectsArray.Any())
        {
            // TODO: set selected level and selected cell to null
            return;
        }
        
        _selectedLevel = selectedObjectsArray[0] as Level;

        if (_selectedLevel == null)
        {
            // TODO: set selected level and selected cell to null
            return;
        }

        BindLevelLayout(_selectedLevel);
    }

    private void BindLevelLayout(Level level)
    {
        levelLayout.BindLevelLayout(level);
    }
    
    private void AddBrickTypeToCell(int row, int column)
    {
        var selectedBrickType = _brickTypeToolbar.SelectedBrickType;

        if (!selectedBrickType)
        {
            return;
        }
        
        levelLayout.SetCellBrick(row, column, new Brick(){ BrickType = selectedBrickType });
    }

    private void AddPowerUpToCell(int row, int column)
    {
        var selectedPowerUp = _powerUpToolbar.SelectedPowerUp;
        if (!selectedPowerUp)
        {
            return;
        }
        levelLayout.SetCellPowerUp(row, column, selectedPowerUp);
    }
}
