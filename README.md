# WindowsUtils

WindowsUtilsは.Netの標準ライブラリがサポートしないWindowsの機能などを手軽に利用できるようにするライブラリです。

## StockIcons
Windows Vista以降のシェルアイコンをSystem.Drawing.Iconクラスのインスタンスとして取得できるライブラリです。

### サンプル

```cs
System.Drawing.Icon serverlargeIcon = WindowsControls.StokIcons.ServerLarge;
```

## TaskDialog
Windows Vista以降のタスクダイアログを.Netから利用することができるライブラリです。

### サンプル

```cs
var taskDialogPage = new TaskDialogPage
{
    WindowTitle = "Samples",
    MainInstructionText = "Select sample",
    TaskButtonStyle = TaskDialogTaskButtonStyle.CommandLink,
    MainIcon = TaskDialogIcon.Infomation,
};

var button1 = new TaskButton("選択１");
var button2 = new TaskButton("選択２");
var button3 = new TaskButton("選択３");

basicTaskDialogPage.SetButtons(
    button1,
    button2,
    button3);
        
var taskDialog = new WindowsControls.TaskDialog(taskDialogPage);
taskDialog.DoModal();

if (taskDialog.ClickedButton == button1)
{
  Console.WriteLine("ボタン1が押下されました。");
}
// ...省略...
```
