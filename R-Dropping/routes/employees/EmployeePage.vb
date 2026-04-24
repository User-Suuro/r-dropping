Imports System.Security.Cryptography
Imports MySql.Data.MySqlClient

Public Class EmployeePage
    Inherits BasePanel

    Private _dgv As BaseDGV

    Public Sub New()
        Me.Dock = DockStyle.Fill
        InitializeComponent()
        SetupEventHandlers()
        FetchEmployeesData()
    End Sub

    Private Sub InitializeComponent()
        _dgv = New BaseDGV()
        Me.Controls.Add(_dgv)
    End Sub

    Private Sub SetupEventHandlers()


        AddHandler _dgv.SearchButton.Click, Sub(sender, e)
                                                Dim searchText = _dgv.GetSearchText()
                                                Dim filterQuery = $"{EmployeeTable.first_name} LIKE '%{searchText}%' OR {EmployeeTable.last_name} LIKE '%{searchText}%'"
                                                _dgv.FilterData(searchText, filterQuery)
                                                HandleCol()
                                            End Sub


        AddHandler _dgv.AddButton.Click, Sub(sender, e)
                                             root.rootNav.GoToPage(New EmployeeForm())
                                         End Sub

        AddHandler _dgv.UpdateButton.Click, Sub(sender, e)
                                                Dim selectedRow = _dgv.GetSelectedRow()
                                                root.rootNav.GoToPage(New EmployeeForm(selectedRow.Cells(0).Value))
                                            End Sub

        AddHandler _dgv.DeleteButton.Click, Sub(sender, e)
                                                Dim selectedRow = _dgv.GetSelectedRow()
                                                DeleteEmployees(selectedRow.Cells(0).Value)
                                            End Sub

        AddHandler _dgv.RefreshButton.Click, Sub(sender, e)
                                                 FetchEmployeesData()
                                             End Sub
    End Sub

    Private Async Sub FetchEmployeesData()

        Dim loadingDlg As New BaseDialog()

        Await DialogTypes.ShowLoadingUntilAsync(
            loadingDlg,
            Form1.Instance,
            Async Function()
                Dim sql As String = $"SELECT * FROM {EmployeeTable.table_name}"

                Dim reader As MySqlDataReader = Await ReadQueryAsync(sql)

                If reader IsNot Nothing Then
                    Dim dt As New DataTable()
                    dt.Load(reader)
                    reader.Close()

                    _dgv.BindDataSource(dt)
                End If

                HandleCol()
            End Function
        )
    End Sub

    Private Sub HandleCol()
        With _dgv.DataGridView.Columns
            .Item(EmployeeTable.id).Visible = False
        End With
    End Sub

    Public Async Sub DeleteEmployees(id As Integer)
        Dim confirm_dlg = New BaseDialog()

        DialogTypes.Apply(confirm_dlg,
                 DialogType.Confirmation,
                 "Confirmation",
                 "Are you sure you want to delete this employee?")

        If Await confirm_dlg.ShowBaseDialogAsync(Form1.Instance) = DialogResultType.Confirm Then
            Dim sql As String = $"DELETE FROM {EmployeeTable.table_name} 
                                WHERE {EmployeeTable.id} = @{EmployeeTable.id}"


            Dim params As New Dictionary(Of String, Object) From {
                {$"@{EmployeeTable.id}", id}
            }

            Dim affectedRows As Integer = Await ExecuteQueryAsync(sql, params)

            If affectedRows > 0 Then

                Dim info_dlg = New BaseDialog()

                DialogTypes.Apply(info_dlg,
                          DialogType.Info,
                          "Success",
                          "Employee data was deleted successfully")

                Await info_dlg.ShowBaseDialogAsync(Form1.Instance)

                FetchEmployeesData()

            End If
        End If

        ' Handle error

    End Sub

End Class
