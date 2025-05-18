
using System;
using System.Collections.Generic;
using DeeDeeR.BrickBreaker.Bricks;
using Editor.GameDataInitializers;
using UnityEngine.UIElements;

[UxmlElement]
public partial class BrickTypeToolbar : VisualElement
{
    public event EventHandler<BrickType> BrickTypeSelectionChanged;
    
    private List<BrickType> _brickTypes = new();
    
    private BrickType _selectedBrickType = null;
    
    public BrickType SelectedBrickType => _selectedBrickType;
    
    private VisualElement _veMain;
    public BrickTypeToolbar()
    {
        // Register callback for when the element is attached to the panel
        RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
        RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);
    }
    
    private void OnAttachToPanel(AttachToPanelEvent evt)
    {
        _veMain = this.Q("veMain");
        
        AddNoneButton();
        AddBrickTypeButtons();
    }

    private void AddBrickTypeButtons()
    {
        var brickTypes = ScriptableObjectHelper.GetAllScriptableObjects<BrickType>(PathHelper.BrickTypesPath);

        foreach (var brickType in brickTypes)
        {
            var button = AddButton(brickType, brickType.BrickTypeName, string.Empty, brickType.BrickTypeName, brickType.Color);

            _veMain.Add(button);
            _brickTypes.Add(null);
        }
    }
    
    private void AddNoneButton()
    {
        var button = AddButton(null, "None", "None","No selection");;
        
        _veMain.Add(button);
        _brickTypes.Add(null);
    }

    private Button AddButton(BrickType brickType, string buttonName, string buttonText, string buttonTooltip, UnityEngine.Color? color = null)
    {
        var button = new Button(() =>
        {
            BrickTypeSelectionChanged?.Invoke(this, brickType);
        });
        button.name = buttonName;
        button.text = buttonText;
        button.tooltip = buttonTooltip;
        button.AddToClassList("toolbar-button");
        if (color.HasValue)
        {
            button.style.backgroundColor = color.Value;
        }
        button.clicked += () => OnBrickTypeButtonClicked(brickType);
        return button;
    }

    private void OnDetachFromPanel(DetachFromPanelEvent evt)
    {

    }
    
    private void OnBrickTypeButtonClicked(BrickType brickType)
    {
        CancelCurrentSelection();

        if (brickType == null)
        {
            return;
        }
        _veMain.Q<Button>(brickType.BrickTypeName).AddToClassList("selected");
        _selectedBrickType = brickType;

        BrickTypeSelectionChanged?.Invoke(this, brickType);
    }

    public void CancelCurrentSelection()
    {
        if (_selectedBrickType)
        {
            _veMain.Q<Button>(_selectedBrickType.BrickTypeName).RemoveFromClassList("selected");
        }
    }
}
