Imports Guna.UI2.WinForms

Public Class BaseButton
    Inherits Guna2Button

    Public Sub New()
        MyBase.New()

        InitializeBaseStyle()
    End Sub

    Private Sub InitializeBaseStyle()

        ' Size / Layout 

        Me.Height = 36
        Me.Dock = DockStyle.Top
        Me.BorderRadius = 6

        ' Colors (Default / Primary) 
        Me.FillColor = Colors.Primary
        Me.ForeColor = Colors.OnPrimary

        ' Font 
        Me.Font = New Font(Strings.FONT_FAMILY, Dimen.LABEL_SM, FontStyle.Regular)

        Me.Cursor = Cursors.Hand
        Me.BorderThickness = 0

    End Sub

    ' ===== VARIANTS =====

    Public Sub SetPrimary()
        Me.FillColor = Colors.BtnPrimary
        Me.ForeColor = Colors.OnBtnPrimary

        Me.HoverState.FillColor = ControlPaint.Light(Colors.BtnPrimary)
        Me.PressedColor = ControlPaint.Dark(Colors.BtnPrimary)
    End Sub

    Public Sub SetSecondary()
        Me.FillColor = Colors.BtnSecondary
        Me.ForeColor = Colors.OnBtnSecondary

        Me.HoverState.FillColor = ControlPaint.Light(Colors.BtnSecondary)
        Me.PressedColor = ControlPaint.Dark(Colors.BtnSecondary)
    End Sub

    Public Sub SetDanger()
        Me.FillColor = Color.FromArgb(220, 53, 69)
        Me.ForeColor = Color.White

        Me.HoverState.FillColor = ControlPaint.Light(Me.FillColor)
        Me.PressedColor = ControlPaint.Dark(Me.FillColor)
    End Sub

End Class