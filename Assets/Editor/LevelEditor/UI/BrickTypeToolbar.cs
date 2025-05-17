
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
            var button = new Button(() =>
            {
                BrickTypeSelectionChanged?.Invoke(this, brickType);
            });
            button.name = brickType.BrickTypeName;
            button.AddToClassList("toolbar-button");
            button.style.backgroundColor = brickType.Color;
            
            button.clicked += () => OnBrickTypeButtonClicked(brickType);
            
            _veMain.Add(button);
        }
    }

    private void AddNoneButton()
    {
        var noneButton = new Button(() =>
        {
            BrickTypeSelectionChanged?.Invoke(this, null);
        });
        noneButton.text = "None";
        noneButton.AddToClassList("toolbar-button");
        
        noneButton.clicked += () => OnBrickTypeButtonClicked(null);

        _veMain.Add(noneButton);
    }

    private void OnDetachFromPanel(DetachFromPanelEvent evt)
    {

    }
    
    private void OnBrickTypeButtonClicked(BrickType brickType)
    {
        if (_selectedBrickType)
        {
            _veMain.Q<Button>(_selectedBrickType.BrickTypeName).RemoveFromClassList("selected");
        }

        if (brickType == null)
        {
            return;
        }
        _veMain.Q<Button>(brickType.BrickTypeName).AddToClassList("selected");
        _selectedBrickType = brickType;

        BrickTypeSelectionChanged?.Invoke(this, brickType);
    }
}
