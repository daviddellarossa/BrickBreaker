using System;
using DeeDeeR.BrickBreaker.Bricks;
using DeeDeeR.BrickBreaker.Levels;
using DeeDeeR.BrickBreaker.PowerUps;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class LevelLayout : VisualElement
{
    public event EventHandler<CellSelectedEventArgs> CellSelected;
    
    private (int row, int column)? _selectedCell = null;
    
    private Level _level;
    
    private VisualElement _veMain;

    public LevelLayout()
    {
        RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
        RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);
    }

    private void OnDetachFromPanel(DetachFromPanelEvent evt)
    {
        
    }

    private void OnAttachToPanel(AttachToPanelEvent evt)
    {
        _veMain = this.Q<VisualElement>("veMain");
    }
    
    public void BindLevelLayout(Level level)
    {
        this._level = level;
        
        _veMain.Clear();
      
        for (int i = 0; i < level.NumRows; i++)
        {
            var veRow = new VisualElement();
            veRow.name = $"Row{i}";
            veRow.AddToClassList("layout-row");
            _veMain.Add(veRow);
            
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
                
                var cell = level.Cells[i * level.NumColumns + j];
                if (cell.Brick.BrickType)
                {
                    veCell.style.backgroundColor = cell.Brick.BrickType.Color;
                }

                if (cell.PowerUp)
                {
                    veCell.style.backgroundImage = Background.FromSprite(cell.PowerUp.Sprite);
                    veCell.style.backgroundSize = new BackgroundSize(BackgroundSizeType.Contain);
                    veCell.style.unityBackgroundImageTintColor = Color.white;
                }
                
                veRow.Add(veCell);
            }
        }
    }

    public void SetCellPowerUp(int row, int col, PowerUp powerUp)
    {
        var cell = _level.Cells[row * _level.NumColumns + col];
        cell.PowerUp = powerUp;
        
        var veCell = _veMain.Q<VisualElement>($"Cell{row}-{col}");
        veCell.style.backgroundImage = Background.FromSprite(powerUp.Sprite);
        veCell.style.backgroundSize = new BackgroundSize(BackgroundSizeType.Contain);
        veCell.style.unityBackgroundImageTintColor = Color.white;
        
        EditorUtility.SetDirty(_level);
    }

    public void SetCellBrick(int row, int col, Brick brick)
    {
        var cell = _level.Cells[row * _level.NumColumns + col];
        cell.Brick = brick;
        
        var veCell = _veMain.Q<VisualElement>($"Cell{row}-{col}");
        if (brick.BrickType)
        {
            veCell.style.backgroundColor = brick.BrickType.Color;
        }
        
        EditorUtility.SetDirty(_level);
    }
    
    private void OnCellClicked(int row, int col)
    {
        VisualElement cellElement = null;
        if (_selectedCell.HasValue)
        {
            cellElement = _veMain.Q<VisualElement>($"Cell{_selectedCell.Value.row}-{_selectedCell.Value.column}");
            cellElement?.RemoveFromClassList("selected");
        }
        
        Debug.Log($"Cell clicked: Row {row}, Column {col}");
        _selectedCell = (row, col);
    
        cellElement = _veMain.Q($"Cell{row}-{col}");
        cellElement?.AddToClassList("selected");
        
        CellSelected?.Invoke(this, new CellSelectedEventArgs()
        {
            Row = row,
            Column = col
        });
    }
}

public class CellSelectedEventArgs
{
    public int Row { get; set; }
    public int Column { get; set; }
}
