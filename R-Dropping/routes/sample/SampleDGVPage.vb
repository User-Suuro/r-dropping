Public Class SampleDGVPage
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


        ' Search Button
        AddHandler _dgv.SearchButton.Click, Sub(sender, e)
                                                Dim searchText = _dgv.GetSearchText()

                                                FilterData(searchText)
                                            End Sub

        ' Add Button
        AddHandler _dgv.AddButton.Click, Sub(sender, e)
                                             MsgBox("Add functionality - Open a form to add new item", MsgBoxStyle.Information)
                                         End Sub

        ' Update Button (Only visible when row is selected)
        AddHandler _dgv.UpdateButton.Click, Sub(sender, e)
                                                Dim selectedRow = _dgv.GetSelectedRow()
                                                If selectedRow IsNot Nothing Then
                                                    MsgBox("Update functionality - Selected: " & selectedRow.Cells(0).Value, MsgBoxStyle.Information)
                                                End If
                                            End Sub

        ' Delete Button (Only visible when row is selected)
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

    Private Sub FilterData(searchText As String)
        If _dgv.DataGridView.DataSource Is Nothing Then Exit Sub

        Dim dt As DataTable = DirectCast(_dgv.DataGridView.DataSource, DataTable)

        If String.IsNullOrEmpty(searchText) Then
            dt.DefaultView.RowFilter = ""
        Else
            ' Search in Name and Email columns
            dt.DefaultView.RowFilter = String.Format("Name LIKE '%{0}%' OR Email LIKE '%{0}%'", searchText)
        End If
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
        dt.Rows.Add(1, "John Doe", "john@example.com", "Active", "2024-01-15")
        dt.Rows.Add(2, "Jane Smith", "jane@example.com", "Inactive", "2024-01-18")
        dt.Rows.Add(3, "Bob Johnson", "bob@example.com", "Active", "2024-01-20")
        dt.Rows.Add(4, "Alice Brown", "alice@example.com", "Active", "2024-01-22")
        dt.Rows.Add(5, "Charlie Wilson", "charlie@example.com", "Inactive", "2024-01-25")
        dt.Rows.Add(6, "Diana Martinez", "diana@example.com", "Active", "2024-01-28")
        dt.Rows.Add(7, "Edward Taylor", "edward@example.com", "Active", "2024-02-01")
        dt.Rows.Add(8, "Fiona Anderson", "fiona@example.com", "Active", "2024-02-03")

        dt.Rows.Add(1, "John Doe", "john@example.com", "Active", "2024-01-15")
        dt.Rows.Add(2, "Jane Smith", "jane@example.com", "Inactive", "2024-01-18")
        dt.Rows.Add(3, "Bob Johnson", "bob@example.com", "Active", "2024-01-20")
        dt.Rows.Add(4, "Alice Brown", "alice@example.com", "Active", "2024-01-22")
        dt.Rows.Add(5, "Charlie Wilson", "charlie@example.com", "Inactive", "2024-01-25")
        dt.Rows.Add(6, "Diana Martinez", "diana@example.com", "Active", "2024-01-28")
        dt.Rows.Add(7, "Edward Taylor", "edward@example.com", "Active", "2024-02-01")
        dt.Rows.Add(8, "Fiona Anderson", "fiona@example.com", "Active", "2024-02-03")

        dt.Rows.Add(1, "John Doe", "john@example.com", "Active", "2024-01-15")
        dt.Rows.Add(2, "Jane Smith", "jane@example.com", "Inactive", "2024-01-18")
        dt.Rows.Add(3, "Bob Johnson", "bob@example.com", "Active", "2024-01-20")
        dt.Rows.Add(4, "Alice Brown", "alice@example.com", "Active", "2024-01-22")
        dt.Rows.Add(5, "Charlie Wilson", "charlie@example.com", "Inactive", "2024-01-25")
        dt.Rows.Add(6, "Diana Martinez", "diana@example.com", "Active", "2024-01-28")
        dt.Rows.Add(7, "Edward Taylor", "edward@example.com", "Active", "2024-02-01")
        dt.Rows.Add(8, "Fiona Anderson", "fiona@example.com", "Active", "2024-02-03")

        ' Bind to DataGridView
        _dgv.BindDataSource(dt)
    End Sub

End Class
