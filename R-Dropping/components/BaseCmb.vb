Imports Guna.UI2.WinForms


Public Class BaseComboBox
    Inherits UserControl
    Implements IValueProvider
    Implements IValidationStyleable

    Public Event SelectedValueChanged As EventHandler
    Public Event ValueChanged As EventHandler Implements IValueProvider.ValueChanged

    Public Property Items As New List(Of String)
    Public Property Placeholder As String = "Select an option..."
    Public Property SearchEnabled As Boolean = True

    Private _selectedValue As String = ""
    Private WithEvents _btn As New Guna2Button
    Private _dropdown As ComboDropdownPanel
    Private _label As BaseLabel
    Private _cmbName As String

    Public ReadOnly Property Value As String Implements IValueProvider.Value
        Get
            Return _selectedValue
        End Get
    End Property

    '  SelectedValue (public, raises both events) 

    Public Property SelectedValue As String
        Get
            Return _selectedValue
        End Get
        Set(value As String)
            _selectedValue = value
            UpdateButtonAppearance()
            RaiseEvent SelectedValueChanged(Me, EventArgs.Empty)
            RaiseEvent ValueChanged(Me, EventArgs.Empty)
        End Set
    End Property

    '  IValidationStyleable 

    Public Sub OnValidationError() Implements IValidationStyleable.OnValidationError
        _btn.BorderColor = Color.Red
    End Sub

    Public Sub OnValidationClear() Implements IValidationStyleable.OnValidationClear
        _btn.BorderColor = Color.FromArgb(220, 220, 220)
    End Sub

    '  Constructor 

    Public Sub New(cmbName As String)
        Me.AutoSize = True
        Me.AutoSizeMode = AutoSizeMode.GrowAndShrink
        _cmbName = cmbName

        With _btn
            .Dock = DockStyle.Top
            .Height = 36
            .FillColor = Color.White
            .ForeColor = Color.FromArgb(150, 150, 150)
            .BorderColor = Color.FromArgb(220, 220, 220)
            .BorderRadius = 4
            .BorderThickness = 1
            .Font = New Font("Segoe UI", 9.0F)
            .TextAlign = HorizontalAlignment.Left
            .HoverState.FillColor = Color.FromArgb(248, 250, 252)
            .Cursor = Cursors.Hand
        End With

        _label = New BaseLabel
        With _label
            .Text = cmbName
            .Dock = DockStyle.Top
            .SetSmall()
            .Padding = New Padding(0, 0, 0, 4)
        End With

        UpdateButtonAppearance()

        Me.Controls.Add(_btn)
        Me.Controls.Add(_label)
    End Sub

    '  Button appearance 

    Private Sub UpdateButtonAppearance()
        Dim hasValue = Not String.IsNullOrWhiteSpace(_selectedValue)
        _btn.Text = If(hasValue, "  " & _selectedValue, Placeholder)
        _btn.ForeColor = If(hasValue,
                            Color.FromArgb(20, 20, 20),
                            Color.FromArgb(150, 150, 150))
    End Sub

    '  Dropdown toggle

    Private Sub _btn_Click(sender As Object, e As EventArgs) Handles _btn.Click
        If _dropdown IsNot Nothing Then
            _dropdown.CloseDropdown()
            _dropdown = Nothing
            Return
        End If

        Dim parentForm = Me.FindForm()
        If parentForm Is Nothing Then Return

        _dropdown = New ComboDropdownPanel(Items, _selectedValue, Not SearchEnabled)
        _dropdown.Width = Me.Width

        Dim screenPoint = Me.Parent.PointToScreen(Me.Location)
        Dim formPoint = parentForm.PointToClient(screenPoint)
        _dropdown.Location = New Point(formPoint.X - 4, formPoint.Y + Me.Height + 4)

        AddHandler _dropdown.ItemSelected,
            Sub(value As String)
                SelectedValue = value
            End Sub

        AddHandler _dropdown.DropdownClosed,
            Sub()
                _dropdown = Nothing
            End Sub

        parentForm.Controls.Add(_dropdown)
        _dropdown.BringToFront()
        _dropdown.OpenDropdown()
    End Sub

    ' ── Public helpers ───────────────────────────────────────────

    Public Sub ClearSelection()
        SelectedValue = String.Empty
    End Sub

    Private Sub InitializeComponent()
        Me.SuspendLayout()
        '
        'BaseComboBox
        '
        Me.Name = _cmbName
        Me.ResumeLayout(False)

    End Sub

    Private Sub BaseComboBox_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class


' 
' COMBO DROPDOWN PANEL 
' 
Public Class ComboDropdownPanel
    Inherits Guna2Panel

    Public Event ItemSelected(value As String)
    Public Event DropdownClosed()

    Private ReadOnly _allItems As List(Of String)
    Private ReadOnly _current As String
    Private ReadOnly _disableSearch As Boolean

    Private WithEvents _search As New Guna2TextBox
    Private _list As New FlowLayoutPanel
    Private _empty As New Label
    Private _timer As New Timer With {.Interval = 100}

    Public Sub New(items As List(Of String), current As String,
                   Optional disableSearch As Boolean = False)
        _allItems = items
        _current = current
        _disableSearch = disableSearch

        Me.FillColor = Color.White
        Me.BorderRadius = 10
        Me.BorderThickness = 1
        Me.BorderColor = Color.FromArgb(220, 220, 220)
        Me.ShadowDecoration.Enabled = True
        Me.ShadowDecoration.Depth = 8
        Me.Padding = New Padding(8)
        Me.Visible = False

        BuildUI()
    End Sub

    Private Sub BuildUI()
        Dim flow As New FlowLayoutPanel With {
            .Dock = DockStyle.Fill,
            .FlowDirection = FlowDirection.TopDown,
            .WrapContents = False,
            .AutoScroll = False,
            .Padding = New Padding(0)
        }

        If Not _disableSearch Then
            With _search
                .Width = Me.Width - 16
                .Height = 32
                .PlaceholderText = "Search..."
                .FillColor = Color.FromArgb(248, 250, 252)
                .BorderRadius = 7
                .BorderColor = Color.FromArgb(220, 220, 220)
                .FocusedState.BorderColor = Color.FromArgb(99, 102, 241)
                .Font = New Font("Segoe UI", 9.5F)
                .Margin = New Padding(0, 0, 0, 6)
                .Anchor = AnchorStyles.Left Or AnchorStyles.Right
            End With
            flow.Controls.Add(_search)
        End If

        _list = New FlowLayoutPanel With {
            .Width = Me.Width - 16,
            .FlowDirection = FlowDirection.TopDown,
            .WrapContents = False,
            .AutoScroll = True,
            .Margin = New Padding(0)
        }

        _empty = New Label With {
            .Text = "No results found.",
            .Font = New Font("Segoe UI", 9.0F),
            .ForeColor = Color.FromArgb(150, 150, 150),
            .TextAlign = ContentAlignment.MiddleCenter,
            .Width = Me.Width - 16,
            .Height = 40,
            .Visible = False
        }

        flow.Controls.Add(_list)
        flow.Controls.Add(_empty)
        Controls.Add(flow)

        AddHandler _timer.Tick, AddressOf CheckOutsideClick
    End Sub

    Public Sub OpenDropdown()
        Me.Visible = True
        Populate(_allItems)
        If Not _disableSearch Then _search.Focus()
        _timer.Start()
    End Sub

    Public Sub CloseDropdown()
        _timer.Stop()
        If Me.Parent IsNot Nothing Then Me.Parent.Controls.Remove(Me)
        RaiseEvent DropdownClosed()
        Me.Dispose()
    End Sub

    Private Sub Populate(items As List(Of String))
        _list.Controls.Clear()

        If items.Count = 0 Then
            _empty.Visible = True
            _list.Visible = False
            Me.Height = 110
            ResizeLayout()
            Return
        End If

        _empty.Visible = False
        _list.Visible = True

        For Each item In items
            Dim isSelected = (item = _current)
            Dim btn As New Guna2Button With {
                .Text = If(isSelected, "  ✓  " & item, "     " & item),
                .Height = 32,
                .Width = Me.Width - 24,
                .FillColor = If(isSelected, Color.FromArgb(238, 242, 255), Color.White),
                .ForeColor = If(isSelected, Color.FromArgb(79, 70, 229), Color.FromArgb(30, 41, 59)),
                .BorderRadius = 6,
                .BorderThickness = 0,
                .Font = New Font("Segoe UI", 9.5F,
                                 If(isSelected, FontStyle.Bold, FontStyle.Regular)),
                .TextAlign = HorizontalAlignment.Left,
                .Cursor = Cursors.Hand,
                .Tag = item,
                .Margin = New Padding(0, 0, 0, 4)
            }
            btn.HoverState.FillColor = Color.FromArgb(238, 242, 255)
            btn.HoverState.ForeColor = Color.FromArgb(79, 70, 229)

            AddHandler btn.Click,
                Sub(sender As Object, e As EventArgs)
                    RaiseEvent ItemSelected(btn.Tag.ToString())
                    CloseDropdown()
                End Sub

            _list.Controls.Add(btn)
        Next

        Dim visibleCount = Math.Min(items.Count, 6)
        Dim searchHeight = If(_disableSearch, 0, 48)
        Me.Height = searchHeight + (visibleCount * 40) + 16
        ResizeLayout()
    End Sub

    Private Sub ResizeLayout()
        Dim innerWidth = Me.Width - 16
        _search.Width = innerWidth
        _list.Width = innerWidth
        _list.Height = Me.Height - If(_disableSearch, 16, 54)
        For Each ctrl As Control In _list.Controls
            ctrl.Width = innerWidth - 5
        Next
        _empty.Width = innerWidth
    End Sub

    Private Sub _search_TextChanged(sender As Object, e As EventArgs) Handles _search.TextChanged
        Dim query = _search.Text.Trim().ToLower()
        Dim filtered = If(String.IsNullOrWhiteSpace(query),
                          _allItems,
                          _allItems.Where(Function(x) x.ToLower().Contains(query)).ToList())
        Populate(filtered)
    End Sub

    Protected Overrides Sub OnResize(e As EventArgs)
        MyBase.OnResize(e)
        ResizeLayout()
    End Sub

    Private Sub CheckOutsideClick(sender As Object, e As EventArgs)
        If Control.MouseButtons = MouseButtons.Left Then
            If Not RectangleToScreen(Me.ClientRectangle).Contains(Cursor.Position) Then
                CloseDropdown()
            End If
        End If
    End Sub

    Protected Overrides Sub Dispose(disposing As Boolean)
        If disposing Then
            _timer.Stop()
            _timer.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

End Class