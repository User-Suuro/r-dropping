Public Class Colors

    Private Sub New()
    End Sub

    ' Backgrounds
    Public Shared Background As Color
    Public Shared Primary As Color
    Public Shared Secondary As Color

    ' Text on surfaces
    Public Shared OnPrimary As Color
    Public Shared OnSecondary As Color

    ' Accent
    Public Shared Accent As Color

    ' Labels
    Public Shared LblPrimary As Color
    Public Shared LblSecondary As Color
    Public Shared LblMuted As Color

    ' Buttons
    Public Shared BtnPrimary As Color
    Public Shared OnBtnPrimary As Color

    Public Shared BtnSecondary As Color
    Public Shared OnBtnSecondary As Color

End Class


Public Class Themes
    Private Sub New()
    End Sub

    Public Shared Sub ApplyDarkTheme()
        Colors.Background = Color.FromArgb(30, 30, 30)

        Colors.Primary = Color.FromArgb(45, 45, 48)
        Colors.OnPrimary = Color.White

        Colors.Secondary = Color.FromArgb(63, 63, 70)
        Colors.OnSecondary = Color.White

        Colors.Accent = Color.FromArgb(0, 122, 204)

        Colors.LblPrimary = Color.FromArgb(240, 240, 240)
        Colors.LblSecondary = Color.FromArgb(180, 180, 180)
        Colors.LblMuted = Color.FromArgb(120, 120, 120)

        Colors.BtnPrimary = Color.White
        Colors.OnBtnPrimary = Color.Black

        Colors.BtnSecondary = Color.FromArgb(200, 200, 200)
        Colors.OnBtnSecondary = Color.Black
    End Sub

    Public Shared Sub ApplyLightTheme()
        Colors.Background = Color.White

        Colors.Primary = Color.FromArgb(240, 240, 240)
        Colors.OnPrimary = Color.Black

        Colors.Secondary = Color.FromArgb(220, 220, 220)
        Colors.OnSecondary = Color.Black

        Colors.Accent = Color.FromArgb(0, 122, 204)

        Colors.LblPrimary = Color.FromArgb(33, 37, 41)
        Colors.LblSecondary = Color.FromArgb(108, 117, 125)
        Colors.LblMuted = Color.FromArgb(173, 181, 189)

        Colors.BtnPrimary = Color.Black
        Colors.OnBtnPrimary = Color.White

        Colors.BtnSecondary = Color.FromArgb(80, 80, 80)
        Colors.OnBtnSecondary = Color.White
    End Sub

End Class