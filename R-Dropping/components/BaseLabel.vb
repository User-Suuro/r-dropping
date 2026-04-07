Public Class BaseLabel
    Inherits Label

    Public Sub New()
        MyBase.New()

        Me.AutoSize = True
        Me.ForeColor = Colors.LblPrimary
        Me.BackColor = Color.Transparent

        Me.Font = New Font(Strings.FONT_FAMILY, Dimen.LABEL_MD, FontStyle.Regular)

    End Sub

    Public Sub SetXS()
        Me.Font = New Font(Strings.FONT_FAMILY, Dimen.LABEL_XS, FontStyle.Regular)
    End Sub

    Public Sub SetSmall()
        Me.Font = New Font(Strings.FONT_FAMILY, Dimen.LABEL_SM, FontStyle.Regular)
    End Sub

    Public Sub SetMedium()
        Me.Font = New Font(Strings.FONT_FAMILY, Dimen.LABEL_MD, FontStyle.Regular)
    End Sub

    Public Sub SetLarge()
        Me.Font = New Font(Strings.FONT_FAMILY, Dimen.LABEL_LG, FontStyle.Bold)
    End Sub


    Public Sub SetPrimary()
        Me.ForeColor = Colors.LblPrimary
    End Sub

    Public Sub SetSecondary()
        Me.ForeColor = Colors.LblSecondary
    End Sub

    Public Sub SetMuted()
        Me.ForeColor = Colors.LblMuted
    End Sub
End Class