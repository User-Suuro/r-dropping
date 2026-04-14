
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

        Dim dlg As New BaseDialog(Me.mainPanel)

        DialogTypes.Apply(dlg,
                          DialogType.Confirmation,
                          "Delete Record",
                          "Are you sure you want to delete this?")

        AddHandler dlg.DialogClosed,
            Sub(result)

                If result = DialogResultType.Confirm Then
                    DeleteRecord()
                End If

            End Sub

        dlg.ShowDialogBounded()
    End Sub


    Private Sub DeleteRecord()
        MessageBox.Show("Deleted!")
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

    Private dialogOverlay As BaseDialogOverlay


    Public Sub New(targetPanel As Control)
        FormBorderStyle = FormBorderStyle.None
        StartPosition = FormStartPosition.Manual
        ShowInTaskbar = False
        BackColor = Color.White

        With Me
            .AutoSize = True
            .AutoSizeMode = AutoSizeMode.GrowAndShrink
        End With

        dialogOverlay = New BaseDialogOverlay(targetPanel)

        InitializeDialogUI()
    End Sub

    ' ==== CONTENT ======

    Private lblTitle As BaseLabel
    Private lblDescription As BaseLabel
    Private picIcon As PictureBox
    Private btnConfirm As BaseButton
    Private btnCancel As BaseButton
    Private buttonTable As TableLayoutPanel
    Private subContainer As PrimaryFlowLayoutPanel

    Public Property Result As DialogResultType = DialogResultType.None
    Public Event DialogClosed(result As DialogResultType)

    Private Sub InitializeDialogUI()

        subContainer = New PrimaryFlowLayoutPanel()

        With subContainer

            .Padding = New Padding(16)
            .FlowDirection = FlowDirection.TopDown
            .AutoSize = True
            .AutoSizeMode = AutoSizeMode.GrowAndShrink
            .Margin = Padding.Empty

        End With

        ' TITLE
        lblTitle = New BaseLabel()
        With lblTitle
            .SetSmall()
            .Anchor = AnchorStyles.None
            .TextAlign = ContentAlignment.MiddleCenter
        End With

        ' ICON
        picIcon = New PictureBox With {
            .SizeMode = PictureBoxSizeMode.CenterImage,
            .Dock = DockStyle.Top
        }

        ' DESCRIPTION
        lblDescription = New BaseLabel()
        With lblDescription
            .SetSmall()
            .TextAlign = ContentAlignment.MiddleCenter
             .Anchor = AnchorStyles.None
        End With

        ' BUTTON TABLE (2 columns)
        buttonTable = New TableLayoutPanel With {
            .AutoSize = True,
            .AutoSizeMode = AutoSizeMode.GrowAndShrink,
            .ColumnCount = 2
        }

        buttonTable.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50))
        buttonTable.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50))

        ' BUTTONS
        btnConfirm = New BaseButton()
        With btnConfirm
            .Text = Strings.BTN_CONFIRM
            .Dock = DockStyle.Fill
            .SetPrimary()
        End With

        btnCancel = New BaseButton()

        With btnCancel
            .Text = Strings.BTN_CANCEL
            .Dock = DockStyle.Fill
            .SetDanger()
        End With

        AddHandler btnConfirm.Click, Sub()
                                         Result = DialogResultType.Confirm
                                         RaiseEvent DialogClosed(Result)
                                         Me.Close()
                                     End Sub

        AddHandler btnCancel.Click, Sub()
                                        Result = DialogResultType.Cancel
                                        RaiseEvent DialogClosed(Result)
                                        Me.Close()
                                    End Sub

        buttonTable.Controls.Add(btnCancel, 0, 0)
        buttonTable.Controls.Add(btnConfirm, 1, 0)


        Me.Controls.Add(subContainer)

        With subContainer.Controls
            .Add(lblTitle)
            .Add(picIcon)
            .Add(lblDescription)

            .Add(buttonTable)
        End With

    End Sub

    Public Sub SetTitle(text As String)
        lblTitle.Text = text
    End Sub

    Public Sub SetMessage(text As String)
        lblDescription.Text = text
    End Sub

    Public Sub SetIcon(img As Image)
        picIcon.Image = img
    End Sub

    Public Sub SetConfirmVisible(visible As Boolean)
        btnConfirm.Visible = visible
    End Sub

    ' UTILITIES

    Private Sub SyncDialogPosition(sender As Object, e As EventArgs)
        If dialogOverlay Is Nothing OrElse dialogOverlay.IsDisposed Then Return

        Dim center As Point = New Point(
        dialogOverlay.Left + (dialogOverlay.Width - Me.Width) \ 2,
        dialogOverlay.Top + (dialogOverlay.Height - Me.Height) \ 2
    )
        Me.Location = center
    End Sub

    Public Sub ShowDialogBounded()
        dialogOverlay.Show()

        AddHandler dialogOverlay.BoundsChanged, AddressOf SyncDialogPosition

        Me.Owner = dialogOverlay
        Me.Show()

        SyncDialogPosition(Nothing, EventArgs.Empty)
    End Sub

    Protected Overrides Sub OnFormClosed(e As FormClosedEventArgs)
        MyBase.OnFormClosed(e)

        If dialogOverlay IsNot Nothing AndAlso Not dialogOverlay.IsDisposed Then
            dialogOverlay.Hide()
            dialogOverlay.Dispose()
        End If
    End Sub

    Protected Overrides Sub OnDeactivate(e As EventArgs)
        MyBase.OnDeactivate(e)

        If Me.Visible Then
            Me.Focus()
            Me.BringToFront()
        End If
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



