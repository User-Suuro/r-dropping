Public Class EmployeesPage
    Inherits BasePanel

    Private _dgv As BaseDGV

    Public Sub New()
        Me.Dock = DockStyle.Fill
        InitializeComponent()
        SetupEventHandlers()
        LoadDummyData()
    End Sub

    Private Sub InitializeComponent()
        _dgv = New BaseDGV()
        Me.Controls.Add(_dgv)
    End Sub

    Private Sub SetupEventHandlers()


        AddHandler _dgv.SearchButton.Click, Sub(sender, e)
                                                Dim searchText = _dgv.GetSearchText()
                                                _dgv.FilterData(searchText, "Name LIKE '%{0}%' OR Email LIKE '%{0}%'")
                                            End Sub


        AddHandler _dgv.AddButton.Click, Sub(sender, e)
                                             root.rootNav.GoToPage(New EmployeeForm())
                                         End Sub

        AddHandler _dgv.UpdateButton.Click, Sub(sender, e)
                                                Dim selectedRow = _dgv.GetSelectedRow()
                                                If selectedRow IsNot Nothing Then
                                                    MsgBox("Update functionality - Selected: " & selectedRow.Cells(0).Value, MsgBoxStyle.Information)
                                                End If
                                            End Sub


        AddHandler _dgv.DeleteButton.Click, Sub(sender, e)
                                                Dim selectedRow = _dgv.GetSelectedRow()
                                                If selectedRow IsNot Nothing Then
                                                    MsgBox("Delete functionality - Deleting: " & selectedRow.Cells(0).Value, MsgBoxStyle.Information)
                                                End If
                                            End Sub

        AddHandler _dgv.RefreshButton.Click, Sub(sender, e)
                                                 MsgBox("Refresh functionality - Reloading data", MsgBoxStyle.Information)
                                                 LoadDummyData()
                                             End Sub
    End Sub

    Private Sub FetchEmployeesData()

    End Sub

    Private Sub DeleteEmployees()

    End Sub


    Private Sub LoadDummyData()
        ' Create DataTable
        Dim dt As New DataTable()

        dt.Columns.Add("ID", GetType(Integer))
        dt.Columns.Add("Name", GetType(String))
        dt.Columns.Add("Email", GetType(String))
        dt.Columns.Add("Status", GetType(String))
        dt.Columns.Add("Date", GetType(String))
        ' Add dummy data rows


        'With _dgv.DataGridView.Columns
        '    .Item("ID").Visible = False
        'End With

        ' Bind to DataGridView
        _dgv.BindDataSource(dt)
    End Sub
End Class
