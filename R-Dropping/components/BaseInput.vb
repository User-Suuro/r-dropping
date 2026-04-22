Imports Guna.UI2.WinForms

Public Class BaseInputPanel
    Inherits PrimaryPanel
    Implements IValueProvider
    Implements IValidationStyleable

    Public Event ValueChanged As EventHandler Implements IValueProvider.ValueChanged

    Private _lblTitle As BaseLabel
    Private _txtInput As Guna2TextBox

    Private Const PanelHeight As Integer = 56

    Public Sub New()
        MyBase.New()
        Me.DoubleBuffered = True
        Me.Height = PanelHeight
        InitializeComponents()
        AttachEvents()
    End Sub

    ' ── Build ────────────────────────────────────────────────────

    Private Sub InitializeComponents()
        _lblTitle = New BaseLabel()
        With _lblTitle
            .SetSmall()
            .Dock = DockStyle.Top
            .Padding = New Padding(0, 0, 0, 4)
        End With

        _txtInput = New Guna2TextBox()
        With _txtInput
            .Dock = DockStyle.Top
            .BorderRadius = 4
            .PlaceholderForeColor = Colors.LblMuted
            .Height = 32
            .Font = New Font("Segoe UI", 9.0F)
            .ForeColor = Color.Black
        End With

        Me.Controls.Add(_txtInput)
        Me.Controls.Add(_lblTitle)
    End Sub

    Private Sub AttachEvents()
        AddHandler _txtInput.TextChanged,
            Sub(s As Object, e As EventArgs)
                RaiseEvent ValueChanged(Me, EventArgs.Empty)
            End Sub
    End Sub

    Public Sub SetValue(value As String)
        _txtInput.Text = value
    End Sub


    Public ReadOnly Property Value As String Implements IValueProvider.Value
        Get
            Return _txtInput.Text
        End Get
    End Property


    ' ── IValidationStyleable ──────────────────────────────────────

    Public Sub OnValidationError() Implements IValidationStyleable.OnValidationError
        _txtInput.BorderColor = Color.Red
    End Sub

    Public Sub OnValidationClear() Implements IValidationStyleable.OnValidationClear
        _txtInput.BorderColor = Color.FromArgb(213, 218, 223)
    End Sub

    ' ── Public API ───────────────────────────────────────────────

    Public Property LabelText As String
        Get
            Return _lblTitle.Text
        End Get
        Set(value As String)
            _lblTitle.Text = value
        End Set
    End Property

    Public Property PlaceholderText As String
        Get
            Return _txtInput.PlaceholderText
        End Get
        Set(value As String)
            _txtInput.PlaceholderText = value
        End Set
    End Property

    ''' <summary>Direct access to the underlying Guna textbox if needed.</summary>
    Public ReadOnly Property InputControl As Guna2TextBox
        Get
            Return _txtInput
        End Get
    End Property

End Class