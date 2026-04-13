
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

        ' main container

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
        Dim bg As New PictureBox()

        With bg
            .Dock = DockStyle.Fill
            .SizeMode = PictureBoxSizeMode.StretchImage
            .Image = My.Resources.Resource1.login_animation
        End With


        mainPanel.Controls.Add(bg)

        ' Overlay
        Dim overlay As Panel = OverlayPanel.CreateOverlay()

        overlay.Dock = DockStyle.Fill
        bg.Controls.Add(overlay)

        ' Config Panel

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

        Dim overlay As Panel = OverlayPanel.CreateOverlay()

        overlay.Dock = DockStyle.Fill


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



' ===== DIALOG COMPONENT  =====



Public Class BaseDialog
    Inherits Form

    Private overlay As BaseDialogOverlay
    Private container As Panel

    Public Sub New(targetPanel As Control)
        FormBorderStyle = FormBorderStyle.None
        StartPosition = FormStartPosition.Manual
        ShowInTaskbar = False
        BackColor = Color.White
        Size = New Size(400, 200)

        container = New Panel With {
            .Dock = DockStyle.Fill,
            .BackColor = Color.White
        }

        Controls.Add(container)

        overlay = New BaseDialogOverlay(targetPanel)
    End Sub

    Public Sub ShowDialogBounded()
        overlay.Show()

        AddHandler overlay.BoundsChanged, AddressOf SyncDialogPosition

        Me.Owner = overlay
        Me.Show()

        SyncDialogPosition(Nothing, EventArgs.Empty)
    End Sub

    Protected Overrides Sub OnFormClosed(e As FormClosedEventArgs)
        MyBase.OnFormClosed(e)
        If overlay IsNot Nothing AndAlso Not overlay.IsDisposed Then
            overlay.Close()
        End If
    End Sub

    Protected Overrides Sub OnDeactivate(e As EventArgs)
        MyBase.OnDeactivate(e)

        If Me.Visible Then
            Me.Focus()
            Me.BringToFront()
        End If
    End Sub

    Private Sub SyncDialogPosition(sender As Object, e As EventArgs)
        If overlay Is Nothing OrElse overlay.IsDisposed Then Return

        Dim center As Point = New Point(
        overlay.Left + (overlay.Width - Me.Width) \ 2,
        overlay.Top + (overlay.Height - Me.Height) \ 2
    )

        Me.Location = center
    End Sub

End Class

Public Class BaseDialogOverlay
    Inherits Form

    Private ReadOnly _targetPanel As Control
    Private ReadOnly _parentForm As Form
    Public Event BoundsChanged As EventHandler

    Public Sub New(targetPanel As Control)
        _targetPanel = targetPanel
        _parentForm = targetPanel.FindForm()

        ' FORM CONFIG
        FormBorderStyle = FormBorderStyle.None
        ShowInTaskbar = False
        TopMost = False
        StartPosition = FormStartPosition.Manual

        BackColor = Color.Black
        Opacity = 0.45R

        If _parentForm IsNot Nothing Then
            Owner = _parentForm
        End If

        AddHandler _targetPanel.SizeChanged, AddressOf SyncBounds
        AddHandler _targetPanel.LocationChanged, AddressOf SyncBounds

        If _parentForm IsNot Nothing Then
            AddHandler _parentForm.LocationChanged, AddressOf SyncBounds
            AddHandler _parentForm.SizeChanged, AddressOf SyncBounds
        End If
    End Sub

    Protected Overrides Sub OnShown(e As EventArgs)
        MyBase.OnShown(e)
        SyncBounds(Nothing, EventArgs.Empty)
    End Sub

    Private Sub SyncBounds(sender As Object, e As EventArgs)
        If _targetPanel Is Nothing OrElse _targetPanel.IsDisposed Then Return

        Dim rect As Rectangle = _targetPanel.RectangleToScreen(_targetPanel.ClientRectangle)
        Bounds = rect

        RaiseEvent BoundsChanged(Me, EventArgs.Empty)
    End Sub

End Class