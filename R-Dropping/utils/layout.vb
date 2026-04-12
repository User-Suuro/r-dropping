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

        CenterBoth(ctrl)
    End Sub

End Class