Public Class root
    Inherits BasePanel

    Private TopPanel As Panel
    Private SidebarContainer As Panel
    Private Sidebar As FlowLayoutPanel
    Private MainContent As Panel


    Public Sub New()
        Me.Dock = DockStyle.Fill
        Me.BackColor = Color.White
        InitializeUI()
    End Sub

    Private Sub InitializeUI()

        ' Top Panel Container
        TopPanel = New Panel With {
            .Dock = DockStyle.Top,
            .Height = 60
        }

        ' Top Panel Bottom Border

        TopPanel.Controls.Add(New Panel With {
            .Dock = DockStyle.Bottom,
            .Height = 1,
            .BackColor = Color.LightGray
        })

        ' Sidebar Container
        SidebarContainer = New Panel With {
            .Dock = DockStyle.Left,
            .Width = 120,
            .Padding = Padding.Empty
        }

        ' Sidebar

        Sidebar = New FlowLayoutPanel With {
            .Dock = DockStyle.Fill,
            .FlowDirection = FlowDirection.TopDown,
            .WrapContents = False,
            .Padding = Padding.Empty,
            .Margin = Padding.Empty
        }

        Dim homeBtn As New NavBtn("Home", SidebarContainer.Width)
        Dim dropOffBtn As New NavBtn("Drop-off", SidebarContainer.Width)
        Dim employeesBtn As New NavBtn("Employees", SidebarContainer.Width)
        Dim sellersBtn As New NavBtn("Sellers", SidebarContainer.Width)
        Dim courierBtn As New NavBtn("Courier", SidebarContainer.Width)
        Dim pricingBtn As New NavBtn("Pricing", SidebarContainer.Width)
        Dim storageBtn As New NavBtn("Storage", SidebarContainer.Width)

        AddHandler homeBtn.ButtonControl.Click,
        Sub(sender, e)
            showUnavailablePage()
        End Sub

        AddHandler dropOffBtn.ButtonControl.Click,
        Sub(sender, e)
            showUnavailablePage()
        End Sub

        AddHandler employeesBtn.ButtonControl.Click,
        Sub(sender, e)
            showUnavailablePage()
        End Sub

        AddHandler sellersBtn.ButtonControl.Click,
        Sub(sender, e)
            showUnavailablePage()
        End Sub

        AddHandler courierBtn.ButtonControl.Click,
        Sub(sender, e)
            showUnavailablePage()
        End Sub

        AddHandler pricingBtn.ButtonControl.Click,
        Sub(sender, e)
            showUnavailablePage()
        End Sub

        AddHandler storageBtn.ButtonControl.Click,
        Sub(sender, e)
            showUnavailablePage()
        End Sub


        With Sidebar.Controls
            .Add(homeBtn)
            .Add(dropOffBtn)
            .Add(employeesBtn)
            .Add(sellersBtn)
            .Add(courierBtn)
            .Add(pricingBtn)
            .Add(storageBtn)
        End With

        SidebarContainer.Controls.Add(Sidebar)

        ' Sidebar Right Border

        SidebarContainer.Controls.Add(New Panel With {
            .Dock = DockStyle.Right,
            .Width = 1,
            .BackColor = Color.LightGray
        })

        ' ===== MAIN =====
        MainContent = New Panel With {
            .Dock = DockStyle.Fill
        }

        ' ===== ADD ROOT =====
        Me.Controls.Add(MainContent)
        Me.Controls.Add(SidebarContainer)
        Me.Controls.Add(TopPanel)

    End Sub

    Private Sub showUnavailablePage()
        Dim dlg = New BaseDialog()
        DialogTypes.Apply(dlg,
                 DialogType.Info,
                 "Unavailable Page",
                 "Please wait for updates")
        dlg.ShowBaseDialog(Form1.Instance)
    End Sub

End Class

Public Class NavBtn
    Inherits Panel

    Public ReadOnly Property ButtonControl As Button
    Public ReadOnly Property BorderControl As Panel

    Public Sub New(text As String, width As Integer)

        Me.Width = width
        Me.Height = 40
        Me.Margin = Padding.Empty
        Me.Padding = Padding.Empty

        ButtonControl = New Button With {
            .Text = text,
            .Dock = DockStyle.Fill,
            .FlatStyle = FlatStyle.Flat,
            .BackColor = Color.Black,
            .ForeColor = Color.White
        }

        ButtonControl.FlatAppearance.BorderSize = 0

        BorderControl = New Panel With {
            .Dock = DockStyle.Bottom,
            .Height = 1,
            .BackColor = Color.LightGray
        }

        Me.Controls.Add(ButtonControl)
        Me.Controls.Add(BorderControl)

    End Sub
End Class