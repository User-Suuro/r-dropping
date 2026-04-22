
Imports MySql.Data.MySqlClient

Public Class EmployeeForm
    Inherits BasePanel

    Private _subContainer As PrimaryFlowLayoutPanel

    Private _inpFirstName As BaseInputPanel
    Private _fieldFirstName As ValidationPanel

    Private _inpLastName As BaseInputPanel
    Private _fieldLastName As ValidationPanel

    Private _inpMiddleName As BaseInputPanel

    Private _cbxPosition As BaseComboBox
    Private _cbxPosField As ValidationPanel

    Private _addButton As BaseButton
    Private _cancelButton As BaseButton

    Private _buttonTable As TableLayoutPanel

    Private _id As Integer

    Public Sub New(Optional id = Nothing)
        Me.Dock = DockStyle.Fill
        _id = id
        InitializeComponent()
        LoadData()

        AddHandler Me.Resize, AddressOf CenterSubContainer
        AddHandler _subContainer.SizeChanged, AddressOf CenterSubContainer
    End Sub

    Public Sub InitializeComponent()
        _subContainer = New PrimaryFlowLayoutPanel() With {
            .AutoSize = True,
            .FlowDirection = FlowDirection.TopDown,
            .AutoSizeMode = AutoSizeMode.GrowAndShrink,
            .BackColor = Color.White,
            .Padding = New Padding(16),
            .BorderStyle = BorderStyle.FixedSingle
        }

        ' First Name
        _inpFirstName = New BaseInputPanel() With {
            .LabelText = "First Name"
        }

        _fieldFirstName = New ValidationPanel(_inpFirstName)
        _fieldFirstName.SetValidator(New InputValidator().Required())

        ' Middle Name
        _inpMiddleName = New BaseInputPanel() With {
            .LabelText = "Middle Name (optional)"
        }

        ' Last Name
        _inpLastName = New BaseInputPanel() With {
            .LabelText = "Last Name"
        }

        _fieldLastName = New ValidationPanel(_inpLastName)
        _fieldLastName.SetValidator(New InputValidator().Required())

        ' Position
        _cbxPosition = New BaseComboBox("Position") With {
            .Placeholder = "Select position...",
            .SearchEnabled = False
        }

        _cbxPosField = New ValidationPanel(_cbxPosition)
        _cbxPosField.SetValidator(New InputValidator().Required())

        ' Button Table
        _buttonTable = New TableLayoutPanel With {
             .ColumnCount = 2,
            .Padding = Padding.Empty,
            .Margin = New Padding(0, 4, 0, 0),
            .CellBorderStyle = TableLayoutPanelCellBorderStyle.None,
            .Width = _fieldLastName.Width + 8,
            .Height = 40
        }

        _buttonTable.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50))
        _buttonTable.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50))

        _addButton = New BaseButton() With {
            .Text = "Add",
            .Dock = DockStyle.Top
        }

        _addButton.SetPrimary()

        _cancelButton = New BaseButton() With {
            .Text = "Cancel",
            .Dock = DockStyle.Top
        }

        _cancelButton.SetDanger()

        _buttonTable.Controls.Add(_cancelButton, 0, 0)
        _buttonTable.Controls.Add(_addButton, 1, 0)

        ' Handle Edit Mode
        If _id = Not Nothing Then
            handleEditMode(_id)
        End If

        ' Controls

        With _subContainer.Controls
            .Add(_fieldFirstName)
            .Add(_inpMiddleName)
            .Add(_fieldLastName)
            .Add(_cbxPosField)
            .Add(_buttonTable)
        End With

        Me.Controls.Add(_subContainer)

        ' Bind Events

        AddHandler _addButton.Click, AddressOf QueryEmployee
        AddHandler _cancelButton.Click, AddressOf CancelAdd
    End Sub

    Private Function handleEditMode(id As Integer)
        _addButton.Text = "Save"
    End Function

    Private Function ValidateAllInputs() As Boolean
        Return {_fieldFirstName, _fieldLastName, _cbxPosField}.All(Function(f) f.ValidateInput())
    End Function

    Private Async Sub QueryEmployee()

        If Not ValidateAllInputs() Then
            Exit Sub
        End If

        Dim confirm_dlg = New BaseDialog()

        DialogTypes.Apply(confirm_dlg,
                 DialogType.Confirmation,
                 "Confirmation",
                 "Are you sure you want to add this employee?")

        If Await confirm_dlg.ShowBaseDialogAsync(Form1.Instance) = DialogResultType.Confirm Then
            Dim loadingDlg As New BaseDialog()

            Dim completed As Boolean = Await DialogTypes.ShowLoadingUntilAsync(
                loadingDlg,
                Form1.Instance,
                Async Function()

                    Dim queryResult As Boolean = Await AddEmployeeQuery()
                    If queryResult Then
                        Dim info_dlg = New BaseDialog()

                        DialogTypes.Apply(info_dlg,
                          DialogType.Info,
                          "Success",
                          "Employee saved successfully")

                        Dim result_info_dlg = Await info_dlg.ShowBaseDialogAsync(Form1.Instance)

                        If result_info_dlg = DialogResultType.Confirm Then
                            root.rootNav.GoBackPage()
                        End If
                    End If
                End Function
            )
        Else
            confirm_dlg.Hide()
        End If

    End Sub

    Private Async Function AddEmployeeQuery() As Task(Of Boolean)
        Dim sql As String =
        $"INSERT INTO {EmployeeTable.table_name} " &
        $"({EmployeeTable.first_name}, {EmployeeTable.middle_name}, {EmployeeTable.last_name}, {EmployeeTable.position}) " &
        $"VALUES (@{EmployeeTable.first_name}, @{EmployeeTable.middle_name}, @{EmployeeTable.last_name}, @{EmployeeTable.position})"

        Dim params As New Dictionary(Of String, Object) From {
        {$"@{EmployeeTable.first_name}", _inpFirstName.Value},
        {$"@{EmployeeTable.middle_name}", ToDbNull(_inpMiddleName.Value)},
        {$"@{EmployeeTable.last_name}", _inpLastName.Value},
        {$"@{EmployeeTable.position}", _cbxPosition.SelectedValue}
        }

        Dim affectedRows As Integer = Await ExecuteQueryAsync(sql, params)

        If affectedRows > 0 Then
            Return True
        End If

        Return False
    End Function

    Private Sub CancelAdd()
        root.rootNav.GoBackPage()
    End Sub

    Private Sub CenterSubContainer(sender As Object, e As EventArgs)
        _subContainer.Left = (Me.ClientSize.Width - _subContainer.Width) \ 2
        _subContainer.Top = (Me.ClientSize.Height - _subContainer.Height) \ 2
    End Sub

    Private Sub LoadData()
        _cbxPosition.Items = New List(Of String) From {
          "Admin", "Manager", "Staff"
        }
    End Sub
End Class
