Imports Guna.UI2.WinForms

Public Class BaseInputPanel
    Inherits Panel

    Private lblTitle As BaseLabel
    Private txtInput As Guna2TextBox
    Private inputContainer As Panel
    Private lblError As BaseLabel

    Private _validator As InputValidator = Nothing

    Public Sub New()
        MyBase.New()

        Me.DoubleBuffered = True
        Me.BackColor = Color.Transparent
        Me.Height = 80

        InitializeComponents()
        AttachEvents()
    End Sub

    Private Sub InitializeComponents()

        ' Label 
        lblTitle = New BaseLabel()
        lblTitle.SetSmall()
        lblTitle.Dock = DockStyle.Top
        lblTitle.Padding = New Padding(0, 0, 0, 4)

        ' Error Label (Bottom) 

        Dim errorContainer As New Panel()
        errorContainer.Dock = DockStyle.Bottom
        errorContainer.Height = 22

        lblError = New BaseLabel()
        lblError.SetXS()
        lblError.SetMuted()
        lblError.ForeColor = Color.Red
        lblError.Dock = DockStyle.Right
        lblError.Visible = False
        lblError.Padding = New Padding(0, 2, 0, 0)

        errorContainer.Controls.Add(lblError)

        ' Input Container 
        inputContainer = New Panel()
        inputContainer.Dock = DockStyle.Fill

        ' Guna Input 
        txtInput = New Guna2TextBox()
        txtInput.Dock = DockStyle.Top

        txtInput.BorderRadius = 4
        txtInput.PlaceholderForeColor = Colors.LblMuted

        inputContainer.Controls.Add(txtInput)

        Me.Controls.Add(errorContainer)
        Me.Controls.Add(inputContainer)
        Me.Controls.Add(lblTitle)

    End Sub

    Private Sub AttachEvents()
        AddHandler txtInput.TextChanged, Sub()
                                             If _validator IsNot Nothing Then
                                                 ValidateInput()
                                             End If
                                         End Sub
    End Sub

    ' ===== VALIDATION =====

    Public Sub SetValidator(validator As InputValidator)
        _validator = validator
    End Sub

    Public Function ValidateInput() As Boolean
        If _validator Is Nothing Then
            ClearError()
            Return True
        End If

        Dim result = _validator.Validate(txtInput.Text)

        If Not result.IsValid Then
            ShowError(result.ErrorMessage)
            Return False
        Else
            ClearError()
            Return True
        End If
    End Function

    Private Sub ShowError(message As String)
        lblError.Text = message
        lblError.Visible = True
        txtInput.BorderColor = Color.Red
    End Sub

    Private Sub ClearError()
        lblError.Visible = False
        txtInput.BorderColor = Color.Gray
    End Sub

    ' ===== PUBLIC API =====

    Public Property LabelText As String
        Get
            Return lblTitle.Text
        End Get
        Set(value As String)
            lblTitle.Text = value
        End Set
    End Property

    Public Property InputText As String
        Get
            Return txtInput.Text
        End Get
        Set(value As String)
            txtInput.Text = value
        End Set
    End Property

    Public Property Placeholder As String
        Get
            Return txtInput.PlaceholderText
        End Get
        Set(value As String)
            txtInput.PlaceholderText = value
        End Set
    End Property

    Public ReadOnly Property InputControl As Guna2TextBox
        Get
            Return txtInput
        End Get
    End Property

End Class



Public Class InputValidator

    Private ReadOnly _rules As New List(Of Func(Of String, ValidationResult))

    ' ===== STRING RULES =====

    Public Function Required() As InputValidator
        _rules.Add(Function(value)
                       If String.IsNullOrWhiteSpace(value) Then
                           Return New ValidationResult(False, "This field is required")
                       End If
                       Return New ValidationResult(True, "")
                   End Function)
        Return Me
    End Function

    Public Function MinLength(length As Integer) As InputValidator
        _rules.Add(Function(value)
                       If value Is Nothing OrElse value.Length < length Then
                           Return New ValidationResult(False, $"Minimum length is {length}")
                       End If
                       Return New ValidationResult(True, "")
                   End Function)
        Return Me
    End Function

    Public Function MaxLength(length As Integer) As InputValidator
        _rules.Add(Function(value)
                       If value IsNot Nothing AndAlso value.Length > length Then
                           Return New ValidationResult(False, $"Maximum length is {length}")
                       End If
                       Return New ValidationResult(True, "")
                   End Function)
        Return Me
    End Function

    ' ===== NUMBER RULES =====

    Public Function IsNumber() As InputValidator
        _rules.Add(Function(value)
                       Dim num As Double
                       If Not Double.TryParse(value, num) Then
                           Return New ValidationResult(False, "Must be a valid number")
                       End If
                       Return New ValidationResult(True, "")
                   End Function)
        Return Me
    End Function

    Public Function MinValue(min As Double) As InputValidator
        _rules.Add(Function(value)
                       Dim num As Double
                       If Not Double.TryParse(value, num) Then
                           Return New ValidationResult(False, "Must be a valid number")
                       End If

                       If num < min Then
                           Return New ValidationResult(False, $"Minimum value is {min}")
                       End If

                       Return New ValidationResult(True, "")
                   End Function)
        Return Me
    End Function

    Public Function MaxValue(max As Double) As InputValidator
        _rules.Add(Function(value)
                       Dim num As Double
                       If Not Double.TryParse(value, num) Then
                           Return New ValidationResult(False, "Must be a valid number")
                       End If

                       If num > max Then
                           Return New ValidationResult(False, $"Maximum value is {max}")
                       End If

                       Return New ValidationResult(True, "")
                   End Function)
        Return Me
    End Function

    ' ===== EXECUTION =====

    Public Function Validate(value As String) As ValidationResult
        For Each rule In _rules
            Dim result = rule.Invoke(value)
            If Not result.IsValid Then
                Return result
            End If
        Next

        Return New ValidationResult(True, "")
    End Function

End Class

Public Class ValidationResult
    Public Property IsValid As Boolean
    Public Property ErrorMessage As String

    Public Sub New(valid As Boolean, message As String)
        IsValid = valid
        ErrorMessage = message
    End Sub
End Class
