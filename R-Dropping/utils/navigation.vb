Public Class NavigationManager

    Private ReadOnly _hostPanel As Control
    Private ReadOnly _history As New Stack(Of Control)
    Private _currentPage As Control

    Public Sub New(hostPanel As Control)
        _hostPanel = hostPanel
    End Sub

    Public ReadOnly Property CurrentPage As Control
        Get
            Return _currentPage
        End Get
    End Property

    Public Function CanGoBack() As Boolean
        Return _history.Count > 0
    End Function

    Public Sub GoToPage(page As Control)
        If page Is Nothing Then Exit Sub

        If _currentPage IsNot Nothing Then
            _history.Push(_currentPage)
            _hostPanel.Controls.Remove(_currentPage)
        End If

        _currentPage = page

        With _currentPage
            .Dock = DockStyle.Fill
            .Visible = True
        End With

        _hostPanel.Controls.Clear()
        _hostPanel.Controls.Add(_currentPage)
        _currentPage.BringToFront()
    End Sub

    Public Sub ReplacePage(page As Control)
        If page Is Nothing Then Exit Sub

        If _currentPage IsNot Nothing Then
            _hostPanel.Controls.Remove(_currentPage)
        End If

        _currentPage = page

        With _currentPage
            .Dock = DockStyle.Fill
            .Visible = True
        End With

        _hostPanel.Controls.Clear()
        _hostPanel.Controls.Add(_currentPage)
        _currentPage.BringToFront()
    End Sub

    Public Sub GoBackPage()
        If _history.Count = 0 Then Exit Sub

        If _currentPage IsNot Nothing Then
            _hostPanel.Controls.Remove(_currentPage)
        End If

        _currentPage = _history.Pop()

        With _currentPage
            .Dock = DockStyle.Fill
            .Visible = True
        End With

        _hostPanel.Controls.Clear()
        _hostPanel.Controls.Add(_currentPage)
        _currentPage.BringToFront()
    End Sub

    Public Sub ClearHistory()
        _history.Clear()
    End Sub

End Class

'```

'Example usage : 

'```vbnet
'Private nav As NavigationManager

'Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
'    nav = New NavigationManager(mainPanel)

'    nav.GoToPage(New root())
'End Sub
'```

'Navigate to another page:

'```vbnet
'nav.GoToPage(New SettingsPage())
'```

'Go back : 

'```vbnet
'nav.GoBackPage()
'```

'Replace current page without storing history:

'```vbnet
'nav.ReplacePage(New DashboardPage())
'```
