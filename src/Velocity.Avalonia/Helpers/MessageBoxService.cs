using Avalonia.Controls;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia.Models;

namespace Velocity.Avalonia.Helpers;

public static class MessageBox
{
    public static Task<string> ShowDialogAsync(string title, string message, IEnumerable<ButtonDefinition> buttonDefinitions)
    {
        return MessageBoxManager
            .GetMessageBoxCustom(new MessageBoxCustomParams
            {
                CanResize = false,
                ContentMessage = message,
                ContentTitle = title,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ShowInCenter = true,
                ButtonDefinitions = buttonDefinitions,
                Topmost = true,
            }).ShowWindowDialogAsync(App.GetMainWindow());
    }
    
    public static Task<ButtonResult> ShowDialogAsync(string title, string message, ButtonEnum buttonDefinitions = ButtonEnum.Ok)
    {
        return MessageBoxManager
            .GetMessageBoxStandard(new MessageBoxStandardParams()
            {
                CanResize = false,
                ContentMessage = message,
                ContentTitle = title,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ShowInCenter = true,
                ButtonDefinitions = buttonDefinitions,
                Topmost = true,
            }).ShowWindowDialogAsync(App.GetMainWindow());
    }
}

public class MessageBoxButton
{
    public string Name { get; set; } = "OK";

    public bool IsDefault { get; set; }

    public bool IsCancel { get; set; }
}