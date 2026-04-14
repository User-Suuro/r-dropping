Public Enum DialogType
    Info
    Warning
    [Error]
    Confirmation
End Enum

Public Enum DialogResultType
    None
    Confirm
    Cancel
End Enum

Public Class DialogTypes

    Public Shared Sub Apply(dialog As BaseDialog, type As DialogType, title As String, message As String)

        dialog.SetTitle(title)
        dialog.SetMessage(message)

        Select Case type

            Case DialogType.Info
                dialog.SetIcon(SystemIcons.Information.ToBitmap())
                dialog.SetConfirmVisible(True)
                dialog.SetCancelVisible(False)
                dialog.SetCancelText(Strings.BTN_CONFIRM)

            Case DialogType.Warning
                dialog.SetIcon(SystemIcons.Warning.ToBitmap())
                dialog.SetConfirmVisible(True)
                dialog.SetCancelVisible(False)
                dialog.SetCancelText(Strings.BTN_CONFIRM)

            Case DialogType.Error
                dialog.SetIcon(SystemIcons.Error.ToBitmap())
                dialog.SetConfirmVisible(True)
                dialog.SetCancelVisible(False)
                dialog.SetCancelText(Strings.BTN_CONFIRM)

            Case DialogType.Confirmation
                dialog.SetIcon(SystemIcons.Question.ToBitmap())
                dialog.SetConfirmVisible(True)
                dialog.SetConfirmVisible(True)

        End Select

    End Sub

End Class