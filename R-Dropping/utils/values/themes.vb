Public Class Themes

    Public Shared Sub ApplyDarkTheme()
        Colors.Background = Color.FromArgb(30, 30, 30)
        Colors.Primary = Color.FromArgb(45, 45, 48)
        Colors.Secondary = Color.FromArgb(63, 63, 70)
    End Sub

    Public Shared Sub ApplyLightTheme()
        Colors.Background = Color.White
        Colors.Primary = Color.FromArgb(240, 240, 240)
        Colors.Secondary = Color.FromArgb(220, 220, 220)
    End Sub

End Class