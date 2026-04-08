Imports System.Windows

Public Class Form1

    Public mainPanel As New PrimaryPanel()

    ' CONFIG ELEMENTS
    Private configPanel As New PrimaryPanel()

    Private serverInput As New BaseInputPanel()
    Private uidInput As New BaseInputPanel()
    Private pwdInput As New BaseInputPanel()
    Private dbNameInput As New BaseInputPanel()

    Private overlay As New Panel()
    Private btnSubmit As New BaseButton()

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        mainPanel.Dock = DockStyle.Fill

        With Me
            .WindowState = FormWindowState.Maximized
            .StartPosition = FormStartPosition.CenterScreen
            .Icon = My.Resources.Resource1.logo
            .MinimumSize = Dimen.MIN_RES
            .Text = Strings.APP_NAME
            .Controls.Add(mainPanel)
        End With

        Themes.ApplyLightTheme()

        ' BG ANIMATION
        Dim bg As New PictureBox()

        With bg
            .Dock = DockStyle.Fill
            .SizeMode = PictureBoxSizeMode.StretchImage
            .Image = My.Resources.Resource1.login_animation
        End With

        mainPanel.Controls.Add(bg)

        ' Overlay (Background transparent black panel)
        overlay = OverlayPanel.CreateOverlay(120)
        bg.Controls.Add(overlay)

        ' Config Panel

        With configPanel
            .Size = New Size(260, 336)
            .Padding = New Padding(16)
        End With

        overlay.Controls.Add(configPanel)

        LayoutHelper.CenterBoth(configPanel)
        LayoutHelper.EnableAutoCenter(configPanel)

        renderConfigPanelContent()

    End Sub


    Private Sub renderConfigPanelContent()

        With serverInput
            .LabelText = Strings.SERVER_LBL
            .Placeholder = Strings.SERVER_PLACEHOLDER
            .Dock = DockStyle.Top
            .SetValidator(
            New InputValidator().Required())
        End With

        With uidInput
            .LabelText = Strings.UID_LBL
            .Placeholder = Strings.UID_PLACEHOLDER
            .Dock = DockStyle.Top
            .SetValidator(
            New InputValidator().Required())
        End With

        With pwdInput
            .LabelText = Strings.DB_PASS_LBL
            .Placeholder = Strings.DB_PASS_PLACEHOLDER
            .Dock = DockStyle.Top
            .IsPassword = True
            .SetValidator(
            New InputValidator().Required())
        End With

        With dbNameInput
            .LabelText = Strings.DB_NAME
            .Placeholder = Strings.DB_NAME_PLACEHOLDER
            .Dock = DockStyle.Top
            .SetValidator(
            New InputValidator().Required())
        End With

        With btnSubmit
            .Text = Strings.BTN_SUBMIT
            .SetPrimary()
        End With

        With configPanel.Controls
            .Add(btnSubmit)
            .Add(dbNameInput)
            .Add(pwdInput)
            .Add(uidInput)
            .Add(serverInput)
        End With


        AddHandler btnSubmit.Click, AddressOf SaveConfig
    End Sub

    Private Sub SaveConfig()

        If Not ValidateAllInputs() Then
            Exit Sub
        End If

    End Sub

    Private Function ValidateAllInputs() As Boolean

        Dim Inputs As New List(Of BaseInputPanel)

        Inputs.Add(serverInput)
        Inputs.Add(uidInput)
        Inputs.Add(pwdInput)
        Inputs.Add(dbNameInput)

        Return Inputs.All(Function(i) i.ValidateInput())
    End Function




End Class