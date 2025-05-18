using System;
using System.Collections.Generic;
using DeeDeeR.BrickBreaker.PowerUps;
using Editor.GameDataInitializers;
using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class PowerUpToolbar: VisualElement
{
    public event EventHandler<PowerUp> PowerUpSelectionChanged;
    public event EventHandler<ToolbarMode> ToolbarModeChanged;

    private List<PowerUp> _powerUps = new();
    
    private PowerUp _selectedPowerUp = null;

    private ToolbarMode _toolbarMode = global::ToolbarMode.Set;
    
    public PowerUp SelectedPowerUp => _selectedPowerUp;

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
    
    public PowerUpToolbar()
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
        AddPowerUpButtons();
    }

    private void AddIgnoreButton()
    {
        _btnIgnore = AddButton(
            buttonName: "Ignore", 
            buttonText: "Ignore", 
            buttonTooltip: "Ignore selection", 
            sprite: null);
        
        _btnIgnore.clicked += OnIgnorePowerUpButtonClicked;
        
        _veMain.Add(_btnIgnore);
        _powerUps.Add(null);
    }
    
    private void AddUnsetButton()
    {
        _btnUnset = AddButton(
            buttonName: "Unset", 
            buttonText: "Unset", 
            buttonTooltip: "Cancel selection", 
            sprite: null);

        _btnUnset.clicked += OnUnsetPowerUpButtonClicked;

        _veMain.Add(_btnUnset);
        _powerUps.Add(null);
    }
    
    private void AddPowerUpButtons()
    {
        var powerUps = ScriptableObjectHelper.GetAllScriptableObjects<PowerUp>(PathHelper.PowerUpsPath);
        foreach (var powerUp in powerUps)
        {
            var button = AddButton(
                buttonName: powerUp.PowerUpName,
                buttonText: string.Empty,
                buttonTooltip: powerUp.PowerUpName,
                sprite: powerUp.Sprite);
            
            button.clicked += () => OnPowerUpButtonClicked(powerUp);

            _veMain.Add(button);
            _powerUps.Add(powerUp);
        }
    }

    private Button AddButton(string buttonName, string buttonText, string buttonTooltip, Sprite sprite = null)
    {
        var button = new Button();
        button.name = buttonName;
        button.text = buttonText;
        button.tooltip = buttonTooltip;
        button.AddToClassList("toolbar-button");
        if (sprite)
        {
            button.iconImage = Background.FromSprite(sprite);
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
    
    private void OnPowerUpButtonClicked(PowerUp powerUp)
    {
        ToolbarMode = ToolbarMode.Set;
        
        CancelCurrentSelection();

        if (!powerUp)
        {
            return;
        }
        SetSelection(powerUp);
        _selectedPowerUp = powerUp;

        PowerUpSelectionChanged?.Invoke(this, powerUp);
    }
    
    public void CancelCurrentSelection()
    {
        CancelSelection(_selectedPowerUp);
    }
    
    public void CancelSelection(PowerUp powerUp)
    {
        if (powerUp)
        {
            _veMain.Q<Button>(powerUp.PowerUpName).RemoveFromClassList("selected");
        }
    }
    
    public void SetSelection(PowerUp powerUp)
    {
        if (powerUp)
        {
            _veMain.Q<Button>(powerUp.PowerUpName).AddToClassList("selected");
        }
    }
}
public enum ToolbarMode
{
    Ignore,
    Unset,
    Set,
}
