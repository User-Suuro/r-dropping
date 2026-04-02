Public Class Form1

    Public mainPanel As New PrimaryPanel()
    Private loginPanel As New PrimaryPanel()

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        mainPanel.Dock = DockStyle.Fill

        With Me
            .WindowState = FormWindowState.Maximized
            .StartPosition = FormStartPosition.CenterScreen
            .Icon = My.Resources.Resource1.logo
            .MinimumSize = New Size(1024, 768)
            .Text = Strings.application_title
            .Controls.Add(mainPanel)
            Themes.ApplyLightTheme()
        End With

        ' BG ANIMATION
        Dim bg As New PictureBox()

        With bg
            .Dock = DockStyle.Fill
            .SizeMode = PictureBoxSizeMode.StretchImage
            .Image = My.Resources.Resource1.login_animation
        End With

        mainPanel.Controls.Add(bg)
        Dim overlay As Panel = OverlayHelper.CreateOverlay(120)
        bg.Controls.Add(overlay)


    End Sub



End Class