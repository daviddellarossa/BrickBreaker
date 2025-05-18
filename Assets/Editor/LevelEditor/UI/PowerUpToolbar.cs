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

    private List<PowerUp> _powerUps = new();
    
    private PowerUp _selectedPowerUp = null;
    
    public PowerUp SelectedPowerUp => _selectedPowerUp;
    
    private VisualElement _veMain;
    
    public PowerUpToolbar()
    {
        // Register callback for when the element is attached to the panel
        RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
        RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);
    }
    
    private void OnAttachToPanel(AttachToPanelEvent evt)
    {
        _veMain = this.Q("veMain");
        
        AddNoneButton();
        AddPowerUpButtons();
    }

    private void AddNoneButton()
    {
        var button = AddButton(
            powerUp: null, 
            buttonName: "None", 
            buttonText: "None", 
            buttonTooltip: "No selection", 
            sprite: null);

        _veMain.Add(button);
        _powerUps.Add(null);
    }

    private void AddPowerUpButtons()
    {
        var powerUps = ScriptableObjectHelper.GetAllScriptableObjects<PowerUp>(PathHelper.PowerUpsPath);
        foreach (var powerUp in powerUps)
        {
            var button = AddButton(
                powerUp: powerUp,
                buttonName: powerUp.PowerUpName,
                buttonText: string.Empty,
                buttonTooltip: powerUp.PowerUpName,
                sprite: powerUp.Sprite);

            _veMain.Add(button);
            _powerUps.Add(powerUp);
        }
    }

    private Button AddButton(PowerUp powerUp, string buttonName, string buttonText, string buttonTooltip, Sprite sprite = null)
    {
        var button = new Button(() =>
        {
            PowerUpSelectionChanged?.Invoke(this, powerUp);
        });
        button.name = buttonName;
        button.text = buttonText;
        button.tooltip = buttonTooltip;
        button.AddToClassList("toolbar-button");
        if (sprite)
        {
            button.iconImage = Background.FromSprite(sprite);
        }
        
        button.clicked += () => OnPowerUpButtonClicked(powerUp);
        return button;
    }

    private void OnDetachFromPanel(DetachFromPanelEvent evt)
    {

    }
    
    private void OnPowerUpButtonClicked(PowerUp powerUp)
    {
        CancelCurrentSelection();

        if (powerUp == null)
        {
            return;
        }
        _veMain.Q<Button>(powerUp.PowerUpName).AddToClassList("selected");
        _selectedPowerUp = powerUp;

        PowerUpSelectionChanged?.Invoke(this, powerUp);
    }
    
    public void CancelCurrentSelection()
    {
        if (_selectedPowerUp)
        {
            _veMain.Q<Button>(_selectedPowerUp.PowerUpName).RemoveFromClassList("selected");
        }
    }
}
