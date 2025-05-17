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

    private VisualElement veLevelLayout;

    private ListView lvLevels;
    private LevelLayout levelLayout;
    
    private List<Level> _levels = new List<Level>();

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
        
        veLevelLayout = veCentrePanel.Q<VisualElement>("veLevelLayout");
        
        lvLevels = veLeftPanel.Q<ListView>("lvLevels");
        levelLayout = veCentrePanel.Q<LevelLayout>("levelLayout");

        BindLevelToolbar();
        BindBrickTypeToolbar();
        BindPowerUpToolbar();
        BindLevels();
    }

    private void BindBrickTypeToolbar()
    {
        var brickTypeToolbar = veLeftPanel.Q<BrickTypeToolbar>("brickTypeToolbar");
        brickTypeToolbar.BrickTypeSelectionChanged += BrickTypeToolbarOnBrickTypeSelectionChanged;
    }

    private void BrickTypeToolbarOnBrickTypeSelectionChanged(object sender, BrickType e)
    {
        
    }

    private void BindPowerUpToolbar()
    {
        var powerUpToolbar = veLeftPanel.Q<PowerUpToolbar>("powerUpToolbar");
        powerUpToolbar.PowerUpSelectionChanged += PowerUpToolbarOnPowerUpSelectionChanged;

    }

    private void PowerUpToolbarOnPowerUpSelectionChanged(object sender, PowerUp e)
    {
        
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
        
        lvLevels.selectionChanged += LvLevelsOnselectionChanged;

        lvLevels.itemsSource = _levels;

        lvLevels.selectedIndex = 0;
    }

    private Level _selectedLevel = null;
    private (int row, int column)? _selectedCell = null;

    private void LvLevelsOnselectionChanged(IEnumerable<object> selectedObjects)
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
        veLevelLayout.Clear();

        
        for (int i = 0; i < level.NumRows; i++)
        {
            var veRow = new VisualElement();
            veRow.name = $"Row{i}";
            veRow.AddToClassList("layout-row");
            veLevelLayout.Add(veRow);
            
            for (int j = 0; j < level.NumColumns; j++)
            {
                var veCell = new VisualElement();
                veCell.name = $"Cell{i}-{j}";
                veCell.AddToClassList("layout-cell");
                veCell.style.flexDirection = FlexDirection.Column;
                int row = i;
                int col = j;
                veCell.focusable = true;
                veCell.RegisterCallback<ClickEvent>(evt => OnCellClicked(row, col));
                veCell.RegisterCallback((ContextualMenuPopulateEvent evt) =>
                {
                    evt.menu.AppendAction("Action A", (x) => { });
                    evt.menu.AppendAction("Action B", (x) => { });
                });
                veCell.AddManipulator(new ContextualMenuManipulator((ContextualMenuPopulateEvent evt) =>
                {
                    Debug.Log("Contextual menu");
                }));
                
                veRow.Add(veCell);
            }
        }
    }

    private void OnCellClicked(int row, int col)
    {
        VisualElement cellElement = null;
        if (_selectedCell.HasValue)
        {
            cellElement = veLevelLayout.Q<VisualElement>($"Cell{_selectedCell.Value.row}-{_selectedCell.Value.column}");
            cellElement?.RemoveFromClassList("selected");
        }
        
        Debug.Log($"Cell clicked: Row {row}, Column {col}");
        _selectedCell = (row, col);
    
        cellElement = veLevelLayout.Q($"Cell{row}-{col}");
        cellElement?.AddToClassList("selected");
    }
}
