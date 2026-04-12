
Public Class BasePanel
    Inherits Panel

    Public Sub New()
        Me.DoubleBuffered = True
        Me.ResizeRedraw = True
        Me.BackColor = Colors.Background
    End Sub

    Protected Overrides Sub OnPaint(e As PaintEventArgs)
        MyBase.OnPaint(e)
        Me.BackColor = Colors.Background
    End Sub
End Class

Public Class PrimaryPanel
    Inherits BasePanel

    Public Sub New()
        MyBase.New()
        Me.BackColor = Colors.Primary
        Me.ForeColor = Colors.OnPrimary
    End Sub
End Class

Public Class SecondaryPanel
    Inherits BasePanel

    Public Sub New()
        MyBase.New()
        Me.BackColor = Colors.Secondary
        Me.ForeColor = Colors.OnSecondary
    End Sub
End Class

Public Class OverlayPanel
    Public Shared Function CreateOverlay(Optional alpha As Integer = 120) As Panel
        Dim overlay As New DoubleBufferedPanel()

        overlay.Dock = DockStyle.Fill
        overlay.BackColor = Color.FromArgb(alpha, 0, 0, 0)

        Return overlay
    End Function

End Class

Public Class DoubleBufferedPanel
    Inherits Panel

    Public Sub New()
        Me.DoubleBuffered = True
        Me.SetStyle(ControlStyles.AllPaintingInWmPaint Or
                    ControlStyles.UserPaint Or
                    ControlStyles.OptimizedDoubleBuffer, True)
        Me.UpdateStyles()
    End Sub

End Class