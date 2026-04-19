
Imports Guna.UI2.WinForms

Public Class SearchableComboBox
    Inherits UserControl

    Public Event SelectedValueChanged As EventHandler

    Public Property Items As New List(Of String)
    Public Property Placeholder As String = "Select an option..."

    Private _selectedValue As String = ""
    Private WithEvents _btn As New Guna2Button
    Private _dropdown As ComboDropdownPanel

    Public Property SelectedValue As String
        Get
            Return _selectedValue
        End Get
        Set(value As String)
            _selectedValue = value
            _btn.Text = If(String.IsNullOrWhiteSpace(value), Placeholder, "  " & value)
            _btn.ForeColor = If(String.IsNullOrWhiteSpace(value),
                                Color.FromArgb(150, 150, 150),
                                Color.FromArgb(20, 20, 20))
        End Set
    End Property

    Public Sub New()
        Me.Size = New Size(260, 40)
        Me.BackColor = Color.Transparent

        With _btn
            .Dock = DockStyle.Fill
            .Text = Placeholder
            .FillColor = Color.White
            .ForeColor = Color.FromArgb(150, 150, 150)
            .BorderColor = Color.FromArgb(220, 220, 220)
            .BorderRadius = 8
            .BorderThickness = 1
            .Font = New Font("Segoe UI", 9.5F)
            .TextAlign = HorizontalAlignment.Left
            .HoverState.FillColor = Color.FromArgb(248, 250, 252)
            .Cursor = Cursors.Hand
        End With

        Controls.Add(_btn)
    End Sub

    Private Sub _btn_Click(sender As Object, e As EventArgs) Handles _btn.Click
        If _dropdown IsNot Nothing Then
            _dropdown.CloseDropdown()
            _dropdown = Nothing
            Return
        End If

        Dim parentForm = Me.FindForm()
        If parentForm Is Nothing Then Return

        _dropdown = New ComboDropdownPanel(Items, _selectedValue)
        _dropdown.Width = Me.Width

        Dim screenPoint = Me.Parent.PointToScreen(Me.Location)
        Dim formPoint = parentForm.PointToClient(screenPoint)

        _dropdown.Location = New Point(formPoint.X, formPoint.Y + Me.Height + 4)

        AddHandler _dropdown.ItemSelected,
            Sub(value As String)
                SelectedValue = value
                RaiseEvent SelectedValueChanged(Me, EventArgs.Empty)
            End Sub

        AddHandler _dropdown.DropdownClosed,
            Sub()
                _dropdown = Nothing
            End Sub

        parentForm.Controls.Add(_dropdown)
        _dropdown.BringToFront()
        _dropdown.OpenDropdown()
    End Sub

    Public Sub ClearSelection()
        SelectedValue = ""
        RaiseEvent SelectedValueChanged(Me, EventArgs.Empty)
    End Sub

    Private Sub InitializeComponent()
        Me.SuspendLayout()
        '
        'SearchableComboBox
        '
        Me.Name = "SearchableComboBox"
        Me.ResumeLayout(False)

    End Sub

    Private Sub SearchableComboBox_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class

Public Class ComboDropdownPanel
    Inherits Guna2Panel

    Public Event ItemSelected(value As String)
    Public Event DropdownClosed()

    Private ReadOnly _allItems As List(Of String)
    Private ReadOnly _current As String

    Private WithEvents _search As New Guna2TextBox
    Private _list As New FlowLayoutPanel
    Private _empty As New Label
    Private _timer As New Timer With {.Interval = 100}

    Public Sub New(items As List(Of String), current As String)
        _allItems = items
        _current = current

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
        With _search
            .Location = New Point(8, 8)
            .Height = 28
            .PlaceholderText = "Search..."
            .FillColor = Color.FromArgb(248, 250, 252)
            .BorderRadius = 7
            .BorderColor = Color.FromArgb(220, 220, 220)
            .FocusedState.BorderColor = Color.FromArgb(99, 102, 241)
            .Font = New Font("Segoe UI", 9.5F)
            .Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        End With

        _list = New FlowLayoutPanel With {
            .Location = New Point(8, 52),
            .FlowDirection = FlowDirection.TopDown,
            .WrapContents = False,
            .AutoScroll = True,
            .Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right Or AnchorStyles.Bottom
        }

        _empty = New Label With {
            .Text = "No results found.",
            .Font = New Font("Segoe UI", 9.0F),
            .ForeColor = Color.FromArgb(150, 150, 150),
            .TextAlign = ContentAlignment.MiddleCenter,
            .Visible = False
        }

        Controls.Add(_search)
        Controls.Add(_list)
        Controls.Add(_empty)

        AddHandler _timer.Tick, AddressOf CheckOutsideClick
    End Sub

    Public Sub OpenDropdown()
        Me.Visible = True
        Populate(_allItems)
        _search.Focus()
        _timer.Start()
    End Sub

    Public Sub CloseDropdown()
        _timer.Stop()

        If Me.Parent IsNot Nothing Then
            Me.Parent.Controls.Remove(Me)
        End If

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
                .FillColor = If(isSelected,
                                Color.FromArgb(238, 242, 255),
                                Color.White),
                .ForeColor = If(isSelected,
                                Color.FromArgb(79, 70, 229),
                                Color.FromArgb(30, 41, 59)),
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
        Me.Height = 60 + (visibleCount * 40) + 10

        ResizeLayout()
    End Sub

    Private Sub ResizeLayout()
        Dim innerWidth = Me.Width - 16

        _search.Width = innerWidth
        _list.Width = innerWidth
        _list.Height = Me.Height - 60

        For Each ctrl As Control In _list.Controls
            ctrl.Width = innerWidth - 5
        Next

        _empty.Location = New Point(8, 55)
        _empty.Size = New Size(innerWidth, 40)
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
            Dim mousePos = Cursor.Position
            Dim screenBounds = RectangleToScreen(Me.ClientRectangle)

            If Not screenBounds.Contains(mousePos) Then
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
