using System;
using UnityEditorInternal;
using UnityEngine.UIElements;

[UxmlElement]
public partial class LevelToolbar : VisualElement
{
    public event EventHandler SortListEvent;
    public event EventHandler CreateLevelEvent;
    public event EventHandler DeleteLevelEvent;
    
    private Button _btnCreateLevel;
    private Button _btnDeleteLevel;
    private Button _btnSortList;

    public LevelToolbar()
    {
        // Register callback for when the element is attached to the panel
        RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
        RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);
    }
    
    private void OnAttachToPanel(AttachToPanelEvent evt)
    {
        // Initialize button references after the element is attached to the panel
        _btnCreateLevel = this.Q<Button>("btnCreateLevel");
        _btnDeleteLevel = this.Q<Button>("btnDeleteLevel");
        _btnSortList = this.Q<Button>("btnSortList");

        // Register button callbacks
        if (_btnCreateLevel != null)
            _btnCreateLevel.clicked += () => CreateLevelEvent?.Invoke(this, EventArgs.Empty);
        
        if (_btnDeleteLevel != null)
            _btnDeleteLevel.clicked += () => DeleteLevelEvent?.Invoke(this, EventArgs.Empty);
        
        if (_btnSortList != null)
            _btnSortList.clicked += () => SortListEvent?.Invoke(this, EventArgs.Empty);
    }

    private void OnDetachFromPanel(DetachFromPanelEvent evt)
    {
        // Unregister button callbacks
        if (_btnCreateLevel != null)
            _btnCreateLevel.clicked -= () => CreateLevelEvent?.Invoke(this, EventArgs.Empty);
        
        if (_btnDeleteLevel != null)
            _btnDeleteLevel.clicked -= () => DeleteLevelEvent?.Invoke(this, EventArgs.Empty);
        
        if (_btnSortList != null)
            _btnSortList.clicked -= () => SortListEvent?.Invoke(this, EventArgs.Empty);
    }



    private void OnEnable()
    {
        this.Q<Button>("btnCreateLevel").RegisterCallback<ClickEvent>(e => CreateLevelEvent?.Invoke(this, EventArgs.Empty));
        this.Q<Button>("btnDeleteLevel").RegisterCallback<ClickEvent>(e => DeleteLevelEvent?.Invoke(this, EventArgs.Empty));
        this.Q<Button>("btnSortList").RegisterCallback<ClickEvent>(e => SortListEvent?.Invoke(this, EventArgs.Empty));
    }
    private void OnDisable()
    {
        this.Q<Button>("btnCreateLevel").UnregisterCallback<ClickEvent>(e => CreateLevelEvent?.Invoke(this, EventArgs.Empty));
        this.Q<Button>("btnDeleteLevel").UnregisterCallback<ClickEvent>(e => DeleteLevelEvent?.Invoke(this, EventArgs.Empty));
        this.Q<Button>("btnSortList").UnregisterCallback<ClickEvent>(e => SortListEvent?.Invoke(this, EventArgs.Empty));
    }
}
