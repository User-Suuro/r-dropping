
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

Public Class LayoutHelper

    ' Center horizontally
    Public Shared Sub CenterHorizontal(ctrl As Control)
        If ctrl.Parent Is Nothing Then Exit Sub

        Dim parentWidth As Integer = ctrl.Parent.ClientSize.Width
        ctrl.Left = (parentWidth - ctrl.Width) \ 2
    End Sub

    ' Center vertically
    Public Shared Sub CenterVertical(ctrl As Control)
        If ctrl.Parent Is Nothing Then Exit Sub

        Dim parentHeight As Integer = ctrl.Parent.ClientSize.Height
        ctrl.Top = (parentHeight - ctrl.Height) \ 2
    End Sub

    ' full center (both axes)
    Public Shared Sub CenterBoth(ctrl As Control)
        CenterHorizontal(ctrl)
        CenterVertical(ctrl)
    End Sub

    Public Shared Sub EnableAutoCenter(ctrl As Control)

        If ctrl.Parent Is Nothing Then Exit Sub

        Dim parent As Control = ctrl.Parent

        ' Re-center whenever the parent resizes
        AddHandler parent.Resize,
            Sub()
                CenterBoth(ctrl)
            End Sub

        ' Initial positioning
        CenterBoth(ctrl)

    End Sub

End Class