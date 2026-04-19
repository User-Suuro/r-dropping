Public Enum DialogType
    Info
    Warning
    [Error]
    Confirmation
    Loading
End Enum

Public Enum DialogResultType
    None
    Confirm
    Cancel
End Enum

Public Class DialogTypes

    Public Shared Sub Apply(dialog As BaseDialog, type As DialogType, Optional title As String = "", Optional message As String = "")


        dialog.SetTitle(title)
        dialog.SetMessage(message)

        Select Case type

            Case DialogType.Info
                dialog.SetIcon(SystemIcons.Information.ToBitmap())
                dialog.SetConfirmVisible(True)
                dialog.SetCancelVisible(False)
                dialog.SetConfirmText(Strings.BTN_CONFIRM)
                dialog.SetCancelText(Strings.BTN_CANCEL)

            Case DialogType.Warning
                dialog.SetIcon(SystemIcons.Warning.ToBitmap())
                dialog.SetConfirmVisible(True)
                dialog.SetCancelVisible(False)
                dialog.SetConfirmText(Strings.BTN_CONFIRM)
                dialog.SetCancelText(Strings.BTN_CANCEL)

            Case DialogType.Error
                dialog.SetIcon(SystemIcons.Error.ToBitmap())
                dialog.SetConfirmVisible(True)
                dialog.SetCancelVisible(False)
                dialog.SetConfirmText(Strings.BTN_CONFIRM)
                dialog.SetCancelText(Strings.BTN_CANCEL)

            Case DialogType.Confirmation
                dialog.SetIcon(SystemIcons.Question.ToBitmap())
                dialog.SetConfirmVisible(True)
                dialog.SetCancelVisible(True)
                dialog.SetConfirmText(Strings.BTN_CONFIRM)
                dialog.SetCancelText(Strings.BTN_CANCEL)

            Case DialogType.Loading
                dialog.SetIcon(SystemIcons.Information.ToBitmap())
                dialog.SetTitle("Loading...")
                dialog.SetMessage("Please wait while the operation is being processed.")
                dialog.SetConfirmVisible(False)
                dialog.SetCancelVisible(False)


        End Select

    End Sub
    Public Shared Async Function ShowLoadingUntilAsync(
    dialog As BaseDialog,
    owner As Form,
    asyncAction As Func(Of Task),
    Optional title As String = "Loading",
    Optional message As String = "Please wait...",
    Optional timeoutMilliseconds As Integer = 8000
) As Task(Of Boolean)

        DialogTypes.Apply(dialog, DialogType.Loading, title, message)
        dialog.ShowBaseDialog(owner)

        Await Task.Delay(100)

        Dim actionTask As Task = asyncAction()
        Dim timeoutTask As Task = Task.Delay(timeoutMilliseconds)

        Dim completedTask = Await Task.WhenAny(actionTask, timeoutTask)

        dialog.Close()

        If completedTask Is timeoutTask Then
            Return False
        End If

        Await actionTask
        Return True

    End Function
End Class