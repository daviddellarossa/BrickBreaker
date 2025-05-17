using System;
using System.Collections.Generic;
using DeeDeeR.BrickBreaker.PowerUps;
using Editor.GameDataInitializers;
using UnityEngine.UIElements;

[UxmlElement]
public partial class PowerUpToolbar: VisualElement
{
    public event EventHandler<PowerUp> PowerUpSelectionChanged;

    private List<PowerUp> _powerUps = new();
    
    private PowerUp _selectedPowerUp = null;
    
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

    private void AddPowerUpButtons()
    {
        var powerUps = ScriptableObjectHelper.GetAllScriptableObjects<PowerUp>(PathHelper.PowerUpsPath);
        foreach (var powerUp in powerUps)
        {
            var button = new Button(() =>
            {
                PowerUpSelectionChanged?.Invoke(this, powerUp);
            });
            // button.text = powerUp.PowerUpName;
            button.name = powerUp.PowerUpName;
            button.AddToClassList("toolbar-button");
            button.iconImage = Background.FromSprite(powerUp.Sprite);
            
            button.clicked += () => OnPowerUpButtonClicked(powerUp);
    
            _veMain.Add(button);
            _powerUps.Add(powerUp);
        }
    }

    private void OnDetachFromPanel(DetachFromPanelEvent evt)
    {

    }
    
    private void AddNoneButton()
    {
        var noneButton = new Button(() =>
        {
            PowerUpSelectionChanged?.Invoke(this, null);
        });
        noneButton.text = "None";
        noneButton.AddToClassList("toolbar-button");
        
        noneButton.clicked += () => OnPowerUpButtonClicked(null);

        _veMain.Add(noneButton);
    }

    private void OnPowerUpButtonClicked(PowerUp powerUp)
    {
        if (_selectedPowerUp)
        {
            _veMain.Q<Button>(_selectedPowerUp.PowerUpName).RemoveFromClassList("selected");
        }

        if (powerUp == null)
        {
            return;
        }
        _veMain.Q<Button>(powerUp.PowerUpName).AddToClassList("selected");
        _selectedPowerUp = powerUp;

        PowerUpSelectionChanged?.Invoke(this, powerUp);
    }
}
