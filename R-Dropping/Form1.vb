Imports System.Windows
Imports Mysqlx.XDevAPI.Common

Public Class Form1

    Public mainPanel As New PrimaryPanel()

    ' CONFIG ELEMENTS

    Private configContainerPanel As New PrimaryPanel()
    Private configSubPanel As New PrimaryFlowLayoutPanel()

    Private serverInput As New BaseInputPanel()
    Private uidInput As New BaseInputPanel()
    Private pwdInput As New BaseInputPanel()
    Private dbNameInput As New BaseInputPanel()
    Private dbPortInput As New BaseInputPanel()

    Private btnSubmit As New BaseButton()

    Private configVal As New DbConfig()

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

        ' Config Panel
        mainPanel.Controls.Add(bg)

        ' Overlay
        Dim overlay As Panel = OverlayPanel.CreateOverlay()
        overlay.Dock = DockStyle.Fill
        overlay.BringToFront()
        bg.Controls.Add(overlay)

        With configContainerPanel
            .AutoSize = True
            .AutoSizeMode = AutoSizeMode.GrowAndShrink
        End With

        overlay.Controls.Add(configContainerPanel)

        With configSubPanel
            .Padding = New Padding(16)
            .FlowDirection = FlowDirection.TopDown
            .AutoSize = True
            .AutoSizeMode = AutoSizeMode.GrowAndShrink
        End With

        configContainerPanel.Controls.Add(configSubPanel)

        renderConfigPanelContent()

        LayoutHelper.CenterBoth(configContainerPanel)
        LayoutHelper.EnableAutoCenter(configContainerPanel)

        ' Initialize Config
        ConfigManager.EnsureConfigExists(Of DbConfig)()

    End Sub


    Private Sub renderConfigPanelContent()

        configVal = ConfigManager.Load(Of DbConfig)()

        With serverInput
            .LabelText = Strings.SERVER_LBL
            .InputControl.PlaceholderText = Strings.SERVER_PLACEHOLDER
            .InputControl.Text = configVal.DB_SERVER
            .SetValidator(
            New InputValidator().Required())
        End With

        With uidInput
            .LabelText = Strings.UID_LBL
            .InputControl.PlaceholderText = Strings.UID_PLACEHOLDER
            .InputControl.Text = configVal.DB_UID
            .SetValidator(
            New InputValidator().Required())
        End With

        With pwdInput
            .LabelText = Strings.DB_PASS_LBL
            .InputControl.PlaceholderText = Strings.DB_PASS_PLACEHOLDER
            .InputControl.UseSystemPasswordChar = True
            .InputControl.Text = configVal.DB_PWD
            .SetValidator(
            New InputValidator().Required())
        End With

        With dbNameInput
            .LabelText = Strings.DB_NAME
            .InputControl.PlaceholderText = Strings.DB_NAME_PLACEHOLDER
            .InputControl.Text = configVal.DB_NAME
            .SetValidator(
            New InputValidator().Required())
        End With

        With dbPortInput
            .LabelText = Strings.DB_PORT
            .InputControl.PlaceholderText = Strings.DB_PORT_PLACEHOLDER
            .InputControl.Text = configVal.DB_PORT
            .SetValidator(
            New InputValidator().Required())
        End With

        With btnSubmit
            .Text = Strings.BTN_CONNECT
            .SetPrimary()
        End With

        With configSubPanel.Controls
            .Add(serverInput)
            .Add(dbPortInput)
            .Add(uidInput)
            .Add(pwdInput)
            .Add(dbNameInput)
            .Add(btnSubmit)
        End With


        AddHandler btnSubmit.Click, AddressOf SaveConfig
    End Sub

    Private Sub SaveConfig()

        If Not ValidateAllInputs() Then
            Exit Sub
        End If

        Try
            With configVal
                .DB_PORT = dbPortInput.InputControl.Text
                .DB_UID = uidInput.InputControl.Text
                .DB_SERVER = serverInput.InputControl.Text
                .DB_PWD = pwdInput.InputControl.Text
                .DB_NAME = dbNameInput.InputControl.Text
            End With

            ConfigManager.Save(configVal)

            MessageBox.Show("success")
        Catch ex As Exception

            MessageBox.Show(ex.Message)
        End Try



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

