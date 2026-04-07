Public Class Form1

    Public mainPanel As New PrimaryPanel()
    Private configPanel As New PrimaryPanel()
    Private inputName As New BaseInputPanel()

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        mainPanel.Dock = DockStyle.Fill

        With Me
            .WindowState = FormWindowState.Maximized
            .StartPosition = FormStartPosition.CenterScreen
            .Icon = My.Resources.Resource1.logo
            .MinimumSize = Dimen.MIN_RES
            .Text = Strings.APP_NAME
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

        ' Overlay
        Dim overlay As Panel = OverlayPanel.CreateOverlay(120)
        bg.Controls.Add(overlay)

        ' config panel
        configPanel.Size = New Size(240, 300)
        configPanel.Padding = New Padding(12)
        overlay.Controls.Add(configPanel)

        LayoutHelper.CenterBoth(configPanel)


        inputName.LabelText = "Server"
        inputName.Placeholder = "Enter the Server Name or IP Address"
        inputName.Dock = DockStyle.Top

        inputName.SetValidator(
            New InputValidator().Required())




        Dim btnSubmit As New Guna.UI2.WinForms.Guna2Button()

        btnSubmit.Text = "Submit"
        btnSubmit.Dock = DockStyle.Top
        btnSubmit.Height = 40
        btnSubmit.BorderRadius = 6

        configPanel.Controls.Add(btnSubmit)
        configPanel.Controls.Add(inputName)

        LayoutHelper.EnableAutoCenter(configPanel)

        AddHandler btnSubmit.Click, AddressOf SaveConfig

    End Sub

    Private Sub SaveConfig()

        If Not inputName.ValidateInput() Then
            Exit Sub
        End If

    End Sub




End Class