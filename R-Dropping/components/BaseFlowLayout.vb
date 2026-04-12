
Public Class BaseFlowLayoutPanel
    Inherits FlowLayoutPanel

    Public Sub New()
        Me.DoubleBuffered = True
        Me.ResizeRedraw = True

        Me.WrapContents = False
        Me.AutoScroll = True

        Me.BackColor = Colors.Background
    End Sub

End Class

Public Class PrimaryFlowLayoutPanel
    Inherits BaseFlowLayoutPanel

    Public Sub New()
        MyBase.New()

        Me.BackColor = Colors.Primary
        Me.ForeColor = Colors.OnPrimary
    End Sub
End Class

Public Class SecondaryFlowLayoutPanel
    Inherits BaseFlowLayoutPanel

    Public Sub New()
        MyBase.New()

        Me.BackColor = Colors.Secondary
        Me.ForeColor = Colors.OnSecondary
    End Sub

End Class

Public Class DoubleBufferedFlowLayoutPanel
    Inherits FlowLayoutPanel

    Public Sub New()
        Me.SetStyle(ControlStyles.AllPaintingInWmPaint Or
                        ControlStyles.UserPaint Or
                        ControlStyles.OptimizedDoubleBuffer, True)

        Me.UpdateStyles()
    End Sub

End Class
