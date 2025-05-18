
using System;
using System.Collections.Generic;
using DeeDeeR.BrickBreaker.Bricks;
using Editor.GameDataInitializers;
using UnityEngine.UIElements;

[UxmlElement]
public partial class BrickTypeToolbar : VisualElement
{
    public event EventHandler<BrickType> BrickTypeSelectionChanged;
    public event EventHandler<ToolbarMode> ToolbarModeChanged;
    
    private List<BrickType> _brickTypes = new();
    
    private BrickType _selectedBrickType = null;
    
    private ToolbarMode _toolbarMode = global::ToolbarMode.Set;
    
    public BrickType SelectedBrickType => _selectedBrickType;
    
    private VisualElement _veMain;
    private Button _btnIgnore;
    private Button _btnUnset;
    
    public ToolbarMode ToolbarMode
    {
        get => _toolbarMode;
        private set
        {
            _toolbarMode = value;
            CancelCurrentSelection();
            switch (value)
            {
                case ToolbarMode.Ignore:
                    _btnIgnore.AddToClassList("selected");
                    _btnUnset.RemoveFromClassList("selected");
                    break;   
                case ToolbarMode.Unset:
                    _btnIgnore.RemoveFromClassList("selected");
                    _btnUnset.AddToClassList("selected");
                    break;
                case ToolbarMode.Set:
                    _btnIgnore.RemoveFromClassList("selected");
                    _btnUnset.RemoveFromClassList("selected");
                    break;
                default:
                    break;
            }
            ToolbarModeChanged?.Invoke(this, value);
        }   
    }
    
    public BrickTypeToolbar()
    {
        // Register callback for when the element is attached to the panel
        RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
        RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);
    }
    
    private void OnAttachToPanel(AttachToPanelEvent evt)
    {
        _veMain = this.Q("veMain");
        
        AddIgnoreButton();
        AddUnsetButton();
        AddBrickTypeButtons();
    }
    
    private void AddIgnoreButton()
    {
        _btnIgnore = AddButton(
            brickType: null, 
            buttonName: "Ignore", 
            buttonText: "Ignore", 
            buttonTooltip: "Ignore selection");
        
        _btnIgnore.clicked += OnIgnorePowerUpButtonClicked;

        _veMain.Add(_btnIgnore);
        _brickTypes.Add(null);
    }
    
    private void AddUnsetButton()
    {
        _btnUnset = AddButton(
            brickType: null, 
            buttonName: "Unset", 
            buttonText: "Unset", 
            buttonTooltip: "Cancel selection");

        _btnUnset.clicked += OnUnsetPowerUpButtonClicked;

        _veMain.Add(_btnUnset);
        _brickTypes.Add(null);
    }

    private void AddBrickTypeButtons()
    {
        var brickTypes = ScriptableObjectHelper.GetAllScriptableObjects<BrickType>(PathHelper.BrickTypesPath);

        foreach (var brickType in brickTypes)
        {
            var button = AddButton(
                brickType: brickType, 
                buttonName: brickType.BrickTypeName, 
                buttonText: string.Empty, 
                buttonTooltip: brickType.BrickTypeName, 
                color: brickType.Color);

            button.clicked += () => OnBrickTypeButtonClicked(brickType);
            
            _veMain.Add(button);
            _brickTypes.Add(null);
        }
    }
    
    private Button AddButton(BrickType brickType, string buttonName, string buttonText, string buttonTooltip, UnityEngine.Color? color = null)
    {
        var button = new Button();
        button.name = buttonName;
        button.text = buttonText;
        button.tooltip = buttonTooltip;
        button.AddToClassList("toolbar-button");
        if (color.HasValue)
        {
            button.style.backgroundColor = color.Value;
        }
        
        return button;
    }

    private void OnIgnorePowerUpButtonClicked()
    {
        this.ToolbarMode = ToolbarMode.Ignore;
    }
    
    private void OnUnsetPowerUpButtonClicked()
    {
        CancelCurrentSelection();
        this.ToolbarMode = ToolbarMode.Unset;
    }
    
    private void OnDetachFromPanel(DetachFromPanelEvent evt)
    {

    }
    
    private void OnBrickTypeButtonClicked(BrickType brickType)
    {
        ToolbarMode = ToolbarMode.Set;
        
        CancelCurrentSelection();

        if (!brickType)
        {
            return;
        }
        SetSelection(brickType);
        _selectedBrickType = brickType;

        BrickTypeSelectionChanged?.Invoke(this, brickType);
    }

    public void CancelCurrentSelection()
    {
        CancelSelection(_selectedBrickType);
    }
    
    public void CancelSelection(BrickType brickType)
    {
        if (brickType)
        {
            _veMain.Q<Button>(brickType.BrickTypeName).RemoveFromClassList("selected");
        }
    }
    
    public void SetSelection(BrickType brickType)
    {
        if (brickType)
        {
            _veMain.Q<Button>(brickType.BrickTypeName).AddToClassList("selected");
        }
    }
}
