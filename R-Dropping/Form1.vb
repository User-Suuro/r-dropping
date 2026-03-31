Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.WindowState = FormWindowState.Maximized
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.Icon = My.Resources.Resource1.logo
        Me.MinimumSize = New Size(1024, 768)
        Me.Text = ""
    End Sub

    Private Sub CreateAnimationBG()

    End Sub
End Class
