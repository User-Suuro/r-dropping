Public Class SampleComboPage
    Inherits BasePanel

    Private _cbxCountry As BaseComboBox


    Private _cbxCountryField As ValidationPanel

    Public Sub New()
        Me.Dock = DockStyle.Fill
        InitializeComponent()
        SetupEventHandlers()
        LoadData()
    End Sub

    ' ── 1. Create and place controls ────────────────────
    Private Sub InitializeComponent()
        _cbxCountry = New BaseComboBox("Country") With {
            .Placeholder = "Select a country...",
            .SearchEnabled = False,
            .Size = New Size(200, 30)
        }

        _cbxCountryField = New ValidationPanel(_cbxCountry) With {
            .Location = New Point(40, 100)
        }

        _cbxCountryField.SetValidator(New InputValidator().Required())

        Dim allValid = {_cbxCountryField}.All(Function(f) f.ValidateInput())


        Me.Controls.AddRange({_cbxCountryField})
    End Sub

    ' ── 2. Wire up events ───────────────────────────────
    Private Sub SetupEventHandlers()

        ' Country changed
        AddHandler _cbxCountry.SelectedValueChanged, Sub(sender, e)
                                                         Dim selected = _cbxCountry.SelectedValue
                                                         MsgBox("Country selected: " & selected)
                                                     End Sub

        ' Role changed

    End Sub

    ' ── 3. Load data ────────────────────────────────────
    Private Sub LoadData()
        _cbxCountry.Items = New List(Of String) From {
            "Philippines", "United States", "United Kingdom",
            "Canada", "Australia", "Japan", "Singapore", "Germany"
        }


    End Sub

    ' ── Helpers (optional) ──────────────────────────────
    Public Sub ClearAll()
        _cbxCountry.ClearSelection()

    End Sub

    Public Function GetSelectedValues() As Dictionary(Of String, String)
        Return New Dictionary(Of String, String) From {
            {"Country", _cbxCountry.SelectedValue}
        }
    End Function

End Class