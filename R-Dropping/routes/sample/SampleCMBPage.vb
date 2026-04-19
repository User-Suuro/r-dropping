Public Class SampleComboPage
    Inherits BasePanel

    Private _cbxCountry As SearchableComboBox
    Private _cbxRole As SearchableComboBox
    Private _cbxStatus As SearchableComboBox

    Public Sub New()
        Me.Dock = DockStyle.Fill
        InitializeComponent()
        SetupEventHandlers()
        LoadData()
    End Sub

    ' ── 1. Create and place controls ────────────────────
    Private Sub InitializeComponent()
        _cbxCountry = New SearchableComboBox With {
            .Location = New Point(40, 100),
            .Size = New Size(200, 30),
            .Placeholder = "Select a country..."
        }

        _cbxRole = New SearchableComboBox With {
            .Location = New Point(40, 160),
            .Size = New Size(300, 40),
            .Placeholder = "Select a role..."
        }

        _cbxStatus = New SearchableComboBox With {
            .Location = New Point(40, 220),
            .Size = New Size(300, 40),
            .Placeholder = "Select a status..."
        }

        Me.Controls.AddRange({_cbxCountry, _cbxRole, _cbxStatus})
    End Sub

    ' ── 2. Wire up events ───────────────────────────────
    Private Sub SetupEventHandlers()

        ' Country changed
        AddHandler _cbxCountry.SelectedValueChanged, Sub(sender, e)
                                                         Dim selected = _cbxCountry.SelectedValue
                                                         MsgBox("Country selected: " & selected)
                                                     End Sub

        ' Role changed
        AddHandler _cbxRole.SelectedValueChanged, Sub(sender, e)
                                                      Dim selected = _cbxRole.SelectedValue
                                                      MsgBox("Role selected: " & selected)
                                                  End Sub

        ' Status changed
        AddHandler _cbxStatus.SelectedValueChanged, Sub(sender, e)
                                                        Dim selected = _cbxStatus.SelectedValue
                                                        MsgBox("Status selected: " & selected)
                                                    End Sub
    End Sub

    ' ── 3. Load data ────────────────────────────────────
    Private Sub LoadData()
        _cbxCountry.Items = New List(Of String) From {
            "Philippines", "United States", "United Kingdom",
            "Canada", "Australia", "Japan", "Singapore", "Germany"
        }

        _cbxRole.Items = New List(Of String) From {
            "Administrator", "Developer", "Designer",
            "Project Manager", "QA Engineer", "DevOps", "Analyst"
        }

        _cbxStatus.Items = New List(Of String) From {
            "Active", "Inactive", "Pending", "Suspended", "Archived"
        }
    End Sub

    ' ── Helpers (optional) ──────────────────────────────
    Public Sub ClearAll()
        _cbxCountry.ClearSelection()
        _cbxRole.ClearSelection()
        _cbxStatus.ClearSelection()
    End Sub

    Public Function GetSelectedValues() As Dictionary(Of String, String)
        Return New Dictionary(Of String, String) From {
            {"Country", _cbxCountry.SelectedValue},
            {"Role", _cbxRole.SelectedValue},
            {"Status", _cbxStatus.SelectedValue}
        }
    End Function

End Class